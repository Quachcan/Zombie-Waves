using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField]
    private int totalCoins = 0; 

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        NotifyOnCointChange();
    }

    public void SpendCoin(int amount)
    {
        if (totalCoins < amount)
        {
            totalCoins -= amount;
            NotifyOnCointChange();
        }
        else
        {
            Debug.Log("You don't have enough coin");
        }
    }

    private void NotifyOnCointChange()
    {
        UIManager.Instance.UpdateCoins(totalCoins);
    }

}
