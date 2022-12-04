using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Buttons : MonoBehaviour
{
    public AudioSource sound;
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        newGameButton = root.Q<Button>("buttonNewGame");
        loadGameButton = root.Q<Button>("buttonLoadGame");
        settingsButton = root.Q<Button>("buttonSettings");
        exitButton = root.Q<Button>("ButtonExit");

        newGameButton.clicked += NewGameButtonClick;
        loadGameButton.clicked += LoadGameButtonClick;
        settingsButton.clicked += SettingsButtonClick;
        exitButton.clicked += ExitButtonClick;


    }
    void NewGameButtonClick()
    {
        ClickSoud();
    }
    void LoadGameButtonClick()
    {
        ClickSoud();
    }
    void SettingsButtonClick()
    {
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

}
