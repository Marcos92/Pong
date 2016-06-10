using UnityEngine;
using System.Collections;

public class ChangeScene1 : MonoBehaviour {


	// Update is called once per frame
	public void ChangeToScene (string MudarCena) {
		Application.LoadLevel (MudarCena);
	}

	public void Quit()
	{
		Application.Quit ();
	}
	public void Graphics(int x)
	{

		QualitySettings.SetQualityLevel (x);
	}
}
