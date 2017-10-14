using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public float speed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	//	var hor = Input.GetAxis("Horizontal");
	//	var ver = Input.GetAxis("Vertical");
	//	transform.position += Vector3.right * hor * speed + Vector3.up * ver * speed;
		UpdatePosition();
	}

	[SerializeField]
	UnityEngine.UI.Slider slider;

	void UpdatePosition()
	{
		transform.position = Vector3.right * slider.value * speed;
	}
}
