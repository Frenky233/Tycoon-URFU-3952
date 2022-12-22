using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildMenu : MonoBehaviour
{

    public StyleSheet style;
    public Button button_0_0;
    public PlacedObjectTypeSO button_0_0_obj;
    GridBuildingSystem3D grid ;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        button_0_0 = root.Q<Button>("0-0");
        button_0_0.clicked += button_0_0_click;

    }

    // Update is called once per frame
    void Update()
    {
        GridBuildingSystem3D.Instance.Building();
        GridBuildingSystem3D.Instance.placedObjectTypeSO = button_0_0_obj;
    }
    void button_0_0_click(){
        GetComponent<HUDHendler>().BuildMenuExitEvent();
        
    }
}
