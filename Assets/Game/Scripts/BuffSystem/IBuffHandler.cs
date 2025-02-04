using UnityEngine;

namespace BuffSystem
{
    public interface IBuffHandler 
    {
        void Apply(GameObject target, float value);
        void Remove(GameObject target, float value);
    }
}
