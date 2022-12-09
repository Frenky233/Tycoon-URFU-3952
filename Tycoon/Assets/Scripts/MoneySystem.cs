using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    
    public TMP_Text moneyBank;
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
        moneyBank.GetComponent<TextMeshProUGUI>().text = money.ToString();
    }
}
