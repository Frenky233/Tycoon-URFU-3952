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
<<<<<<< Updated upstream
=======

    private Label Money;
    private Label Income;
    private Label BuildMenuName;
    public float uiBaseScreenHeight = 1080f;
>>>>>>> Stashed changes
    // Start is called before the first frame update

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Building = root.Q<Button>("Building");
        LeaderBoard = root.Q<Button>("LeaderBoard");
        Finance = root.Q<Button>("Finance");
        BuildMenuExit = root.Q<Button>("BuildMenuExit");
        BuildMenu = root.Q<VisualElement>("BuildMenu");
<<<<<<< Updated upstream

=======
        Money = root.Q<Label>("Money");
        Income = root.Q<Label>("Income");
        BuildMenuName = root.Q<Label>("BuildMenuName");
>>>>>>> Stashed changes

        Building.clicked += BuildingEvent;
        BuildMenuExit.clicked += BuildMenuExitEvent;


        
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
=======
        Building.style.fontSize = GetScaledFontSize(35);
        LeaderBoard.style.fontSize = GetScaledFontSize(35);
        Finance.style.fontSize = GetScaledFontSize(35);
        Money.text = player.money.ToString();
        Money.style.fontSize = GetScaledFontSize(35);
        BuildMenuName.style.fontSize = GetScaledFontSize(25);
        if(player.income < 0){
            Income.style.color = Color.red;
        }
        Income.text = player.income.ToString();
        Income.style.fontSize = GetScaledFontSize(25);
>>>>>>> Stashed changes
    }
    void BuildingEvent()
    {
        GetComponent<BuildMenu>().GridBuildingSystem3D.DeselectObjectType();
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
<<<<<<< Updated upstream
=======
    void FinanceEvent()
    {
        player.money += player.income;
    }
    private int GetScaledFontSize (int baseFontSize) 
    {
        float uiScale = Screen.height / uiBaseScreenHeight;
        int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        return scaledFontSize;
    }
>>>>>>> Stashed changes
}
