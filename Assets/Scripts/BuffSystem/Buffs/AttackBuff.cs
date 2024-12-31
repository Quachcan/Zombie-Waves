using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : IBuffHandler
{

    public void Apply(GameObject target, float value)
    {
        var playerCombat = target.GetComponent<PlayerScripts.PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.bulletDamage *= (int)value;
            Debug.Log($"Attack buff applied : {value} damage");
        }
    }

    public void Remove(GameObject target, float value)
    {
        var playerCombat = target.GetComponent<PlayerScripts.PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.bulletDamage /= (int)value;
            Debug.Log($"Attack buff removed");
        }
    }
}
