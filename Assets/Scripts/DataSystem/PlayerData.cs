using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int currentLevel;
    public int currentExp;
    public int expToNextLevel;
    public float[] position;

    public PlayerData(PlayerScripts.Player player)
    {
        currentLevel = player.currentLevel;
        currentExp = player.currentExp;
        expToNextLevel = player.expTotNextLevel;

        // Lưu vị trí của Player
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
