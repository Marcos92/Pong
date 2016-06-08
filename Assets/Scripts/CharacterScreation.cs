using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterScreation : MonoBehaviour {

	private List<GameObject> models;
	//Default index of the model
	private int selectionIndex= 0;
	// Use this for initialization
	private void Start () {
	
		models = new List<GameObject> ();
		foreach (Transform t in transform) {
			models.Add (t.gameObject);
			t.gameObject.SetActive (false);
		}

		models [selectionIndex].SetActive (true);
	}
	private void Update()
	{
		if(Input.GetMouseButton(0))
            models[selectionIndex].transform.RotateAround(Vector3.up,Input.GetAxis("Mouse X"));
	}

	public void Select(int index)
	{
		if (index == selectionIndex)
			return;
		if (index < 0 || index >= models.Count)
			return;

		models [selectionIndex].SetActive (false);
		selectionIndex = index;
        models[selectionIndex].gameObject.transform.Rotate(new Vector3(0,1,0),0f);
		models [selectionIndex].SetActive (true);
       

	}
}
