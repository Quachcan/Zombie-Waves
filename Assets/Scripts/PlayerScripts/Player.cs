using System;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            if (Managers.PlayerManager.Instance != null)
            {
                Managers.PlayerManager.Instance.RegisterPlayer(this.gameObject);
            }
            else
            {
                Debug.LogError("PlayerManager.Instance is null! Make sure PlayerManager exists in the scene.");
            }
        }
    }
}
