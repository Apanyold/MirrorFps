using UnityEngine;
using Mirror;
using System;
using CodeBase.Infrastructure;
using CodeBase.Server;

public class DamagerReciver : NetworkBehaviour
{
    [SyncVar]
    public int Health = 100;
    [SyncVar]
    public bool isDead = false;

    public int defaultHealth = 100;

    public Action onDied;

    [Server]
    public void ServerReciveDamage(int damange, NetworkConnectionToClient sender = null)
    {
        Health -= damange;
        Debug.Log(Health);

        if(Health <= 0)
        {
            isDead = true;
            OnReciverDeath(sender);
        }
    }

    [Command]
    public void CmndRevive(NetworkConnectionToClient sender = null)
    {
        Health = defaultHealth;
        isDead = false;
    }

    [Server]
    private void OnReciverDeath(NetworkConnectionToClient sender = null)
    {
        var identity = gameObject.GetComponent<NetworkIdentity>();

        Debug.Log($"Player {identity.connectionToClient} died");

        var message = new SeverMessage()
        {
            message = $"You just killed {identity.connectionToClient}"
        };

        sender.Send(message);

        TargetOnReciverDeath(identity.connectionToClient);
    }

    [TargetRpc]
    public void TargetOnReciverDeath(NetworkConnection target)
    {
        Debug.Log("You just died");
        onDied?.Invoke();
    }
}
