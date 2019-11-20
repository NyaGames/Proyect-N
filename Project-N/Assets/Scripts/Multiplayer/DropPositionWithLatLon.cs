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
    private double[] latlonSend;
    private double[] latlonReceived;
    private PhotonView photonView;

    private AbstractMap map;
    private Drop dropData; 

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        latlonSend = new double[2];
        latlonReceived = new double[2];
        map = LocationProviderFactory.Instance.mapManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)//SI soy el gm, calculo la posición del drop
        {
            latLonCurrent = CalculateDropLatLon(map.CenterLatitudeLongitude) + map.CenterLatitudeLongitude;
            latlonSend[0] = latLonCurrent.x;
            latlonSend[1] = latLonCurrent.y;
            Debug.Log("Centro mapa: " + map.CenterLatitudeLongitude + "/Drop: " + latLonCurrent);
        }
        else
        {
            Vector2d v = new Vector2d(latlonSend[0], latlonSend[1]);
            transform.localPosition = map.GeoToWorldPosition(v);
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
                object o = stream.ReceiveNext();
                latlonReceived = (double[])o;
            }
        }
    }

}
