using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public double health;
    public double fatigue;
    public double fatigueDrainRate;
    bool isPlayerDead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!(fatigueDrainRate <= 0))
        {
            fatigueDrain();
        }
            playerStatus();
    }

    public void fatigueDrain()
    {
        //while not in oxygen area drain
        fatigue -= fatigueDrainRate;
        if (fatigue <= 0)
        {
            //player loses health or enter no oxygen state
            Debug.Log("fatigue is 0");
            health = 0;
            fatigueDrainRate = 0;
        }

    }
    public void playerStatus()
    {
        if (isPlayerDead == true)
            return;

        if (health > 0)
        {
            //player is alive
            return;
        }
        isPlayerDead = true;
        Debug.Log("Player is dead");
        return;
    }
}
