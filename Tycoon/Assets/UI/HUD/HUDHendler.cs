using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDHendler : MonoBehaviour
{
    public StyleSheet style;
    public Player player;
    public Button Building;
    public Button LeaderBoard;
    public Button Finance;
    public Button BuildMenuExit;
    public VisualElement BuildMenu;

    private Label Money;
    private Label Income;
    public float uiBaseScreenHeight = 1080f;
    // Start is called before the first frame update

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Building = root.Q<Button>("Building");
        LeaderBoard = root.Q<Button>("LeaderBoard");
        Finance = root.Q<Button>("Finance");
        BuildMenuExit = root.Q<Button>("BuildMenuExit");
        BuildMenu = root.Q<VisualElement>("BuildMenu");
        Money = root.Q<Label>("Money");
        Income = root.Q<Label>("Income");


        Building.clicked += BuildingEvent;
        BuildMenuExit.clicked += BuildMenuExitEvent;
        Finance.clicked += FinanceEvent;
    }

    // Update is called once per frame
    void Update()
    {
        Building.style.fontSize = GetScaledFontSize(35);
        LeaderBoard.style.fontSize = GetScaledFontSize(35);
        Finance.style.fontSize = GetScaledFontSize(35);
        Money.text = player.money.ToString();
        Money.style.fontSize = GetScaledFontSize(35);
        Income.text = player.income.ToString();
        Income.style.fontSize = GetScaledFontSize(12);
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
    void FinanceEvent()
    {
        player.money += 1;
    }
    private int GetScaledFontSize (int baseFontSize) 
    {
        float uiScale = Screen.height / uiBaseScreenHeight;
        int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        return scaledFontSize;
    }
}
