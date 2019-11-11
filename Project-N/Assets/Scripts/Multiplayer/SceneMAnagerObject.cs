using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMAnagerObject : MonoBehaviour
{
	DeviceOrientation orientation;


	/**PLACEHOLDER TOTAL Y ABSOLUTO 100% HARDCODEADO. COMO ESTO SE QUEDE EN EL CODIGO TE MATO JUSI**/
	private void Awake()
	{
		orientation = GetComponent<DeviceOrientation>();
	}

	public void ChangeScene()
	{
		if (orientation.portrait)
		{
			SceneManager.LoadScene(2);
		}
		else
		{
			SceneManager.LoadScene(1);
		}
	}
}
