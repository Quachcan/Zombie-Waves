using System.Collections;
using System.Collections.Generic;
using BuffSystem;
using UnityEngine;

public class MovementSpeedBuff : IBuffHandler
{
    public void Apply(GameObject target, float value)
    {
        var playerMovement = target.GetComponent<PlayerScripts.PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.movementSpeed += value;
            Debug.Log($"MovementSpeed buff applied : {value}");
        }
    }

    public void Remove(GameObject target, float value)
    {
        var playerMovement = target.GetComponent<PlayerScripts.PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.movementSpeed -= value;
            Debug.Log($"MovementSpeed buff removed");
        }
    }
}
