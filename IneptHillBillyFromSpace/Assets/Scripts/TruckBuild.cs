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

   
    void Update()
    {
        if (cabin)
        {
            cabinGameobject.SetActive(true);
        }

        if (chassis)
        {
            chassisGameobject.SetActive(true);
        }

        if (engine)
        {
            engineGameobject.SetActive(true);
        }

        if (Swheel)
        {
            SwheelGameobject.SetActive(true);
        }

        if (tires)
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
        }
    }
}
