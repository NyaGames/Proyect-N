using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Manager : MonoBehaviour
{
    public Button button;
    public GameObject Halo;
    public GameObject Points;
    public Animator anim;
    public bool GM;


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
     
     anim = gameObject.GetComponent<Animator>();
     anim.SetTrigger("Active");
    }
}
