using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Collectable;

public class TruckBuild : MonoBehaviour
{
    public bool cabin = false;
    public bool chassis = false;
    public bool engine = false;
    public bool Swheel = false;
    public bool tires = false;

    public GameObject cabinGameobject;
    public GameObject chassisGameobject;
    public GameObject engineGameobject;
    public GameObject SwheelGameobject;
    public GameObject tiresGameobject;

    private PlayerPickUp playerInventory;

    void Awake()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickUp>();
    }
   
    void Update()
    {
        if (cabin && !cabinGameobject.activeInHierarchy )
        {
            cabinGameobject.SetActive(true);
        }

        if (chassis && !chassisGameobject.activeInHierarchy )
        {
            chassisGameobject.SetActive(true);
        }

        if (engine && !engineGameobject.activeInHierarchy)
        {
            engineGameobject.SetActive(true);
        }

        if (Swheel && !SwheelGameobject.activeInHierarchy)
        {
            SwheelGameobject.SetActive(true);
        }

        if (tires && !tiresGameobject.activeInHierarchy)
        {
            tiresGameobject.SetActive(true);
        }
        
    }




    private void OnTriggerEnter(Collider col)
    {
        if ( col.tag == "Collectable" )
        {
            CollectibleType collectType = col.GetComponent<Collectable>().collectType;

            switch(collectType)
            {
                case CollectibleType.CABIN:     cabin = true;       break;
                case CollectibleType.CHASSIS:   chassis = true;     break;
                case CollectibleType.ENGINE:    engine = true;      break;
                case CollectibleType.SWHEEL:    Swheel = true;      break;
                case CollectibleType.TIRES:     tires = true;       break;
            }

            Destroy(col.gameObject);

            if ( playerInventory.carrying > 0 )
            {
               playerInventory.carrying--;
            }
        }
    }
}
