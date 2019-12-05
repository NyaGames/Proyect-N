using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomUsername : MonoBehaviour
{
	[SerializeField] private TMP_InputField inputField;

	private string[] usernamePool = new string[] { "3D Waffle", "57 Pixels", "101", "Accidental Genius", "Alpha", "Airport Hobo", "Bearded Angler", "Beetle King", "Bitmap",
												   "K-9","Keystone","Kickstart","Kill Switch","Kingfisher","Kitchen","Knuckles", "Lady Killer","Liquid Science","Little Cobra",
												   "Little General","Lord Nikon","Lord Pistachio","Mad Irishman","Snow Hound","Sofa King","Speedwell", "Spider Fuji","Springheel Jack","Squatch",
												   "Stacker of Wheat","Sugar Man","Suicide Jockey","Swampmasher","Swerve","Tacklebox","The China Wall", "Toolmaker","Troubadour","Vagabond Warrior",
												   "Washer","Twitch","Glyph","Guillotine","Gunhawk","Highlander Monk","High Kingdom Warrior", "Esquire","Flakes","Flint" };

	public void GetRandomUsername()
	{
		int index = Random.Range(0, usernamePool.Length);

		string username = usernamePool[index];

		inputField.text = username;
	}
}
