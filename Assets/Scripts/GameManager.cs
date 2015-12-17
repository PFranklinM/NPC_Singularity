using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public AudioSource[] AudioClips = null;

	// Use this for initialization
	void Start () {

		Time.timeScale = 1F;
		Application.targetFrameRate = 60;

		int random = Random.Range (1, 101);
		
		if (random == 5) {
			AudioClips [1].Play ();
		} else {
			AudioClips [0].Play ();
		}
	
	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKey (KeyCode.Return)) {
//			Application.LoadLevel("Main");
//		}
	
	}
}
