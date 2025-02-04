using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "BuffSystem/BuffConfig")]
public class BuffConfig : ScriptableObject
{
    public string BuffName;
    public float value;
    public Type BuffType;
    
}

