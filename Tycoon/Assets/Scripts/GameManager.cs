using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake() { instance = this; }

    public MoneySystem money;
    public GridBuildingSystem3D builder;

    void Start()
    {
        GetComponent<MoneySystem>().Init();
    }

}
