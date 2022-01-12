using UnityEngine;
using Mirror;
using CodeBase.Client;
using CodeBase.Infrastructure;

namespace CodeBase.Player
{
    public class PlayerController : NetworkBehaviour
    {
        public CharacterController characterController;
        public float MovementSpeed = 10;

        [HideInInspector]
        public ParticleSystem flash;
        public ParticleSystem impact;

        public Transform gunTransform;
        public DamageDealer damageDealer;
        public DamagerReciver damagerReciver;

        public GameObject playerCamera;

        [TargetRpc]
        public void TargetInit()
        {
            playerCamera.AddComponent<Camera>();

            var clientController = gameObject.AddComponent<ClientPlayerController>();
            damagerReciver = gameObject.GetComponent<DamagerReciver>();

            damagerReciver.onDied += OnPlayerDied;

            clientController.Init(this);
        }

        [Server]
        public void ServerInit()
        {

        }

        public void OnPlayerDied()
        {
            damagerReciver.CmndRevive();
            transform.position = Game.GetStartPosition().position;
        }

        public void Move(Vector3 movementVector)
        {
            characterController.Move(MovementSpeed * movementVector * Time.deltaTime);
        }

        [Client]
        public void Shoot()
        {
            flash.Play();
            CmndPlayFlash();
            CmndShoot();
        }

        [Command]
        public void CmndShoot(NetworkConnectionToClient sender = null)
        {
            RaycastHit hit;

            if (Physics.Raycast(gunTransform.transform.position, gunTransform.transform.forward, out hit))
            {
                if (hit.transform.TryGetComponent(out DamagerReciver damagerReciver))
                    damageDealer.ServerDealDamage(damagerReciver, sender);

                ClientSpawnImpact(hit.point);
            }
        }

        [Command]
        public void CmndPlayFlash()
        {
            ClientPlayFlash();
        }

        [ClientRpc]
        public void ClientPlayFlash()
        {
            flash.Play();
        }

        public void RotateGun(Quaternion rotation)
        {
            playerCamera.transform.localRotation = rotation;
        }

        public void RotatePlayer(Vector3 rotation)
        {
            transform.Rotate(rotation);
        }

        [ClientRpc]
        private void ClientSpawnImpact(Vector3 postiion)
        {
            var impactGo = Instantiate(impact, postiion, new Quaternion());
            Destroy(impactGo, 0.2f);
        }
    }
}