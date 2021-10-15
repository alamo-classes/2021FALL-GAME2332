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
         //=====================================================================================
         // Do you need this?
         // I think you could get away with just the gameobject references
         // Also you don't need to set the position every update if it's a child of the slot
         // - JR
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
            //Input.GetKeyDown(KeyCode.F)
            if (Input.GetButtonDown("PickUp"))
               {
                   AddToPlayer(inRange);
               }
        }
        
        if(carrying >= 1) // if player has at least one object
        {
            //Input.GetKeyDown(KeyCode.G)
            if (Input.GetButtonDown("Drop"))
            {
                DropItem();
            }
        }

    }



    void AddToPlayer(GameObject obj) // "picks up" object 
    {
        if (slot1 == null)
        {
         //====================================================================
         // look into transform.setparent in unity api
         //    Then you would need to disable the collider and RB
         // - JR
            slot1 = Instantiate(obj); // create clone of object

            Destroy(obj); // destroy the original

            slot1.GetComponent<Collider>().enabled = false; // Disable the collider

            carrying++; // increment the carry amount
            return;
        }
        else if (obj != slot1)
        {
            slot2 = Instantiate(obj);
 
            Destroy(obj);

            slot2.GetComponent<Collider>().enabled = true;

            carrying++;
        }

    }

    void DropItem() // drops the item that is held
    {
         //============================================================
         // if the object is a child you have disconnect it by setting the parent to null
         //       transform.parent == null or SetParent ( not sure if it will accept null as a param )
         // then reenable the collider and RB
         // Maybe set the item's position to a specific one?
         // - JR
        if (slot1 != null) // sees if there is an object in the slot
        {
            slot1.GetComponent<Collider>().enabled = true; // make object collidable again
            slot1 = null; // remove item from slot
            carrying--; // remove 1 from carry amount

        }
        else if (slot2 != null)
        {
            slot2.GetComponent<Collider>().enabled = false;
            slot2 = null;
            carrying--;
        }
    }


    private void OnTriggerStay(Collider col) // sees what player is collinding with
    {
        if(col.tag == "Collectable") // if it is tagged correctly
        {
            //==============================================================
            // Can't you just use the carrying var to see if you have an empty slot?
            // - JR
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
