using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun
{

    public class PlayerNetwork : MonoBehaviour ,Photon.Pun.IPunObservable
    {

        [SerializeField] private MonoBehaviour[] playerControlScripts;

        public PhotonView photonViewTransform;
        public PhotonView photonViewInfo;
        
        private Vector3 actualPos;
        private void Start()
        {
            photonViewTransform = GetComponent<PhotonView>();
            Initialize();
        }
        private void Initialize()
        {
            

            if (photonViewTransform.IsMine) //Si somos nosotros
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

        private void Update()
        {
            if (!actualPos.Equals(transform.position)) 
            {
                actualPos = transform.position;
            }
            
        }

        void InitializedOnline()
        {
            Hashtable table = new Hashtable();
            table.Add("id", this.GetComponent<Player>().id);
            table.Add("isGameMaster", this.GetComponent<Player>().isGameMaster);
            PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        }
        

        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info) //Stream = Mensaje que escribes/recibes |||| Info = info del que envia o manda el mensaje
        {
            if (stream.IsWriting)
            {   
                if (photonViewTransform.IsMine) //Solo mando información si soy yo
                {
                    stream.SendNext(actualPos.x);
                    stream.SendNext(actualPos.y);
                    stream.SendNext(actualPos.z);
                }
            }
            else
            {
                if (!photonViewTransform.IsMine){ //Solo recibo información de otros jugadores
                    actualPos.x = (float)stream.ReceiveNext();
                    actualPos.y = (float)stream.ReceiveNext();
                    actualPos.z = (float)stream.ReceiveNext();

                    transform.position = new Vector3(actualPos.x, actualPos.y, actualPos.z);
                }  
            }
        }  

    }

}
