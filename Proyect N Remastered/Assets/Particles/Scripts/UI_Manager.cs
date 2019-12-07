using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(Button))]
public class UI_Manager : MonoBehaviour
{
    public Button button;
    public GameObject Halo;
    public GameObject Points;
    public GameObject Clickar;
    public Animator anim;
    public bool GM;

    public GameObject synchronizeScenesPrefab;

    public void SpawnParticles()
    {
        if (GM == true)
        {
            Halo.GetComponent<ParticleSystem>().Play();
            Points.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Halo.GetComponent<ParticleSystem>().Stop();
            Points.GetComponent<ParticleSystem>().Stop();
        }

    }

    public void ParticlesOnClick()
    {
        if(Clickar != null)
        {
            var vfx = Instantiate(Clickar, button.transform.position, Quaternion.identity) as GameObject;
            vfx.transform.SetParent(button.transform);
            var ps = vfx.GetComponent<ParticleSystem>();
            Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
        }

    }

    public void ChangeGM()
    {
        if(GM == true)
        {
            GM = false;
        }else if(GM == false)
        {
            GM = true;
        }
    }

    public void StartGame()
    {
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("GameStarted", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(table);

        anim = gameObject.GetComponent<Animator>();
        Halo.Destroy();
        Points.Destroy();
        StartCoroutine(LoadScene());
       
    }

    IEnumerator LoadScene()
    {
        anim.SetTrigger("StartGame");
        yield return new WaitForSeconds(2.0f);
        // SceneManager.LoadScene("FinalGameScene");
        PhotonNetwork.LoadLevel("FinalGameScene");
    }

}
