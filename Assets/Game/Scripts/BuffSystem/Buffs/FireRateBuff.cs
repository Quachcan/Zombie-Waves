using System.Collections;
using System.Collections.Generic;
using BuffSystem;
using UnityEngine;

public class FireRateBuff : IBuffHandler
{
    public void Apply(GameObject target, float value)
    {
        var playerCombat = target.GetComponent<PlayerScripts.PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.fireRate += value;
            Debug.Log($"FireRate buff applied : {value}");
        }
    }

    public void Remove(GameObject target, float value)
    {
        var playerCombat = target.GetComponent<PlayerScripts.PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.fireRate += value;
            Debug.Log($"FireRate buff removed");
        }
    }
}
