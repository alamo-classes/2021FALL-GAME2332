using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{

    public int carrying = 0; // the amount of objects being carried

    public bool canPickUp = false; // if an object that can be picked up is in range

    public GameObject slot1; // storage for an object
    public GameObject slot2;

    public Transform t1; // where the picked up object will go when held
    public Transform t2;

    GameObject inRange = null; // reference to the object that is in range
    

    void Update()
    {


        if (slot1 != null)
        {
            slot1.transform.position = t1.transform.position;
            
        }
                                                                        // sets the position of the held items to stick to player
        if (slot2 != null)
        {
            slot2.transform.position = t2.transform.position;
        }




        if (canPickUp && carrying < 2) // checking if player can pick up something and if inventory is full or not.
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                AddToPlayer(inRange);
            }
        }
        
        if(carrying >= 1) // if player has at least one object
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropItem();
            }
        }

    }



    void AddToPlayer(GameObject obj) // "picks up" object 
    {
        if (slot1 == null)
        {
            slot1 = Instantiate(obj); // create clone of object

            Destroy(obj); // destroy the original

            slot1.GetComponent<Collider>().isTrigger = true; // make collider a trigger so it doesnt interfere with the player

            carrying++; // increment the carry amount
            return;
        }
        else if (obj != slot1)
        {
            slot2 = Instantiate(obj);
 
            Destroy(obj);

            slot2.GetComponent<Collider>().isTrigger = true;

            carrying++;
        }

    }

    void DropItem() // drops the item that is held
    {
        if (slot1 != null) // sees if there is an object in the slot
        {
            slot1.GetComponent<Collider>().isTrigger = false; // make object collidable again
            slot1 = null; // remove item from slot
            carrying--; // remove 1 from carry amount

        }
        else if (slot2 != null)
        {
            slot2.GetComponent<Collider>().isTrigger = false;
            slot2 = null;
            carrying--;
        }
    }


    private void OnTriggerStay(Collider col) // sees what player is collinding with
    {
        if(col.tag == "Collectable") // if it is tagged correctly
        {
            if (col.gameObject != slot1 || col.gameObject != slot2) // if it is not = to an object that is already being carried
            {
                canPickUp = true;         // allow pickup
                inRange = col.gameObject; // store object for reference later
            }
            else // if it is = to one of these then dont allow pickup
            {
                canPickUp = false;
                inRange = null;
            }

        }
    }

    private void OnTriggerExit(Collider col) // sees what exits the players collider
    {
        if (col.tag == "Collectable") // if it was a collectable make it so pickup is false
        {
            canPickUp = false;
            inRange = null;
        }
    }




}
