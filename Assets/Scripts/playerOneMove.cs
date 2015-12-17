using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class playerOneMove : MonoBehaviour {
	
	public float moveAmount = 10f;
	
	public float jumpSpeed = 10f;
	
	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject floor;

	public GameObject playerOneScore;
	public GameObject arrow1;
	public GameObject arrow2;
	public GameObject halo;

	private int xDeathMin = -350;
	private int xDeathMax = 350;
	private int yDeath = -200;

	private int xBorderMin = -120;
	private int xBorderMax = 120;
	private int yBorderMin = -70;
	private int yBorderMax = 70;

	private bool offscreen;

	public int winRound = 0;

	private float colorFlash = 0;
	
	public GameObject playerOneShield;
	public GameObject playerTwoShield;

	public GameObject p1HitR;
	public GameObject p1HitL;
		
	public bool playerOneShieldOn = false;
	public bool playerTwoShieldOn = false;
		
	public float Distance = 30.0f;
	
	private int jumpCounter = 0;
	
	Rigidbody2D rb;

	public BoxCollider2D boxCol;

	private SpriteRenderer spriteRenderer;

	public Sprite playerOneStandingLeft;
	public Sprite playerOneStandingRight;
	public Sprite playerOneJumpingLeft;
	public Sprite playerOneJumpingRight;
	public Sprite playerOneRunningLeft1;
	public Sprite playerOneRunningLeft2;
	public Sprite playerOneRunningRight1;
	public Sprite playerOneRunningRight2;
	public Sprite playerOneAttackingLeft;
	public Sprite playerOneAttackingRight;

	private bool facingLeft;
	private bool facingRight;
	private bool attackingLeft;
	private bool attackingRight;
	private bool isMoving;

	private bool airborn;
	private float runAnimation;
	public bool isHit;
	public bool groundPound1;
	private bool gpReady;
	private bool justDied;

	private float attackGroundTime = 0.1f;
	private float attackAirTime = 0.3f;

	private float shieldX = 25.0f;
	private float shieldY = 30.0f;

	public SpriteRenderer renderer;

	public AudioSource[] AudioClips = null;

	// Use this for initialization
	void Start () {

		renderer = GetComponent<SpriteRenderer>();
		
		rb = GetComponent<Rigidbody2D>();
		
		playerOneShield.SetActive(false);

		p1HitR.SetActive(false);
		p1HitL.SetActive(false);

		facingLeft = false;
		facingRight = true;
		airborn = false;
		offscreen = false;
		isHit = false;
		groundPound1 = false;
		gpReady = false;
		justDied = false;
		attackingLeft = false;
		attackingRight = false;
		isMoving = false;

		arrow1.SetActive (false);
		arrow2.SetActive (false);
		halo.SetActive (false);

		runAnimation = 0.0f;

		spriteRenderer = GetComponent<SpriteRenderer> ();
		if (spriteRenderer.sprite == null) {
			spriteRenderer.sprite = playerOneStandingRight;
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Text text = playerOneScore.GetComponent<Text>();
		text.text = "Player One: " + winRound;
		
		Vector3 moving = new Vector3 (transform.position.x,
		                              transform.position.y,
		                              transform.position.z);

		p1HitR.transform.position = playerOne.transform.position;

		Vector3 Right = p1HitR.transform.position;
		Right.y += 8.0f;
		Right.x += 8.5f;
		p1HitR.transform.position = Right;


		p1HitL.transform.position = playerOne.transform.position;
		
		Vector3 Left = p1HitL.transform.position;
		Left.y += 8.0f;
		Left.x -= 8.5f;
		p1HitL.transform.position = Left;

		if (Input.GetAxis ("DPad_Horizontal") > 0 || Input.GetAxis ("Horizontal") > 0 ||
			Input.GetAxis ("DPad_Horizontal") < 0 || Input.GetAxis ("Horizontal") < 0 ||
			Input.GetAxis ("DPad_Vertical") < 0 || Input.GetAxis ("Vertical") < 0) {

			isMoving = true;
		} else {
			isMoving = false;
		}  
		
		if (Input.GetAxis ("DPad_Horizontal") > 0 || Input.GetAxis ("Horizontal") > 0) {
			moving.x += moveAmount * Time.deltaTime;
			facingRight = true;
			facingLeft = false;

			runningRight();
			   

		} else if (Input.GetAxis ("DPad_Horizontal") == 0 && facingRight == true && airborn == false ||
		           Input.GetAxis ("Horizontal") == 0 && facingRight == true && airborn == false) {
			spriteRenderer.sprite = playerOneStandingRight;

			runAnimation = 0;
		}

		if (Input.GetAxis ("DPad_Horizontal") > 0 && airborn == true ||
		    Input.GetAxis ("Horizontal") > 0 && airborn == true) {
			spriteRenderer.sprite = playerOneJumpingRight;
		}
		
		if (Input.GetAxis ("DPad_Horizontal") < 0 || Input.GetAxis ("Horizontal") < 0) {
			moving.x -= moveAmount * Time.deltaTime;
			facingRight = false;
			facingLeft = true;

			runningLeft();
			
		} else if (Input.GetAxis ("DPad_Horizontal") == 0 && facingLeft == true && airborn == false ||
		           Input.GetAxis ("Horizontal") == 0 && facingLeft == true && airborn == false) {
			spriteRenderer.sprite = playerOneStandingLeft;

			runAnimation = 0;
		}

		if (Input.GetAxis ("DPad_Horizontal") < 0 && airborn == true ||
		    Input.GetAxis ("Horizontal") < 0 && airborn == true) {
			spriteRenderer.sprite = playerOneJumpingLeft;
		}

		if (Input.GetAxis ("DPad_Vertical") < 0 && isHit == false && gpReady == true && airborn == true || 
		    Input.GetAxis ("Vertical") < 0 && isHit == false && gpReady == true && airborn == true) {

			rb.velocity = new Vector3(0, -400, 0);

			groundPound1 = true;
		}

		
		if(Input.GetButtonDown ("PS_X") && facingRight == true){
			jumpCounter ++;
			spriteRenderer.sprite = playerOneJumpingRight;
			airborn = true;
		}

		if(Input.GetButtonDown ("PS_X") && facingLeft == true){
			jumpCounter ++;
			spriteRenderer.sprite = playerOneJumpingLeft;
			airborn = true;
		}
		
		if (Input.GetButtonDown ("PS_X") && jumpCounter < 4){
			rb.velocity = new Vector3(0, 300, 0);

			int random = Random.Range (1, 4);
			
			if(random == 1){
				AudioClips[0].Play();
			}
			
			if(random == 2){
				AudioClips[1].Play();
			}
			
			if(random == 3){
				AudioClips[2].Play();
			}
		}

		if (Input.GetButtonDown ("PS_X")) {
			groundPound1 = false;
		}

		if (groundPound1 == true) {
			renderer.color = new Color (0.0f, 255.0f, 0.0f, 1f);
		} else {
			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
		}

		//STATIONARY ATTACKS
		if (Input.GetButtonDown ("PS_Square") && facingRight == true && airborn == false && isHit == false && isMoving == false) {
			StartCoroutine(AttackRightGroundCo());
		}

		if (Input.GetButtonDown ("PS_Square") && facingLeft == true && airborn == false && isHit == false && isMoving == false) {
			StartCoroutine(AttackLeftGroundCo());
		}

		if (Input.GetButtonDown ("PS_Square") && facingRight == true && airborn == true && isHit == false && isMoving == false) {
			StartCoroutine(AttackRightAirCo());
		}

		if (Input.GetButtonDown ("PS_Square") && facingLeft == true && airborn == true && isHit == false && isMoving == false) {
			StartCoroutine(AttackLeftAirCo());
		}

		//MOVING ATTACKS
		if (Input.GetButtonDown ("PS_Square") && facingRight == true && airborn == false && isHit == false && isMoving == true) {
			StartCoroutine(AttackRightMovingGroundCo());
		}

		if (Input.GetButtonDown ("PS_Square") && facingLeft == true && airborn == false && isHit == false && isMoving == true) {
			StartCoroutine(AttackLeftMovingGroundCo());
		}
		
		if (Input.GetButtonDown ("PS_Square") && facingRight == true && airborn == true && isHit == false && isMoving == true) {
			StartCoroutine(AttackRightMovingAirCo());
		}

		if (Input.GetButtonDown ("PS_Square") && facingLeft == true && airborn == true && isHit == false && isMoving == true) {
			StartCoroutine(AttackLeftMovingAirCo());
		}

		if(Input.GetButtonDown ("PS_Square")){
			
			int random = Random.Range (1, 4);
			
			if(random == 1){
				AudioClips[3].Play();
			}
			
			if(random == 2){
				AudioClips[4].Play();
			}
			
			if(random == 3){
				AudioClips[5].Play();
			}
		}
		
		if (Input.GetButton ("PS_Circle") && isHit == false) {
			
			playerOneShield.SetActive (true);
			
			playerOneShieldOn = true;
			
			playerOneShield.transform.position = playerOne.transform.position;

			playerOneShield.transform.localScale -= new Vector3(0.5F, 0.5f, 0);
			
		} else if (Input.GetButtonUp ("PS_Circle")) {

			playerOneShield.SetActive (false);
			
			playerOneShieldOn = false;

			playerOneShield.transform.localScale = 
				new Vector3(transform.localScale.x + 45, transform.localScale.y + 50, transform.localScale.z);
		}

		if (playerOneShield.transform.localScale.magnitude <= shieldX 
		    && playerOneShield.transform.localScale.magnitude <= shieldY) {

			AudioClips[8].Play();
			StartCoroutine(shieldStun());
		}

		if (Vector3.Distance (playerOne.transform.position, playerTwoShield.transform.position) < Distance &&
		    GameObject.Find("Player2").GetComponent<playerTwoMove>().playerTwoShieldOn &&
		    attackingRight == true) {

			AudioClips[8].Play();
			StartCoroutine(shieldKnockBackLeft());
		}

		if (Vector3.Distance (playerOne.transform.position, playerTwoShield.transform.position) < Distance &&
		    GameObject.Find("Player2").GetComponent<playerTwoMove>().playerTwoShieldOn &&
		    attackingLeft == true) {

			AudioClips[8].Play();
			StartCoroutine(shieldKnockBackRight());
		}
		
		transform.position = moving;

		//Player Death

		if (transform.position.x < xDeathMin || transform.position.x > xDeathMax || transform.position.y < yDeath) {

			AudioClips[9].Play();

			playerTwo.GetComponent<playerTwoMove>().Score();
			
			Vector3 respawn = new Vector3 (-50, 50, 0);
			
			transform.position = respawn;

			spriteRenderer.sprite = playerOneStandingRight;
			
			justDied = true;
			
			isHit = false;

			StartCoroutine(backToLife());
		}
		
		if (transform.position.x < xBorderMin || transform.position.x > xBorderMax || 
		    transform.position.y < yBorderMin || transform.position.y > yBorderMax) {

			offscreen = true;
		} else {
			offscreen = false;
		}

		if (offscreen == true) {

			arrow1.SetActive (true);
			arrow2.SetActive (true);

		} else if (offscreen == false) {
			arrow1.SetActive (false);
			arrow2.SetActive (false);
		}

		if(justDied == true){
			facingLeft = false;
			facingRight = true;
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "ground") {
			airborn = false;
			gpReady = false;
			groundPound1 = false;
		}

		if (coll.gameObject.tag == "player2") {
			jumpCounter = 0;
			airborn = false;
//			gpReady = false;
//			groundPound1 = false;
		}

		if (coll.gameObject.tag == "ground" && facingRight == true) {
			jumpCounter = 0;
			spriteRenderer.sprite = playerOneStandingRight;

			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
			colorFlash = 0;
			moveAmount = 140;

			justDied = false;
		}

		if (coll.gameObject.tag == "ground" && facingLeft == true && justDied == false) {
			jumpCounter = 0;
			spriteRenderer.sprite = playerOneStandingLeft;

			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
			colorFlash = 0;
			moveAmount = 140;
		}

		if (coll.gameObject.tag == "ground" && facingLeft == true && justDied == true) {
			jumpCounter = 0;
			spriteRenderer.sprite = playerOneStandingRight;
			
			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
			colorFlash = 0;
			moveAmount = 140;

			justDied = false;
		}

		if (coll.gameObject.tag == "player2" && 
		    GameObject.Find("Player2").GetComponent<playerTwoMove>().groundPound2 &&
		    playerTwoShieldOn == false) {

			AudioClips[8].Play();
			StartCoroutine(player1Fall());
		}

		if (coll.gameObject.tag == "player2" &&
		    GameObject.Find ("Player2").GetComponent<playerTwoMove> ().isHit) {
			
			groundPound1 = false;
		}

		if (coll.gameObject.tag == "player2AttackRight" && playerOneShieldOn == false) {
			StartCoroutine(knockBackRight());

			int random = Random.Range (1, 3);
			
			if(random == 1){
				AudioClips[6].Play();
			}
			
			if(random == 2){
				AudioClips[7].Play();
			}
		}
		
		if (coll.gameObject.tag == "player2AttackLeft" && playerOneShieldOn == false) {
			StartCoroutine(knockBackLeft());

			int random = Random.Range (1, 3);
			
			if(random == 1){
				AudioClips[6].Play();
			}
			
			if(random == 2){
				AudioClips[7].Play();
			}
		}
	}

	void OnCollisionExit2D (Collision2D coll) {
		gpReady = true;
	}

	void runningRight(){
		runAnimation += Time.deltaTime*14;
		
		
		if((int)runAnimation%2==1){
			spriteRenderer.sprite = playerOneRunningRight1;
		}
		
		if((int)runAnimation%2==0){
			spriteRenderer.sprite = playerOneRunningRight2;
		}
	}

	void runningLeft(){
		runAnimation += Time.deltaTime*14;
		
		
		if((int)runAnimation%2==1){
			spriteRenderer.sprite = playerOneRunningLeft1;
		}
		
		if((int)runAnimation%2==0){
			spriteRenderer.sprite = playerOneRunningLeft2;
		}
	}

	//STATIONARY ATTACKS
	IEnumerator AttackRightGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackGroundTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerOneAttackingRight;
			attackingRight = true;

			p1HitR.SetActive(true);

			yield return null;
		}

		p1HitR.SetActive(false);
		attackingRight = false;
	}

	IEnumerator AttackLeftGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackGroundTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerOneAttackingLeft;
			attackingLeft = true;
		
			p1HitL.SetActive(true);

			yield return null;
		}

		p1HitL.SetActive(false);
		attackingLeft = false;
	}
	
	IEnumerator AttackRightAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackAirTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerOneAttackingRight;
			attackingRight = true;

			p1HitR.SetActive(true);

			yield return null;
		}
		p1HitR.SetActive(false);
		attackingRight = false;
	}

	IEnumerator AttackLeftAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackAirTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerOneAttackingLeft;
			attackingLeft = true;

			p1HitL.SetActive(true);

			yield return null;
		}
		p1HitL.SetActive(false);
		attackingLeft = false;
	}


	//MOVING ATTACKS
	IEnumerator AttackRightMovingGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.25f) {
			attackCounter += Time.deltaTime;

			if(attackCounter <= 0.1){
				rb.velocity = new Vector3 (400, 0, 0);
			}

			if(attackCounter > 0.1){
				moveAmount = 0;

				spriteRenderer.sprite = playerOneAttackingRight;

				attackingRight = true;
						
				p1HitR.SetActive(true);
			}

			
			yield return null;
		}

		moveAmount = 140;
		isMoving = false;
		p1HitR.SetActive(false);
		attackingRight = false;
	}
	
	IEnumerator AttackLeftMovingGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.25f) {
			attackCounter += Time.deltaTime;
			
			if(attackCounter <= 0.1){
				rb.velocity = new Vector3 (-400, 0, 0);
			}
			
			if(attackCounter > 0.1){
				moveAmount = 0;
				
				spriteRenderer.sprite = playerOneAttackingLeft;
				
				attackingLeft = true;
				
				p1HitL.SetActive(true);
			}
			
			
			yield return null;
		}
		
		moveAmount = 140;
		isMoving = false;
		p1HitL.SetActive(false);
		attackingLeft = false;
	}
	
	IEnumerator AttackRightMovingAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.5) {
			
			if(Input.GetAxis ("DPad_Horizontal") < 0 || Input.GetAxis ("Horizontal") < 0 ||
			   Input.GetAxis ("DPad_Vertical") < 0 || Input.GetAxis ("Vertical") < 0){
				
				break;
			}
			
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerOneAttackingRight;
			attackingRight = true;
			
			p1HitR.SetActive(true);
			
			yield return null;
		}
		p1HitR.SetActive(false);
		attackingRight = false;
	}
	
	IEnumerator AttackLeftMovingAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.5) {

			if(Input.GetAxis ("DPad_Horizontal") > 0 || Input.GetAxis ("Horizontal") > 0 ||
			   Input.GetAxis ("DPad_Vertical") < 0 || Input.GetAxis ("Vertical") < 0){
				
				break;
			}

			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerOneAttackingLeft;
			attackingLeft = true;
			
			p1HitL.SetActive(true);
			
			yield return null;
		}
		p1HitL.SetActive(false);
		attackingLeft = false;
	}

	IEnumerator knockBackRight(){
		float knockBackCounter = 0.0f;
		float hitTimer = 0.0f;
		
		while (knockBackCounter < 1) {
			isHit = true;

			if(facingRight == true){
				spriteRenderer.sprite = playerOneJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerOneJumpingLeft;
			}
			
			knockBackCounter += Time.deltaTime;
			colorFlash += Time.deltaTime*15;
			hitTimer += Time.deltaTime;
			
			if((int)colorFlash%2==1 && isHit == true){
				renderer.color = new Color(255.0f, 0.0f, 0.0f, 1f);
			}
			
			if((int)colorFlash%2==0 && isHit == true){
				renderer.color = new Color(255.0f, 255.0f, 255.0f, 1f);
			}
			
			if(hitTimer < 0.5 && isHit == true){
				rb.velocity = new Vector3 (100, 100, 0);
				moveAmount = 0;
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 140;

				if(facingRight == true){
					spriteRenderer.sprite = playerOneStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerOneStandingLeft;
				}
				
				isHit = false;
			}
			
			yield return null;
		}
	}

	IEnumerator shieldKnockBackRight(){
		float knockBackCounter = 0.0f;
		float hitTimer = 0.0f;
		
		while (knockBackCounter < 1) {
			isHit = true;

			if(facingRight == true){
				spriteRenderer.sprite = playerOneJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerOneJumpingLeft;
			}
			
			knockBackCounter += Time.deltaTime;
			colorFlash += Time.deltaTime*15;
			hitTimer += Time.deltaTime;
			
			if((int)colorFlash%2==1 && isHit == true){
				renderer.color = new Color(255.0f, 0.0f, 0.0f, 1f);
			}
			
			if((int)colorFlash%2==0 && isHit == true){
				renderer.color = new Color(255.0f, 255.0f, 255.0f, 1f);
			}
			
			if(hitTimer < 0.5 && isHit == true){
				rb.velocity = new Vector3 (250, 0, 0);
				moveAmount = 0;
				spriteRenderer.sprite = playerOneJumpingRight;
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 140;

				if(facingRight == true){
					spriteRenderer.sprite = playerOneStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerOneStandingLeft;
				}
				
				isHit = false;
			}
			
			yield return null;
		}
	}
	
	IEnumerator knockBackLeft(){
		float knockBackCounter = 0.0f;
		float hitTimer = 0.0f;
		
		while (knockBackCounter < 1) {
			isHit = true;

			if(facingRight == true){
				spriteRenderer.sprite = playerOneJumpingRight;
			}

			if(facingLeft == true){
				spriteRenderer.sprite = playerOneJumpingLeft;
			}
			
			knockBackCounter += Time.deltaTime;
			colorFlash += Time.deltaTime*15;
			hitTimer += Time.deltaTime;
			
			if((int)colorFlash%2==1 && isHit == true){
				renderer.color = new Color(255.0f, 0.0f, 0.0f, 1f);
			}
			
			if((int)colorFlash%2==0 && isHit == true){
				renderer.color = new Color(255.0f, 255.0f, 255.0f, 1f);
			}
			
			if(hitTimer < 0.5 && isHit == true){
				rb.velocity = new Vector3 (-100, 100, 0);
				moveAmount = 0;
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 140;

				if(facingRight == true){
					spriteRenderer.sprite = playerOneStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerOneStandingLeft;
				}
				
				isHit = false;
			}
			
			yield return null;
		}
	}

	IEnumerator shieldKnockBackLeft(){
		float knockBackCounter = 0.0f;
		float hitTimer = 0.0f;
		
		while (knockBackCounter < 1) {
			isHit = true;

			if(facingRight == true){
				spriteRenderer.sprite = playerOneJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerOneJumpingLeft;
			}
			
			knockBackCounter += Time.deltaTime;
			colorFlash += Time.deltaTime*15;
			hitTimer += Time.deltaTime;
			
			if((int)colorFlash%2==1 && isHit == true){
				renderer.color = new Color(255.0f, 0.0f, 0.0f, 1f);
			}
			
			if((int)colorFlash%2==0 && isHit == true){
				renderer.color = new Color(255.0f, 255.0f, 255.0f, 1f);
			}
			
			if(hitTimer < 0.5 && isHit == true){
				rb.velocity = new Vector3 (-250, 0, 0);
				moveAmount = 0;
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 140;

				if(facingRight == true){
					spriteRenderer.sprite = playerOneStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerOneStandingLeft;
				}
				
				isHit = false;
			}
			
			yield return null;
		}
	}

	IEnumerator player1Fall(){
		float knockBackCounter = 0.0f;
		float hitTimer = 0.0f;
		
		while (knockBackCounter < 1) {
			isHit = true;
			
			knockBackCounter += Time.deltaTime;
			colorFlash += Time.deltaTime*15;
			hitTimer += Time.deltaTime;
			
			if((int)colorFlash%2==1 && isHit == true){
				renderer.color = new Color(255.0f, 0.0f, 0.0f, 1f);
			}
			
			if((int)colorFlash%2==0 && isHit == true){
				renderer.color = new Color(255.0f, 255.0f, 255.0f, 1f);
			}
			
			if(hitTimer < 0.5 && isHit == true){
				rb.velocity = new Vector3 (0, -400, 0);
				moveAmount = 70;
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 140;
				
				isHit = false;
			}
			
			yield return null;
		}
	}

	IEnumerator backToLife(){

		float respawnTime = 0.0f;

		while (respawnTime < 3) {

			respawnTime += Time.deltaTime;

			halo.SetActive (true);

			renderer.color = renderer.color = new Color (255.0f, 255.0f, 255.0f, 0.5f);

			rb.isKinematic = true;

			boxCol.enabled = false;
			
			jumpCounter = 0;

			spriteRenderer.sprite = playerOneStandingRight;

			if (Input.GetAxis ("DPad_Horizontal") > 0 || Input.GetAxis ("Horizontal") > 0 ||
			    Input.GetAxis ("DPad_Horizontal") < 0 || Input.GetAxis ("Horizontal") < 0 ||
			    Input.GetAxis ("DPad_Vertical") < 0 || Input.GetAxis ("Vertical") < 0 ||
			    Input.GetButtonDown ("PS_Square") || Input.GetButtonDown ("PS_X")){

				if(Input.GetButtonDown ("PS_X")){
					jumpCounter = 1;
				}

				if (Input.GetButtonDown ("PS_X") && jumpCounter < 4){
					rb.velocity = new Vector3(0, 300, 0);
				}
				
				break;
				
			} 
			
			yield return null;

		}

		halo.SetActive (false);

		rb.isKinematic = false;

		boxCol.enabled = true;

	}

	IEnumerator shieldStun(){
		float stunCounter = 0.0f;
		float knockUpCounter = 0.0f;
		
		while (stunCounter < 1) {
			
			stunCounter += Time.deltaTime;
			colorFlash += Time.deltaTime*15;
			knockUpCounter += Time.deltaTime;
			isHit = true;

			if(facingRight == true){
				spriteRenderer.sprite = playerOneStandingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerOneStandingLeft;
			}

			moveAmount = 0;

			playerOneShield.SetActive (false);
			playerOneShieldOn = false;

			playerOneShield.transform.localScale = 
				new Vector3(transform.localScale.x + 45, transform.localScale.y + 50, transform.localScale.z);
			
			if((int)colorFlash%2==1){
				renderer.color = new Color(255.0f, 0.0f, 0.0f, 1f);
			}
			
			if((int)colorFlash%2==0){
				renderer.color = new Color(255.0f, 255.0f, 255.0f, 1f);
			}
			
			if(knockUpCounter < 0.1){
				rb.velocity = new Vector3 (0, 100, 0);

				if(facingRight == true){
					spriteRenderer.sprite = playerOneJumpingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerOneJumpingLeft;
				}
			}
			if(knockUpCounter >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 140;
				isHit = false;
			}
			
			yield return null;
		}
	}


	public void Score(){
		winRound ++;
	}
}
