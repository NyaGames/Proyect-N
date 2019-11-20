using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPositionWithLatLon : MonoBehaviour, IPunObservable
{
    private Vector2d latLonCurrent;
    private Vector2 latlonSend;
    private Vector2 latlonReceived;
    private PhotonView photonView;

    //private AbstractMap map;
    private Drop dropData; 

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        latlonSend = new Vector2();
        latlonReceived = new Vector2();
        //map = LocationProviderFactory.Instance.mapManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)//SI soy el gm, calculo la posición del drop
        {
            var map = LocationProviderFactory.Instance.mapManager;
            latLonCurrent = CalculateDropLatLon(map.CenterLatitudeLongitude) + map.CenterLatitudeLongitude;
            Debug.Log("Centro mapa: " + map.CenterLatitudeLongitude + "/Drop: " + latLonCurrent);
        }
    }

    public void UpdatePosition()
    {
        latlonSend.x = (float)latLonCurrent.x;
        latlonSend.y = (float)latLonCurrent.y;
        if (photonView.IsMine)//SI soy el gm, calculo la posición del drop
        {
            //var map = LocationProviderFactory.Instance.mapManager;
            //Debug.Log("Centro mapa: " + map.CenterLatitudeLongitude + "/Drop: " + latLonCurrent);
        }
        else
        {
            var map = LocationProviderFactory.Instance.mapManager;
            Vector2d v = new Vector2d(latlonReceived.x, latlonReceived.y);
            transform.localPosition = map.GeoToWorldPosition(v);
        }
    }

    private void LateUpdate()
    {
        if (latLonCurrent != null)
        {
            UpdatePosition();
        }
    }

    public Vector2d CalculateDropLatLon(Vector2d center)
    {
        Vector2d latlon = transform.GetGeoPosition(center, 1);
        return latlon;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (photonView.IsMine) //Si he creado yo(GM) la zona, yo mando la info
            {
                stream.SendNext(latlonSend);
            }
            else
            {
                //NO mando nada,porque soy un jugador normal
            }
        }
        else if (stream.IsReading)
        {
            if (photonView != null && !photonView.IsMine)//SI me llega info de la zona, la updateo
            {
                latlonReceived = (Vector2)stream.ReceiveNext();
            }
        }
    }

}
