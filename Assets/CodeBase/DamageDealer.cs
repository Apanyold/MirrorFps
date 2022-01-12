using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageDealer : NetworkBehaviour
{
    public int Damange = 10;

    [Server]
    public void ServerDealDamage(DamagerReciver damagerReciver, NetworkConnectionToClient sender = null)
    {
        damagerReciver.ServerReciveDamage(Damange, sender);
    }
}
