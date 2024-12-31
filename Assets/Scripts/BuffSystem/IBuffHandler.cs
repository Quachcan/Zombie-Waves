using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffHandler 
{
    void Apply(GameObject target, float value);
    void Remove(GameObject target, float value);
}
