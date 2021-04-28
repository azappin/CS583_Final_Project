using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private Vector3 respawnPosition;
    //private Vector3 cameraRespawnPosition;

    //Prefab Death Effect
    public GameObject deathEffect;

    //Coin total reference
    public int currentCoins;

    //Level End Music Reference;
    public int levelEndMusic = 1;

    public string levelToLoad;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Make Cursor Disappear
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Save respawn location
        respawnPosition = PlayerController.instance.transform.position;
        //cameraRespawnPosition = CameraController.instance.transform.position;

        //Start with zero coins
        AddCoins(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            PauseUnpause();
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());

        //Drop Health to Zero when Played is Killed / Falls off the map
        HealthManager.instance.PlayerKilled();
    }

    public IEnumerator RespawnCo()
    {
        //Make Player disappear
        PlayerController.instance.gameObject.SetActive(false);

        //Deactive Cinemachine Brain
        CameraController.instance.cmBrain.enabled = false;

        //Fade to Black
        UIManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);

        //Wait for 2 seconds to respawn
        yield return new WaitForSeconds(2f);

        //Fade From Black
        UIManager.instance.fadeFromBlack = true;

        //Move player and camera to respawn locatation and make Player visible again
        PlayerController.instance.transform.position = respawnPosition;

        //Reactive Cinemachine Brain
        CameraController.instance.cmBrain.enabled = true;

        //CameraController.instance.transform.position = cameraRespawnPosition;
        PlayerController.instance.gameObject.SetActive(true);

        //Reset Health
        HealthManager.instance.ResetHealth();

    }

    //Method to save a new spawn point when the Player reaches a checkpoint
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }

    //Methods to Add Counts to total
    public void AddCoins(int coinsToAdd)
    {
        currentCoins = currentCoins + coinsToAdd;
        UIManager.instance.coinText.text = currentCoins.ToString();
    }

    //Method to Pause the Game
    public void PauseUnpause()
    {
        if (UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            //Make Cursor Disappear
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        } else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            UIManager.instance.CloseOptions();
            Time.timeScale = 0f;

            //Make Cursor Appear
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void GameWon()
    {
        //Make Cursor Appear
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public IEnumerator LevelEndCo()
    {

        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.stopMove = true;

        yield return new WaitForSeconds(3f);
        Debug.Log("Level Ended");

        SceneManager.LoadScene(levelToLoad);
    }

}
