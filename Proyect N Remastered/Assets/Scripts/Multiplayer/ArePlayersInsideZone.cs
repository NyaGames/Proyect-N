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

        if (!zoneCollider.bounds.Contains(GameManager.Instance.myPlayer.transform.position)) //Si el jugador está fuera de la zona y no está avisado, se le avisa
        {
            GameManager.Instance.myPlayer.GetComponent<OutOfZoneInfo>().insideZone = false;
        }else
        {
            GameManager.Instance.myPlayer.GetComponent<OutOfZoneInfo>().insideZone = true;
        }
    }


}
