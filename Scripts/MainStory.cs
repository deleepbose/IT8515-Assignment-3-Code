using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
	public void OnEnable()
	{
		SceneManager.LoadSceneAsync(1);
	}
}
