using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneySystem : MonoBehaviour
{
    public int defaultMoney;
    public int money;

    public void Init()
    {
        money = defaultMoney;
        UpdateUI();
    }

    public void GainMoney(int val)
    {
        money +=val;
        UpdateUI();
    }

    public void UseMoney(int val)
    {
        money -= val;
        UpdateUI();
    }

    public bool EnoughMoney(int val)
    {
        if (val <= money)
            return true;
        else
            return false;
    }

    void UpdateUI()
    {
        
    }
}
