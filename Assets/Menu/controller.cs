using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class controller : MonoBehaviour {

	public void Back()
	{
	
		SceneManager.LoadScene("MenuInicial");
	}
}
