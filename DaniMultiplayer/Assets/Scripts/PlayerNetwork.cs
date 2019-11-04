using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun
{

    public class PlayerNetwork : MonoBehaviour //,Photon.Pun.IPunObservable
    {

        [SerializeField] private MonoBehaviour[] playerControlScripts;

        private PhotonView photonView;

        private Vector3 actualPos;
        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            actualPos = transform.position;
            Initialize();
        }
        private void Initialize()
        {
            if (photonView.IsMine) //Si somos nosotros
            {
                InitializedOnline();
            }
            else //Si es otr persona
            {
                foreach (MonoBehaviour m in playerControlScripts) //Desactivamos sus scripts(para no controlar a todos los que entra en la room)
                {
                    m.enabled = false;
                }
            }            
        }

        void InitializedOnline()
        {
            Hashtable table = new Hashtable();
            table.Add("id", this.GetComponent<Player>().id);
            table.Add("isGameMaster", this.GetComponent<Player>().isGameMaster);
            PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        }
        

        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
        {
            if (stream.IsWriting) //Si mandamos info
            {
                if (photonView.IsMine) //Si soy yo, mando mi posición
                {
                    stream.SendNext(actualPos);
                }

            }else if (stream.IsReading) //SI recibimos info
            {
                if (!photonView.IsMine) //Leo la posición de los demás
                {
                    transform.position = (Vector3)stream.ReceiveNext();
                }
            }
        }  

    }

}
