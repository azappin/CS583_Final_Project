using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    //Instance reference
    public static HealthManager instance;

    //Health Variables References
    public int currentHealth;
    public int maxHealth;

    //Invicible Variable References
    public float invincibleLength = 2f;
    private float invincibleCounter;

    //Reference to all health graphics
    public Sprite[] healthBarImages;

    //Reference to Sound to Play
    public int soundToPlayHurt;
    public int soundToPlayDead;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            for (int i = 0; i < PlayerController.instance.playerPieces.Length; i++)
            {
                if(Mathf.Floor(invincibleCounter *5f) % 2 == 0)
                {
                    PlayerController.instance.playerPieces[i].SetActive(true);
                } else
                {
                    PlayerController.instance.playerPieces[i].SetActive(false);
                }

                if(invincibleCounter <= 0)
                {
                    PlayerController.instance.playerPieces[i].SetActive(true);
                }
            }

        }
    }

    //Method to inflict damage to the Player
    public void Hurt()
    {
        if (invincibleCounter <= 0)
        {
            currentHealth = currentHealth - 1;
            AudioManager.instance.PlaySFX(soundToPlayHurt);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.instance.Respawn();
            }
            else
            {
                PlayerController.instance.Knockback();
                invincibleCounter = invincibleLength;

                for(int i = 0; i < PlayerController.instance.playerPieces.Length; i++)
                {
                    PlayerController.instance.playerPieces[i].SetActive(false);
                }
            }

            UpdateUI();
        }


    }

    //Reset Health
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UIManager.instance.healthImage.enabled = true;
        UpdateUI();
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        //Display current health as Text
        UIManager.instance.healthText.text = currentHealth.ToString();

        //Display current health image
        switch (currentHealth)
        {
            case 5:
                UIManager.instance.healthImage.sprite = healthBarImages[4];
                break;
            case 4:
                UIManager.instance.healthImage.sprite = healthBarImages[3];
                break;
            case 3:
                UIManager.instance.healthImage.sprite = healthBarImages[2];
                break;
            case 2:
                UIManager.instance.healthImage.sprite = healthBarImages[1];
                break;
            case 1:
                UIManager.instance.healthImage.sprite = healthBarImages[0];
                break;
            case 0:
                UIManager.instance.healthImage.enabled = false;
                break;
        }

    }

    public void PlayerKilled()
    {
        currentHealth = 0;
        AudioManager.instance.PlaySFX(soundToPlayDead);
        UpdateUI();
    }

}
