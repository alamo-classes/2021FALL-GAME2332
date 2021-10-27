using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f;
    float health;

    public float fatigue;
    public float fatigueDrainRate;
    bool isPlayerDead = false;
    void Start()
    {
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }

    void Update()
    {
        health -= .1f;
        healthBar.fillAmount = health / maxHealth;
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
