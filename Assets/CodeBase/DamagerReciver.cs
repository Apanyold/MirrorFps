using UnityEngine;
using Mirror;
using System;

public class DamagerReciver : NetworkBehaviour
{
    [SyncVar]
    public int Health = 100;
    [SyncVar]
    public bool isDead = false;

    public Action OnDeath;

    public int defaultHealth = 100;

    [Command]
    public void CmndReciveDamage(int damange)
    {
        Health -= damange;

        if(Health <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Revive()
    {
        Health = defaultHealth;
        isDead = false;
    }

}
