using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class playerTwoMove : MonoBehaviour {

	public float moveAmount = 10f;
	
	public float jumpSpeed = 10f;
	
	public GameObject playerTwo;
	public GameObject playerOne;
	public GameObject floor;

	public GameObject playerTwoScore;
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

	public GameObject p2HitR;
	public GameObject p2HitL;
	
	public bool playerOneShieldOn = false;
	public bool playerTwoShieldOn = false;
	
	public float Distance = 30.0f;

	private int jumpCounter = 0;
	
	Rigidbody2D rb;

	public BoxCollider2D boxCol;
	
	private SpriteRenderer spriteRenderer;
	
	public Sprite playerTwoStandingLeft;
	public Sprite playerTwoStandingRight;
	public Sprite playerTwoJumpingLeft;
	public Sprite playerTwoJumpingRight;
	public Sprite playerTwoRunningLeft1;
	public Sprite playerTwoRunningLeft2;
	public Sprite playerTwoRunningRight1;
	public Sprite playerTwoRunningRight2;
	public Sprite playerTwoAttackingLeft;
	public Sprite playerTwoAttackingRight;
	
	private bool facingLeft;
	private bool facingRight;
	private bool attackingLeft;
	private bool attackingRight;
	private bool isMoving;

	private bool airborn;
	private float runAnimation;
	public bool isHit;
	public bool groundPound2;
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
				
		playerTwoShield.SetActive(false);

		p2HitR.SetActive(false);
		p2HitL.SetActive(false);
		
		facingLeft = true;
		facingRight = false;
		airborn = false;
		offscreen = false;
		isHit = false;
		groundPound2 = false;
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
			spriteRenderer.sprite = playerTwoStandingRight;
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Text text = playerTwoScore.GetComponent<Text>();
		text.text = "Player Two: " + winRound;
		
		Vector3 moving = new Vector3 (transform.position.x,
		                              transform.position.y,
		                              transform.position.z);

		p2HitR.transform.position = playerTwo.transform.position;
		
		Vector3 Right = p2HitR.transform.position;
		Right.y += 13.0f;
		Right.x += 12.0f;
		p2HitR.transform.position = Right;
		
		
		p2HitL.transform.position = playerTwo.transform.position;
		
		Vector3 Left = p2HitL.transform.position;
		Left.y += 13.0f;
		Left.x -= 12.0f;
		p2HitL.transform.position = Left;

		if (Input.GetAxis ("DPad_Horizontal2") > 0 || Input.GetAxis ("Horizontal2") > 0 ||
		    Input.GetAxis ("DPad_Horizontal2") < 0 || Input.GetAxis ("Horizontal2") < 0 ||
		    Input.GetAxis ("DPad_Vertical2") < 0 || Input.GetAxis ("Vertical2") < 0) {
			
			isMoving = true;
		} else {
			isMoving = false;
		}
		
		if (Input.GetAxis ("DPad_Horizontal2") > 0 || Input.GetAxis ("Horizontal2") > 0) {
			moving.x += moveAmount * Time.deltaTime;
			facingRight = true;
			facingLeft = false;
			
			runningRight();
			
			
		} else if (Input.GetAxis ("DPad_Horizontal2") == 0 && facingRight == true && airborn == false ||
		           Input.GetAxis ("Horizontal2") == 0 && facingRight == true && airborn == false) {
			spriteRenderer.sprite = playerTwoStandingRight;
			
			runAnimation = 0;
		}
		
		if (Input.GetAxis ("DPad_Horizontal2") > 0 && airborn == true ||
		    Input.GetAxis ("Horizontal2") > 0 && airborn == true) {
			spriteRenderer.sprite = playerTwoJumpingRight;
		}
		
		if (Input.GetAxis ("DPad_Horizontal2") < 0 || Input.GetAxis ("Horizontal2") < 0) {
			moving.x -= moveAmount * Time.deltaTime;
			facingRight = false;
			facingLeft = true;
			
			runningLeft();
			
		} else if (Input.GetAxis ("DPad_Horizontal2") == 0 && facingLeft == true && airborn == false ||
		           Input.GetAxis ("Horizontal2") == 0 && facingLeft == true && airborn == false) {
			spriteRenderer.sprite = playerTwoStandingLeft;
			
			runAnimation = 0;
		}
		
		if (Input.GetAxis ("DPad_Horizontal2") < 0 && airborn == true ||
		    Input.GetAxis ("Horizontal2") < 0 && airborn == true) {
			spriteRenderer.sprite = playerTwoJumpingLeft;
		}

		if (Input.GetAxis ("DPad_Vertical2") < 0 && isHit == false && gpReady == true && airborn == true || 
		    Input.GetAxis ("Vertical2") < 0 && isHit == false && gpReady == true && airborn == true) {

			rb.velocity = new Vector3(0, -380, 0);

			groundPound2 = true;
		}
		
		
		if(Input.GetButtonDown ("PS_X2") && facingRight == true){
			jumpCounter ++;
			spriteRenderer.sprite = playerTwoJumpingRight;
			airborn = true;
		}
		
		if(Input.GetButtonDown ("PS_X2") && facingLeft == true){
			jumpCounter ++;
			spriteRenderer.sprite = playerTwoJumpingLeft;
			airborn = true;
		}
		
		if (Input.GetButtonDown ("PS_X2") && jumpCounter < 4){
			rb.velocity = new Vector3(0, 340, 0);

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

		if (Input.GetButtonDown ("PS_X2")) {
			groundPound2 = false;
		}

		if (groundPound2 == true) {
			renderer.color = new Color (0.0f, 255.0f, 0.0f, 1f);
		} else {
			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
		}
		
		//STATIONARY ATTACKS
		if (Input.GetButtonDown ("PS_Square2") && facingRight == true && airborn == false && isHit == false && isMoving == false) {
			StartCoroutine(AttackRightGroundCo());
		}

		if (Input.GetButtonDown ("PS_Square2") && facingLeft == true && airborn == false && isHit == false && isMoving == false) {
			StartCoroutine(AttackLeftGroundCo());
		}
		
		if (Input.GetButtonDown ("PS_Square2") && facingRight == true && airborn == true && isHit == false && isMoving == false) {
			StartCoroutine(AttackRightAirCo());
		}

		if (Input.GetButtonDown ("PS_Square2") && facingLeft == true && airborn == true && isHit == false && isMoving == false) {
			StartCoroutine(AttackLeftAirCo());
		}

		//MOVING ATTACKS
		if (Input.GetButtonDown ("PS_Square2") && facingRight == true && airborn == false && isHit == false && isMoving == true) {
			StartCoroutine(AttackRightMovingGroundCo());
		}

		if (Input.GetButtonDown ("PS_Square2") && facingLeft == true && airborn == false && isHit == false && isMoving == true) {
			StartCoroutine(AttackLeftMovingGroundCo());
		}
		
		if (Input.GetButtonDown ("PS_Square2") && facingRight == true && airborn == true && isHit == false && isMoving == true) {
			StartCoroutine(AttackRightMovingAirCo());
		}

		if (Input.GetButtonDown ("PS_Square2") && facingLeft == true && airborn == true && isHit == false && isMoving == true) {
			StartCoroutine(AttackLeftMovingAirCo());
		}

		if(Input.GetButtonDown ("PS_Square2")){

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

		if (Input.GetButton ("PS_Circle2") && isHit == false) {

			playerTwoShield.SetActive (true);
					
			playerTwoShieldOn = true;
					
			playerTwoShield.transform.position = playerTwo.transform.position;

			playerTwoShield.transform.localScale -= new Vector3(0.75F, 0.75f, 0);
					
		} else if (Input.GetButtonUp ("PS_Circle2")){

			playerTwoShield.SetActive (false);
					
			playerTwoShieldOn = false;

			playerTwoShield.transform.localScale = 
				new Vector3(transform.localScale.x + 45, transform.localScale.y + 50, transform.localScale.z);
		}

		if (playerTwoShield.transform.localScale.magnitude <= shieldX 
		    && playerTwoShield.transform.localScale.magnitude <= shieldY) {

			AudioClips[8].Play();
			StartCoroutine(shieldStun());
		}

		if (Vector3.Distance (playerTwo.transform.position, playerOneShield.transform.position) < Distance &&
		    GameObject.Find("Player1").GetComponent<playerOneMove>().playerOneShieldOn &&
		    attackingRight == true) {

			AudioClips[8].Play();
			StartCoroutine(shieldKnockBackLeft());
		}

		if (Vector3.Distance (playerTwo.transform.position, playerOneShield.transform.position) < Distance &&
		    GameObject.Find("Player1").GetComponent<playerOneMove>().playerOneShieldOn &&
		    attackingLeft == true) {

			AudioClips[8].Play();
			StartCoroutine(shieldKnockBackRight());
		}

		transform.position = moving;

		//Player Death

		if (transform.position.x < xDeathMin || transform.position.x > xDeathMax || transform.position.y < yDeath) {

			AudioClips[9].Play();
			
			playerOne.GetComponent<playerOneMove>().Score();
			
			Vector3 respawn = new Vector3 (50, 50, 0);
			
			transform.position = respawn;

			spriteRenderer.sprite = playerTwoStandingLeft;
			
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
			facingLeft = true;
			facingRight = false;
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "ground") {
			airborn = false;
			gpReady = false;
			groundPound2 = false;
		}

		if (coll.gameObject.tag == "player1") {
			jumpCounter = 0;
			airborn = false;
//			gpReady = false;
//			groundPound2 = false;
		}

		if (coll.gameObject.tag == "ground" && facingLeft == true) {
			jumpCounter = 0;
			spriteRenderer.sprite = playerTwoStandingLeft;
			
			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
			colorFlash = 0;
			moveAmount = 150;
			
			justDied = false;
		}

		if (coll.gameObject.tag == "ground" && facingRight == true && justDied == false) {
			jumpCounter = 0;
			spriteRenderer.sprite = playerTwoStandingRight;

			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
			colorFlash = 0;
			moveAmount = 150;
		}

		if (coll.gameObject.tag == "ground" && facingRight == true && justDied == true) {
			jumpCounter = 0;
			spriteRenderer.sprite = playerTwoStandingLeft;
			
			renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
			colorFlash = 0;
			moveAmount = 150;

			justDied = false;
		}

		if (coll.gameObject.tag == "player1" && 
		    GameObject.Find("Player1").GetComponent<playerOneMove>().groundPound1 &&
		    playerOneShieldOn == false) {

			AudioClips[8].Play();
			StartCoroutine(player2Fall());
		}

		if (coll.gameObject.tag == "player1" &&
			GameObject.Find ("Player1").GetComponent<playerOneMove> ().isHit) {

			groundPound2 = false;
		}

		if (coll.gameObject.tag == "player1AttackRight" && playerTwoShieldOn == false) {
			StartCoroutine(knockBackRight());

			int random = Random.Range (1, 3);
			
			if(random == 1){
				AudioClips[6].Play();
			}
			
			if(random == 2){
				AudioClips[7].Play();
			}
		}

		if (coll.gameObject.tag == "player1AttackLeft" && playerTwoShieldOn == false) {
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
			spriteRenderer.sprite = playerTwoRunningRight1;
		}
		
		if((int)runAnimation%2==0){
			spriteRenderer.sprite = playerTwoRunningRight2;
		}
	}
	
	void runningLeft(){
		runAnimation += Time.deltaTime*14;
		
		
		if((int)runAnimation%2==1){
			spriteRenderer.sprite = playerTwoRunningLeft1;
		}
		
		if((int)runAnimation%2==0){
			spriteRenderer.sprite = playerTwoRunningLeft2;
		}
	}

	//STATIONARY ATTACKS
	IEnumerator AttackRightGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackGroundTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerTwoAttackingRight;
			attackingRight = true;
			
			p2HitR.SetActive(true);
			
			yield return null;
		}
		
		p2HitR.SetActive(false);
		attackingRight = false;
	}
	
	IEnumerator AttackLeftGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackGroundTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerTwoAttackingLeft;
			attackingLeft = true;
			
			p2HitL.SetActive(true);
			
			yield return null;
		}
		
		p2HitL.SetActive(false);
		attackingLeft = false;
	}
	
	IEnumerator AttackRightAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackAirTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerTwoAttackingRight;
			attackingRight = true;
			
			p2HitR.SetActive(true);
			
			yield return null;
		}
		p2HitR.SetActive(false);
		attackingRight = false;
	}
	
	IEnumerator AttackLeftAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < attackAirTime) {
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerTwoAttackingLeft;
			attackingLeft = true;
			
			p2HitL.SetActive(true);
			
			yield return null;
		}
		p2HitL.SetActive(false);
		attackingLeft = false;
	}
	
	
	//MOVING ATTACKS
	IEnumerator AttackRightMovingGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.25f) {
			attackCounter += Time.deltaTime;
			
			if(attackCounter <= 0.1){
				rb.velocity = new Vector3 (300, 0, 0);
			}
			
			if(attackCounter > 0.1){
				moveAmount = 0;
				
				spriteRenderer.sprite = playerTwoAttackingRight;
				
				attackingRight = true;
				
				p2HitR.SetActive(true);
			}
			
			
			yield return null;
		}
		
		moveAmount = 150;
		isMoving = false;
		p2HitR.SetActive(false);
		attackingRight = false;
	}
	
	IEnumerator AttackLeftMovingGroundCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.25f) {
			attackCounter += Time.deltaTime;
			
			if(attackCounter <= 0.1){
				rb.velocity = new Vector3 (-300, 0, 0);
			}
			
			if(attackCounter > 0.1){
				moveAmount = 0;
				
				spriteRenderer.sprite = playerTwoAttackingLeft;
				
				attackingLeft = true;
				
				p2HitL.SetActive(true);
			}
			
			
			yield return null;
		}
		
		moveAmount = 150;
		isMoving = false;
		p2HitL.SetActive(false);
		attackingLeft = false;
	}
	
	IEnumerator AttackRightMovingAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.5) {
			
			if(Input.GetAxis ("DPad_Horizontal2") < 0 || Input.GetAxis ("Horizontal2") < 0 ||
			   Input.GetAxis ("DPad_Vertical2") < 0 || Input.GetAxis ("Vertical2") < 0){
				
				break;
			}
			
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerTwoAttackingRight;
			attackingRight = true;
			
			p2HitR.SetActive(true);
			
			yield return null;
		}
		p2HitR.SetActive(false);
		attackingRight = false;
	}
	
	IEnumerator AttackLeftMovingAirCo(){
		float attackCounter = 0.0f;
		
		while (attackCounter < 0.5) {
			
			if(Input.GetAxis ("DPad_Horizontal2") > 0 || Input.GetAxis ("Horizontal2") > 0 ||
			   Input.GetAxis ("DPad_Vertical2") < 0 || Input.GetAxis ("Vertical2") < 0){
				
				break;
			}
			
			attackCounter += Time.deltaTime;
			spriteRenderer.sprite = playerTwoAttackingLeft;
			attackingLeft = true;
			
			p2HitL.SetActive(true);
			
			yield return null;
		}
		p2HitL.SetActive(false);
		attackingLeft = false;
	}

	IEnumerator knockBackRight(){
		float knockBackCounter = 0.0f;
		float hitTimer = 0.0f;
		
		while (knockBackCounter < 1) {
			isHit = true;

			if(facingRight == true){
				spriteRenderer.sprite = playerTwoJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerTwoJumpingLeft;
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
				moveAmount = 150;

				if(facingRight == true){
					spriteRenderer.sprite = playerTwoStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerTwoStandingLeft;
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
				spriteRenderer.sprite = playerTwoJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerTwoJumpingLeft;
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
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 150;

				if(facingRight == true){
					spriteRenderer.sprite = playerTwoStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerTwoStandingLeft;
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
				spriteRenderer.sprite = playerTwoJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerTwoJumpingLeft;
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
				moveAmount = 150;

				if(facingRight == true){
					spriteRenderer.sprite = playerTwoStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerTwoStandingLeft;
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
				spriteRenderer.sprite = playerTwoJumpingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerTwoJumpingLeft;
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
				moveAmount = 150;

				if(facingRight == true){
					spriteRenderer.sprite = playerTwoStandingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerTwoStandingLeft;
				}
				
				isHit = false;
			}
			
			yield return null;
		}
	}


	IEnumerator player2Fall(){
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
				moveAmount = 75;
			}
			if(hitTimer >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 150;
				
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
			
			spriteRenderer.sprite = playerTwoStandingLeft;
			
			if (Input.GetAxis ("DPad_Horizontal2") > 0 || Input.GetAxis ("Horizontal2") > 0 ||
			    Input.GetAxis ("DPad_Horizontal2") < 0 || Input.GetAxis ("Horizontal2") < 0 ||
			    Input.GetAxis ("DPad_Vertical2") < 0 || Input.GetAxis ("Vertical2") < 0 ||
			    Input.GetButtonDown ("PS_Square2") || Input.GetButtonDown ("PS_X2")){

				if(Input.GetButtonDown ("PS_X2")){
					jumpCounter = 1;
				}
				
				if (Input.GetButtonDown ("PS_X2") && jumpCounter < 4){
					rb.velocity = new Vector3(0, 340, 0);
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
				spriteRenderer.sprite = playerTwoStandingRight;
			}
			
			if(facingLeft == true){
				spriteRenderer.sprite = playerTwoStandingLeft;
			}
			
			moveAmount = 0;
			
			playerTwoShield.SetActive (false);
			playerTwoShieldOn = false;
			
			playerTwoShield.transform.localScale = 
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
					spriteRenderer.sprite = playerTwoJumpingRight;
				}
				
				if(facingLeft == true){
					spriteRenderer.sprite = playerTwoJumpingLeft;
				}
			}
			if(knockUpCounter >= 1){
				renderer.color = new Color (255.0f, 255.0f, 255.0f, 1f);
				colorFlash = 0;
				moveAmount = 150;
				isHit = false;
			}
			
			yield return null;
		}
	}

	
	public void Score(){
		winRound ++;
	}
}
