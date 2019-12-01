using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArePlayersInsideZone : MonoBehaviour
{
    SphereCollider zoneCollider;

    public void Awake()
    {
        zoneCollider = GetComponent<SphereCollider>();
    }

    public void Update()
    {
        foreach(GameObject g in GamemasterManager.Instance.playersViewsList)
        {
            if (!zoneCollider.bounds.Contains(g.transform.position)) //Si el jugador está fuera de la zona y no está avisado, se le avisa
            {
                g.GetComponent<OutOfZoneInfo>().insideZone = false;
            }else
            {
                g.GetComponent<OutOfZoneInfo>().insideZone = true;
            }

        }
    }

    /*public void OnTriggerEnter(Collider other) //Algún jugador entra a la zona
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<OutOfZoneInfo>().insideZone = true;
        }
        
    }

    public void OnTriggerExit (Collider other) //Algún jugador sale de la zona
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<OutOfZoneInfo>().insideZone = false;
        }
    }

    public void OnTriggerStay(Collider other) //Algún jugador está dentro de la zona
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<OutOfZoneInfo>().insideZone = true;
        }

    }*/



}
