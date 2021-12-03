using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectibleType { CABIN, ENGINE, CHASSIS, SWHEEL, TIRES };

    public CollectibleType collectType;
    public bool            isPickedUp = false;

    //private void OnCollisionEnter(Collision col)
    //{
    //    Destroy(this.gameObject);
    //    Debug.Log("Deleted " + gameObject.name);
    //}
}
