using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;

public class ZonePositionWithLatLon : MonoBehaviour, IPunObservable
{
    private Vector2d latLonCurrent;
    private Vector2 latlonSend;
    private Vector2 latlonReceived;
    private PhotonView photonView;

    private AbstractMap map;
    private Zone zoneData;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        latlonSend = new Vector2();
        latlonReceived = new Vector2();
        map = LocationProviderFactory.Instance.mapManager;
    }

    void Update()
    {
        if (photonView.IsMine)//SI soy el gm, calculo la posición de la zona
        {
            latLonCurrent = CalculateZoneLatLon(map.CenterLatitudeLongitude) + map.CenterLatitudeLongitude;
            latlonSend.x = (float)latLonCurrent.x;
            latlonSend.y = (float)latLonCurrent.y;
            Debug.Log("Centro mapa: " + map.CenterLatitudeLongitude + "/Zona: " + latLonCurrent);
        }
        else
        {
            Vector2d v = new Vector2d(latlonReceived.x, latlonReceived.y);
            transform.localPosition = map.GeoToWorldPosition(v);
        }

    }
    public Vector2d CalculateZoneLatLon(Vector2d center)
    {
        Vector2d latlon = transform.GetGeoPosition(center, 1);
        return latlon;
    }

    public Vector3 GetGeoPosition()
    {
        Vector2d v = new Vector2d(latlonReceived.x, latlonReceived.y);
        return  map.GeoToWorldPosition(v);
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
        }else if (stream.IsReading)
        {
            if (photonView != null && !photonView.IsMine)//SI me llega info de la zona, la updateo
            {
                latlonReceived = (Vector2)stream.ReceiveNext();
            }
        }
    }

}
