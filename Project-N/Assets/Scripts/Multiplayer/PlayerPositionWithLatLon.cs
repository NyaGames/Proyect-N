using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Photon.Pun;

public class PlayerPositionWithLatLon : MonoBehaviour, IPunObservable 
{
    public Vector2d latLon;
    public Vector2 latlonSend;
    public Vector2 latlonReceived;
    public PhotonView photonView;

    public void Start()
    {
        photonView = GetComponent<PhotonView>();
        latlonSend = new Vector2();
        latlonReceived = new Vector2();
    }

    // Update is called once per frame
    void UpdatePosition(Vector2d latLon)
    {
        latlonSend.x = (float)latLon.x;
        latlonSend.y = (float)latLon.y;
        if (photonView.IsMine) //Si soy yo, me updateo con la posicion del gps
        {
            var map = LocationProviderFactory.Instance.mapManager;
            transform.localPosition = map.GeoToWorldPosition(latLon);
        }
        else
        {
            var map = LocationProviderFactory.Instance.mapManager;
            Vector2d v = new Vector2d(latlonReceived.x, latlonReceived.y);
            transform.localPosition = map.GeoToWorldPosition(v);
        }
       
    }

    private void Update()
    {
        if (photonView.IsMine) //Si soy yo, saco mi posicion del gps
        {
            latLon = LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude;
        }

    }

    private void LateUpdate()
    {
        if (latLon != null)
        {
            UpdatePosition(latLon);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting){
            if (photonView.IsMine) //Si soy yo,mando mi info
            {
                stream.SendNext(latlonSend);
            }
        }
        else if (stream.IsReading)
        {
            if (photonView != null && !photonView.IsMine) //Si no soy yo, updateo a quien me llegue
            {
                latlonReceived = (Vector2)stream.ReceiveNext();
            }
        }
       

    }
}
