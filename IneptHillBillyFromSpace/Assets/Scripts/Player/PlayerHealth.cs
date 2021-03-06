using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public Image healthBar;
    public Image o2Bar;
    public float maxHealth = 100f;
    float health;
    float healthDrainRate = 0f;

    public float maxFatigue;
    float fatigue;
    public float fatigueDrainRate = .001f;
    bool isPlayerDead = false;
    void Start()
    {
        health = maxHealth;
        fatigue = maxFatigue;
    }

    void Update()
    {

        healthBar.fillAmount = health / maxHealth;

        if (healthDrainRate > 0)
        {
            health -= healthDrainRate;
        }
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
        o2Bar.fillAmount = fatigue / maxFatigue;

        if (fatigue <= 0)
        {
            //player loses health or enter no oxygen state
            Debug.Log("fatigue is 0");
            healthDrainRate = .05f;
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

        SceneManager.LoadScene("LoseScene");

        return;
    }


    public void takeDamage(float damage)
    {
        health -= damage;
    }




    /*void OnCollisionEnter(Collision col)
    {
        if (col.gameobject.tag == "Enemy")
        {
            Debug.Log("player hit");
            health -= 5;
        }
    }*/
}
