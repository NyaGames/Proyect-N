using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaLeanTween : MonoBehaviour
{
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.play(gameObject.GetComponent<RectTransform>(), sprites).setSpeed(30.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
