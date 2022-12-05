using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDHendler : MonoBehaviour
{
    public Button Building;
    public Button LeaderBoard;
    public Button Finance;
    public Button BuildMenuExit;
    public VisualElement BuildMenu;
    // Start is called before the first frame update

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Building = root.Q<Button>("Building");
        LeaderBoard = root.Q<Button>("LeaderBoard");
        Finance = root.Q<Button>("Finance");
        BuildMenuExit = root.Q<Button>("BuildMenuExit");
        BuildMenu = root.Q<VisualElement>("BuildMenu");


        Building.clicked += BuildingEvent;
        BuildMenuExit.clicked += BuildMenuExitEvent;


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void BuildingEvent()
    {
        BuildMenu.style.display = DisplayStyle.Flex;
        Building.style.display = DisplayStyle.None;
        LeaderBoard.style.display = DisplayStyle.None;
        Finance.style.display = DisplayStyle.None;
    }
    void BuildMenuExitEvent()
    {
        BuildMenu.style.display = DisplayStyle.None;
        Building.style.display = DisplayStyle.Flex;
        LeaderBoard.style.display = DisplayStyle.Flex;
        Finance.style.display = DisplayStyle.Flex;
    }
}
