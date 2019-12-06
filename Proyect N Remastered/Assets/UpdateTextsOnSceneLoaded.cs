using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LanguageControl))]
public class UpdateTextsOnSceneLoaded : MonoBehaviour
{
	void OnEnable()
	{		
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		GetComponent<LanguageControl>().UpdateTextTranslation();
	}

	void OnDisable()
	{		
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

}
