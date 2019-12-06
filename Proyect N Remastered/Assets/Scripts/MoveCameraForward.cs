using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCameraForward : MonoBehaviour
{
	public float speed = 1f;

	private void Start()
	{
		DontDestroyOnLoad(transform.parent.gameObject);
	}

	private void FixedUpdate()
	{
		transform.position += Vector3.forward * speed * Time.deltaTime;
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "GameScene" || scene.name == "FinalGameScene")
		{
			// Destroy the gameobject this script is attached to
			Destroy(transform.parent.gameObject);
		}
	} 

}
