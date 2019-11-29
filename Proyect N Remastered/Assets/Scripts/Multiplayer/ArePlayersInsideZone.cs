using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArePlayersInsideZone : MonoBehaviour
{

    public void OnTriggerEnter(Collider other) //Algún jugador entra a la zona
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

    }


}
