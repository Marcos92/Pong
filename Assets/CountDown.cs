using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

	public Text CounterText;

	public float segundos,minutos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		minutos = (int)(Time.time / 60f);
		segundos = (int)(Time.time % 60f);
		CounterText.text = minutos.ToString("00") + ":" + segundos.ToString("00");

	}
}
