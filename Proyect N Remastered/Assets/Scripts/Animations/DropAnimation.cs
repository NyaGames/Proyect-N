using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.Events;
using Photon.Pun;

public class DropAnimation : MonoBehaviour
{
	public bool activated = false;

	public Transform model;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
		model = transform.GetChild(0).GetChild(0);		
	}   
  
	
	[Button]
	public void ActivateDrop()
	{
		if (activated) return;

		activated = true;
		Sequence seq = DOTween.Sequence();
		seq.Append(model.DOLocalRotate(new Vector3(45f, 45f, 45f), 1f));
		seq.Join(model.DOLocalMoveY(5f, 1f));
		seq.AppendCallback(() => model.gameObject.GetComponent<FloatingRotator>().enabled = true);
	}

	[Button]
	public void DeactivateDrop()
	{
		if (!activated) return;

		activated = false;

		Sequence seq = DOTween.Sequence();
		seq.AppendCallback(() => model.gameObject.GetComponent<FloatingRotator>().enabled = false);
		seq.Append(model.DOLocalRotate(new Vector3(0, 0, 0), 1f));
		seq.Join(model.DOLocalMoveY(0f, 1f));

	}

	[Button]
    [PunRPC]
	public void TryDestroyAnimation()
	{
		if (!activated) return;

		activated = false;

		Sequence seq = DOTween.Sequence();
		model.GetComponent<FloatingRotator>().AccelerateRotation(50);
		seq.Append(model.DOScale(new Vector3(5, 5, 5), 0.1f));
		seq.Append(model.DOScale(new Vector3(0, 0, 0), 1f));
        if (photonView.IsMine)
        {
            seq.AppendCallback(() => PhotonNetwork.Destroy(gameObject));
        }

    }
}
