using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public float speed = 0.1f;


	private float s;
	private void Start()
	{
		speed /= 1000;
		s = speed;
	}
	
	void Update ()
	{
		if (Time.timeScale < 1) speed = 0;
		else speed = s;
		transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
	}
}
