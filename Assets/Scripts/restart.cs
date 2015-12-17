using UnityEngine;
using System.Collections;

public class restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("PS_X") || Input.GetButtonDown ("PS_X2")) {
			Application.LoadLevel("Title");
		}
	
	}
}
