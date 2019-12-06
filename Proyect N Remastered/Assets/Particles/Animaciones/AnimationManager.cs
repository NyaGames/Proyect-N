using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public GameObject boton;
    public GameObject animator;
    private Button b;
    private void Update()
    {
        b = boton.GetComponent<Button>();
        if (b.interactable == true)
        {
            StartAnimation();
        }
    }
    public void StartAnimation()
    {
        if (b.interactable == true)
        {
            Button b = boton.GetComponent<Button>();
            Animator a = animator.GetComponent<Animator>();
            a.SetTrigger("JoinRoom");
        }
    }
}
