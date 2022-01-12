using UnityEngine;
using Mirror;
using System.Collections;
using CodeBase.Player;

public class PlayerController : NetworkBehaviour
{
    public CharacterController characterController;
    public float MovementSpeed = 10;

    [HideInInspector]
    public ParticleSystem flash;
    public ParticleSystem impact;

    public Transform gunTransform;
    public DamageDealer damageDealer;

    public GameObject playerCamera;

    [TargetRpc]
    public void Init()
    {
        playerCamera.AddComponent<Camera>();
        var clientController = gameObject.AddComponent<ClientPlayerController>();
        clientController.Init(this);
    }

    public void Move(Vector3 movementVector)
    {
        characterController.Move(MovementSpeed * movementVector * Time.deltaTime);
    }

    public void Shoot()
    {
        flash.Play();

        RaycastHit hit;

        if (Physics.Raycast(gunTransform.transform.position, gunTransform.transform.forward, out hit))
        {
            if (hit.transform.TryGetComponent(out DamagerReciver damagerReciver))
                damageDealer.CmndDealDamage(damagerReciver);

            CmdSpawnImpact(hit.point);
        }
    }

    public void RotateGun(Quaternion rotation)
    {
        gunTransform.localRotation = rotation;
    }

    public void RotatePlayer(Vector3 rotation)
    {
        transform.Rotate(rotation);
    }

    [Command]
    private void CmdSpawnImpact(Vector3 postiion)
    {
        var impactGo = Instantiate(impact, postiion, new Quaternion());
        NetworkServer.Spawn(impactGo.gameObject);

        StartCoroutine(DestroyWithDelay(impactGo.gameObject, 0.2f));
    }

    private IEnumerator DestroyWithDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        NetworkServer.Destroy(gameObject);
    }
}