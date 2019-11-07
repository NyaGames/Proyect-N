using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour,IPointerDownHandler
{
    public GameObject dropPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(AutoLobby.Instance.numDrops > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 9));
            pos.y = 0;
            AutoLobby.Instance.dropPos = Instantiate(dropPrefab, pos, Quaternion.identity);
            AutoLobby.Instance.dropPos.transform.Rotate(new Vector3(90,0,0),Space.Self); 
            AutoLobby.Instance.creatingDrop = true;
        }
      
    }

}
