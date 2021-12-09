using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public float AttackSpeed;
    public float attackDamage;

    float time = 0;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }



    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Player" && time >= AttackSpeed)
        {
            print("Player in range");

            PlayerHealth pHealth = col.gameObject.GetComponent<PlayerHealth>();

            pHealth.takeDamage(attackDamage);
            playAttackAnimation();
            time = 0;
        }
    }

    void playAttackAnimation()
    {
        if ( animator != null )
        {
            animator.SetTrigger( "Attack" );
        }
    }

}
