using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateDropButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject imageDrop;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GamemasterManager.Instance.numDrops > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 9));
            pos.y = 0;
            GamemasterManager.Instance.dropPos = Instantiate(imageDrop, pos, Quaternion.identity);
            GamemasterManager.Instance.dropPos.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
            GamemasterManager.Instance.creatingDrop = true;
        }

    }

}
