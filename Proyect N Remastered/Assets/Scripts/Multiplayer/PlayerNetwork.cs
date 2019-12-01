using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun
{

    public class PlayerNetwork : MonoBehaviour //,Photon.Pun.IPunObservable
    {

        [SerializeField] private MonoBehaviour[] playerControlScripts;

        [HideInInspector] public PhotonView photonViewTransform;
        private Vector3 actualPos;

        public bool isGameMaster;
        public int id { get; set; }

        public void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["isGameMaster"] && !photonViewTransform.IsMine) //Si el cliente NO es game master, desactiva a los demás
            {
                // if (isGameMaster)
                //{
                //   gameObject.SetActive(false);
                // }
                // else
                // {
                Destroy(gameObject);
                // }

            }
            photonViewTransform = GetComponent<PhotonView>();
            

            this.GetComponentInChildren<TextMesh>().text = GetComponent<Player>().id.ToString() + "/" + GetComponent<Player>().isGameMaster.ToString();
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

        void InitializedOnline()
        {
            Hashtable table = new Hashtable();
            table.Add("id", this.GetComponent<Player>().id);
            table.Add("isGameMaster", this.GetComponent<Player>().isGameMaster);
            PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        }
        

    

    }

}
