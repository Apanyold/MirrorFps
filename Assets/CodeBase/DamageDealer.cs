using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageDealer : NetworkBehaviour
{
    public int Damange = 10;

    [Command]
    public void CmndDealDamage(DamagerReciver damagerReciver)
    {
        damagerReciver.CmndReciveDamage(Damange);
    }
}
