using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.Events;

public class DropAnimation : MonoBehaviour
{
	private bool activated = false;

	private Transform model;


    // Start is called before the first frame update
    void Start()
    {
		model = transform.GetChild(0);		
	}   
  
	
	[Button]
	private void ActivateDrop()
	{
		if (activated) return;

		activated = true;
		Sequence seq = DOTween.Sequence();
		seq.Append(model.DOLocalRotate(new Vector3(45f, 45f, 45f), 1f));
		seq.Join(model.DOMove(new Vector3(0, 5f, 0f), 1f));
		seq.AppendCallback(() => model.gameObject.GetComponent<FloatingRotator>().enabled = true);
	}

	[Button]
	void DeactivateDrop()
	{
		if (!activated) return;

		activated = false;

		Sequence seq = DOTween.Sequence();
		seq.AppendCallback(() => model.gameObject.GetComponent<FloatingRotator>().enabled = false);
		seq.Append(model.DOLocalRotate(new Vector3(0, 0, 0), 1f));
		seq.Join(model.DOMove(new Vector3(0, 0f, 0f), 1f));

	}
}
