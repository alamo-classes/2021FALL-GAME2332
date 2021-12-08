using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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



        if(cabin && chassis && engine && Swheel && tires)
        {
            SceneManager.LoadScene("WinScene");
        }

        
    }




    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "cabin")
        {
            cabin = true;
            Destroy(col.gameObject);
        }

        if (col.tag == "chassis")
        {
            chassis = true;
            Destroy(col.gameObject);
        }

        if (col.tag == "engine")
        {
            engine = true;
            Destroy(col.gameObject);
        }

        if (col.tag == "Swheel")
        {
            Swheel = true;
            Destroy(col.gameObject);
        }

        if (col.tag == "tires")
        {
            tires = true;
            Destroy(col.gameObject);
        }
    }
}
