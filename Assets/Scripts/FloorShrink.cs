using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloorShrink : MonoBehaviour {

	float gameTime = 120.0f;

	public GameObject gameTimer;

	public GameObject floor;

	public GameObject gameOver;
	public GameObject gameOverBackGround;

	public GameObject player1;
	public GameObject player2;

	// Use this for initialization
	void Start () {

		gameOver.SetActive(false);
		gameOverBackGround.SetActive(false);

		StartCoroutine(floorDissappear());
	
	}
	
	// Update is called once per frame
	void Update () {

		playerOneMove player1Score = player1.GetComponent<playerOneMove>();

		playerTwoMove player2Score = player2.GetComponent<playerTwoMove>();
		
		if (gameTime <= 0 && player1Score.winRound > player2Score.winRound) {
			StartCoroutine(playerOneWin());
		}

		if (gameTime <= 0 && player1Score.winRound < player2Score.winRound) {
			StartCoroutine(playerTwoWin());
		}

		if (gameTime <= 0 && player1Score.winRound == player2Score.winRound) {

			if(player1Score.winRound > player2Score.winRound){
				StartCoroutine(playerOneWin());
			}

			if(player1Score.winRound < player2Score.winRound){
				StartCoroutine(playerTwoWin());
			}
		}
	}

	IEnumerator floorDissappear(){
		
		while (gameTime >= 0) {

			floor.transform.localScale -= new Vector3 (0.02222222F, 0, 0);

			gameTime -= Time.deltaTime;
			
			Text text = gameTimer.GetComponent<Text>();
			text.text = "" + (int)gameTime;

			yield return null;
		}
	}

	IEnumerator playerOneWin(){
		float winDelay = 0.0f;
		
		while (winDelay < 1) {

			winDelay += Time.deltaTime;

			gameOver.SetActive(true);
			gameOverBackGround.SetActive(true);

			Time.timeScale = 0.5F;
			
			yield return null;
		}

		Application.LoadLevel ("player1Win");
	}

	IEnumerator playerTwoWin(){
		float winDelay = 0.0f;
		
		while (winDelay < 1) {
			
			winDelay += Time.deltaTime;
			
			gameOver.SetActive(true);
			gameOverBackGround.SetActive(true);
			
			Time.timeScale = 0.5F;
			
			yield return null;
		}

		Time.timeScale = 1F;
		Application.LoadLevel ("player2Win");
	}
}
