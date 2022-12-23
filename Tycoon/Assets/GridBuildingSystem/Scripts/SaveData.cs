using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public class BuildPos{
        int x;
        int y;
        int type;

    }
    public int year;
    public int money;
    public List<BuildPos> Buildings;

    public SaveData(Player player){
        year = player.year;
        money = player.money;
        
        
    }
}
