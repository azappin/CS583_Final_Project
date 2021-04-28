using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    //Checkpoint references
    public GameObject checkpointOn;
    public GameObject checkpointOff;

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

    //Updates spawn position if the Player reaches a checkpoint
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Set spawnpoint if Player reaches current checkpoint
            GameManager.instance.SetSpawnPoint(transform.position);

            //Playsound
            AudioManager.instance.PlaySFX(soundToPlay);

            //Find all checkpoints and switch them off if they are not the current one
            Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
            for(int i = 0; i < allCheckpoints.Length; i++)
            {
                allCheckpoints[i].checkpointOff.SetActive(true);
                allCheckpoints[i].checkpointOn.SetActive(false);
            }

            //Switch current checkpoint on
            checkpointOff.SetActive(false);
            checkpointOn.SetActive(true);
        }
    }

}
