using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitalSceneController : MonoBehaviour
{
   public void GoToMainMenu()
	{
		SceneManager.LoadScene("FinalMainMenu");
	}
}
