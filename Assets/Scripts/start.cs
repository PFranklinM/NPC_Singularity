using UnityEngine;
using System.Collections;

public class start : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("PS_Circle") || Input.GetButtonDown ("PS_Circle2")) {
			Application.LoadLevel("Main");
		}
	
	}
}
