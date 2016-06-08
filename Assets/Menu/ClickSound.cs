using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(Button))]

public class ClickSound : MonoBehaviour {

	public AudioClip sound,oversound;

	//private Button button { get { return GetComponent<Button> (); } }
	private AudioSource source { get { return GetComponent<AudioSource> (); } }


	// Use this for initialization
	void Start () {

		gameObject.AddComponent<AudioSource> ();
		source.clip = sound;
		source.playOnAwake = false;
	
		//button.onClick.AddListener (() => PlaySound ());
	}
	
	public void PlaySound()
	{
		source.PlayOneShot (sound);
	}

	public void PlayOverSound(BaseEventData data)
	{
		source.PlayOneShot (oversound);
	}
}
