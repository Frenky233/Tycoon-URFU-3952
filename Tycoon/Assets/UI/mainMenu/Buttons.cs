using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Buttons : MonoBehaviour
{
    public AudioSource sound;
    public Object Object;
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button exitButton;
    public VisualElement menu;
    public VisualElement MainMenu;
    public Button menuExit;
    public float uiBaseScreenHeight = 500f;



    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        newGameButton = root.Q<Button>("buttonNewGame");
        loadGameButton = root.Q<Button>("buttonLoadGame");
        settingsButton = root.Q<Button>("buttonSettings");
        exitButton = root.Q<Button>("ButtonExit");
        MainMenu = root.Q<VisualElement>("Container");
        menu = root.Q<VisualElement>("Menu");
        menuExit = root.Q<Button>("menuExit");
        menu.style.display = DisplayStyle.None;
        
        newGameButton.clicked += NewGameButtonClick;
        loadGameButton.clicked += LoadGameButtonClick;
        settingsButton.clicked += SettingsButtonClick;
        exitButton.clicked += ExitButtonClick;
        menuExit.clicked +=menuExitEvent;
    }
    private void Update() 
    {
        newGameButton.style.fontSize = GetScaledFontSize(80);
        loadGameButton.style.fontSize = GetScaledFontSize(80);
        settingsButton.style.fontSize = GetScaledFontSize(80);
        exitButton.style.fontSize = GetScaledFontSize(80);
        menuExit.style.fontSize = GetScaledFontSize(50);
    }
    void NewGameButtonClick()
    {
        ClickSoud();
        
    }
    void LoadGameButtonClick()
    {
        ClickSoud();
        menuOpenEvemt();
    }
    void SettingsButtonClick()
    {
        menuOpenEvemt();
        ClickSoud();

    }
    void ExitButtonClick()
    {
        ClickSoud();
        Application.Quit();
    }
    void ClickSoud()
    {
        sound.Play();
    }
    void menuOpenEvemt()
    {
        menu.style.display = DisplayStyle.Flex;
        MainMenu.style.display = DisplayStyle.None;
        menu.style.opacity = 1;
        MainMenu.style.opacity = 0;
    }
    void menuExitEvent()
    {
        ClickSoud();
        menu.style.display = DisplayStyle.None;
        MainMenu.style.display = DisplayStyle.Flex;
        menu.style.opacity = 0;
        MainMenu.style.opacity = 1;
    }
    private int GetScaledFontSize (int baseFontSize) 
    {
        float uiScale = Screen.height / uiBaseScreenHeight;
        int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        return scaledFontSize;
    }

}
