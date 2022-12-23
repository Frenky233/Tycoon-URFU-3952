using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildMenu : MonoBehaviour
{
    public GridBuildingSystem3D GridBuildingSystem3D;
    public StyleSheet style;
    public Button button_0_0;
    public Button button_0_1;
    public Button button_0_2;
    public Button button_0_3;
    public Button button_0_4;
    

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        button_0_0 = root.Q<Button>("0-0");
        button_0_1 = root.Q<Button>("0-1");
        button_0_2 = root.Q<Button>("0-2");
        button_0_3 = root.Q<Button>("0-3");
        button_0_4 = root.Q<Button>("0-4");



        button_0_0.clicked += button_0_0_click;
        button_0_1.clicked += button_0_1_click;
        button_0_2.clicked += button_0_2_click;
        button_0_3.clicked += button_0_3_click;
        button_0_4.clicked += button_0_4_click;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void button_0_0_click(){
        GetComponent<HUDHendler>().BuildMenuExitEvent();
        GridBuildingSystem3D.GetObjectID(0);
    }
    void button_0_1_click(){
        GetComponent<HUDHendler>().BuildMenuExitEvent();
        GridBuildingSystem3D.GetObjectID(1);
    }
    void button_0_2_click(){
        GetComponent<HUDHendler>().BuildMenuExitEvent();
        GridBuildingSystem3D.GetObjectID(2);
    }
    void button_0_3_click(){
        GetComponent<HUDHendler>().BuildMenuExitEvent();
        GridBuildingSystem3D.GetObjectID(3);
    }
    void button_0_4_click(){
        GetComponent<HUDHendler>().BuildMenuExitEvent();
        GridBuildingSystem3D.GetObjectID(4);
    }
}
