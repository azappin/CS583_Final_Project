using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    //Health variable references
    public int healAmount;
    public bool isFullHeal;

    //Prefab Effect
    public GameObject pickupEffect;

    //Sound reference
    public int soundToPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);

            if (isFullHeal)
            {
                HealthManager.instance.ResetHealth();
            } else
            {
                HealthManager.instance.AddHealth(healAmount);
            }

            Instantiate(pickupEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(soundToPlay);

        }
    }
}
