using UnityEngine;
using System.Collections;

public class p2Arrow : MonoBehaviour {

	public GameObject arrow;
	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		var newRotation = Quaternion.LookRotation(transform.position - player.transform.position, Vector3.forward);
		newRotation.x = 0.0f;
		newRotation.y = 0.0f;
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
	
	}
}
