using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{

    public int carrying = 0; // the amount of objects being carried

    public bool canPickUp = false; // if an object that can be picked up is in range

    public Transform t1; // where the picked up object will go when held
    public Transform t2;
    public Transform dropPos; // place where object will be dropped

    public GameObject slot1; // references to gameobjects held
    public GameObject slot2;

    GameObject inRange = null; // reference to the object that is in range
    

    void Update()
    {

        if(slot1 != null)
            slot1.transform.position = t1.transform.position; // needed to make sure that objects held dont "drag" behind the player when moving
        if (slot2 != null)
            slot2.transform.position = t2.transform.position;





        if (canPickUp && carrying < 2) // checking if player can pick up something and if inventory is full or not.
        {

            if (Input.GetButtonDown("PickUp"))
               {
                   AddToPlayer(inRange);
               }
        }
        
        if(carrying >= 1) // if player has at least one object
        {

            if (Input.GetButtonDown("Drop"))
            {
                DropItem();
            }
        }

    }



    void AddToPlayer( GameObject obj ) // "picks up" object 
    {
        obj.GetComponent<Collider>().enabled = false; // Disable the collider
        obj.GetComponent<GravityBody>().enabled = false; // disable physics
        obj.GetComponent<Collectable>().isPickedUp = true;
        obj.transform.SetParent(transform); // set player as parent
        carrying++; // increment the carry amount

        if (carrying == 0)
        {
            slot1 = obj;
            obj.transform.position = t1.position; // set position to player holding spot
        }
        else if (carrying == 1)
        {
            slot2 = obj;
            obj.transform.position = t2.position; // set position to player holding spot
        }

    }

    void DropItem() // drops the item that is held
    {
        GameObject droppedObj = null;

        if (carrying == 2)
        {
            droppedObj = slot2;
            slot2 = null; // make storage empty for new item
        }
        else if (carrying == 1)
        {
            droppedObj = slot1;
            slot1 = null; // make storage empty for new item
        }

        droppedObj.GetComponent<Collider>().enabled = true; // reenable the collider
        droppedObj.GetComponent<GravityBody>().enabled = true; // reenable physics
        droppedObj.GetComponent<Collectable>().isPickedUp = false;
        droppedObj.transform.SetParent(null); // make object child to nothing 
        droppedObj.transform.position = dropPos.position; // set position to be dropped
        carrying--; // decrement the carry amount
   }


    private void OnTriggerStay(Collider col) // sees what player is collinding with
    {
        if(col.tag == "Collectable") // if it is tagged correctly
        {
            if (slot1 == null || slot2 == null) // 
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
        if (col.tag == "Collectable" ) // if it was a collectable make it so pickup is false
        {
            canPickUp = false;
            inRange = null;
        }
    }




}
