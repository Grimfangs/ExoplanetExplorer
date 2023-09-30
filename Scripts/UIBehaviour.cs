using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject dropdown;
    [SerializeField] Sprite [] sceneImages = new Sprite [5];
    [SerializeField] GameObject bgAudioSlider;
    [SerializeField] GameObject sfxSlider;
    [SerializeField] GameObject cheatsButton;
    [SerializeField] GameObject popupPanel;
    [SerializeField] Sprite [] cheatsButtonStates = new Sprite [2];
    [SerializeField] GameObject saveButton;
    
    int curScene;
    Image panelImg;
    TMP_Dropdown dd;
    bool cheats;
    float bgAudioVolume;
    float sFXVolume;
    Image cheatsButtonImage;
    TMP_Text cheatsButtonText;
    Button saveButtonComponent;
    Slider bgSliderComponent;
    Slider sfxSliderComponent;
    
    void Start()
    {
        curScene = SceneManager.GetActiveScene().buildIndex;
        if (curScene == 1)
        {
            panelImg = panel.GetComponent<Image>();
            dd = dropdown.GetComponent<TMP_Dropdown>();
        }
        else if (curScene == 2)
        {
            popupPanel.SetActive(false);

            cheatsButtonImage = cheatsButton.GetComponent<Image>();
            cheatsButtonText = cheatsButton.GetComponentInChildren<TextMeshProUGUI>(true);

            /* Component[] components = cheatsButton.GetComponentsInChildren(typeof(Component),true);
            foreach(Component component in components)
            {
                Debug.Log(component.ToString());
            } */

            saveButtonComponent = saveButton.GetComponent<Button>();
            saveButtonComponent.interactable = false;

            bgSliderComponent = bgAudioSlider.GetComponent<Slider>();
            sfxSliderComponent = sfxSlider.GetComponent<Slider>();

            bgSliderComponent.value =  PlayerPrefs.GetFloat ("BGAudioVolume", 0.5f);
            sfxSliderComponent.value = PlayerPrefs.GetFloat ("SFXVolume", 0.7f);
            cheats = PlayerPrefs.GetInt("CheatsEnabled",0) == 1 ? true : false;
        }
    }

    void Update()
    {
        ManageKeyInput();
        if (curScene == 2)
        {
            RefreshGUIElements();
        }
    }

    void ManageKeyInput () 
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log ("Escape Pressed");
            GoBack ();
        }
    }

    public void GoBack ()
    {
        if (curScene == 0)
        {
            QuitApp();
        }

        else
        {
            SimulateBackButton();
        }
    }

    public void QuitApp ()
    {
        Application.Quit();
        Debug.Log ("Quitter!");
    }

    public void SimulateBackButton ()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadSpecificLevel (int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void OnChoiceChange ()
    {
        panelImg.sprite = sceneImages[dd.value];
        
    }

    public void LoadSceneFromDropdown ()
    {
        LoadSpecificLevel (dd.value + 3);
    }

    public void ToggleCheats ()
    {
        cheats = !cheats;
        if (cheats)
        {
            popupPanel.SetActive(true);
        }
        SettingsChanged();
    }
    void RefreshGUIElements ()
    {
        if (cheats)
        {
            cheatsButtonText.text = "On";
            cheatsButtonImage.sprite = cheatsButtonStates[1];
        }
        else
        {
            cheatsButtonText.text = "Off";
            cheatsButtonImage.sprite = cheatsButtonStates[0];
        }
    }

    public void ClosePopup ()
    {
        popupPanel.SetActive(false);
    }

    public void SaveSettings ()
    {
        // Write cheats value
        // Write value from sliders
        // in config file
        // Save config file
        bgAudioVolume = bgSliderComponent.value;
        sFXVolume = sfxSliderComponent.value;

        PlayerPrefs.SetFloat ("BGAudioVolume", bgAudioVolume);
        PlayerPrefs.SetFloat ("SFXVolume", sFXVolume);
        PlayerPrefs.SetInt ("CheatsEnabled", cheats?1:0);

        PlayerPrefs.Save();

        saveButtonComponent.interactable = false;
    }

    public void SettingsChanged ()
    {
        saveButtonComponent.interactable = true;
    }
}
