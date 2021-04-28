using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    //Reference variable for health display
    public Text healthText;
    public Image healthImage;

    //Reference variable for coin display
    public Text coinText;

    //Black screen
    public Image blackScreen;

    //Fade in and out variables
    public float fadeSpeed = 1f;
    public bool fadeToBlack;
    public bool fadeFromBlack;

    //Pause Screen
    public GameObject pauseScreen;
    public GameObject optionsScreen;

    //Sliders
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    //Menu
    public string levelSelect;
    public string mainMenu;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(fadeToBlack == true)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            
            if(blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack == true)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    //Resume Button
    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }

    //Open Options Button
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    //Close Options
    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    //Level Select Button
    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
    }

    //Main Menu Button
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    //Set Music Level through Slider
    public void SetMusicLevel()
    {
        AudioManager.instance.SetMusicLevel();
    }

    //Set SFX Level through SLider
    public void SetSFXLevel()
    {
        AudioManager.instance.SetSFXLevel();
    }
}
