using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour,IPointerDownHandler
{
    public GameObject dropPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        Touch touch = Input.GetTouch(0);
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
        AutoLobby.Instance.dropPos = Instantiate(dropPrefab, touch.position, Quaternion.identity);
        AutoLobby.Instance.creatingDrop = true;
        //throw new System.NotImplementedException();
    }

}
