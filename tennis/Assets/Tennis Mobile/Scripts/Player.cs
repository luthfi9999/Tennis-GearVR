using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {
	
	public float speed;
	public float turnSpeed;
	public float ballRange;
	public float force;
	public float upForce;
	public float moveRange;
	public float powerbarSpeedMin;
	public float powerbarSpeedMax;
	public float powerbarMaxSlowdown;
	public float ballMoveSpeed;
	
	public Animator anim;
	public Transform head;
	public Transform lookAt;
	public ParticleSystem moveParticles;
	public Transform ball;
	public Animator indicator;
	public Animator rangeCircle;
	public Image indicatorFill;
	public Transform opponent;
	public Transform ballPosition;
	public Animator comboLabel;
	public Text comboNumberLabel;
	public AudioSource hitAudio;
	public AudioSource fireBallAudio;
	public RandomPitch randomPitch;
	public Animator swipeLabel;
	public Animator shootTip;
	public Transform arrowHolder;
	public Animator playerArrow;
	public Animator startPanel;
	public GameObject gamePanel;
	public GameObject scoreTexts;
	public GameObject matchLabel;
	public Transform racket;
	public GameObject brokenRacket;
	public Animator diamondEffect;
	
	public CameraMovement cameraMovement;
	public EncouragingText encouragingTexts;
	
	public float maxDragTime;
	public float dragDistance;
	public float swipeSensitivity;
	
	public Text tesku; 
	public bool hitted=false;
	bool showBar;
	
	int swipeLabelCount;
	GameObject peramalan;
	Vector3 startPos;
	Vector3 startVelocity;
	float startTime;
	
	float powerbarSpeed;
	public int tech = 4;
	bool right;
	bool moving;
	
	float lastDist;
	float delta;
	
	bool max;
	
	float lastFillAmount=0.7f;
	
	Vector3 target;
	
	bool canShoot;
	peramalan ramal;
	float rotation;
	
	bool shoot = true;
	
	GameManager manager;
	
    void Start(){
		ramal= this.GetComponent<peramalan>();
		//initialize the manager, game panel, range, power bar and particles
		manager = GameObject.FindObjectOfType<GameManager>();
		
		gamePanel.SetActive(false);
		
        rangeCircle.SetBool("Show", false);
		moveParticles.Stop();
		
		powerbarSpeed = UnityEngine.Random.Range(powerbarSpeedMin, powerbarSpeedMax);
		
		//at the start of the game, show the tournament number instead of the match scores
		if(scoreTexts != null){
			scoreTexts.SetActive(false);
		
			int tournament = PlayerPrefs.GetInt("Tournament") + 1;
			matchLabel.GetComponent<Text>().text = "Tournament #" + tournament;
		}
    }

    void Update(){
		OVRInput.Update();
		//check if the player should move on the x axis
		Move();
		
		//show the indicator and zoom towards the player
		if(showBar){
			if(!indicator.GetBool("Active")){
				indicator.SetBool("Active", true);
				
				indicatorFill.fillAmount = 0;
				delta = 1f;	

				cameraMovement.Zoom(true);
			}
			
			//also update the bar to go up and down
			float fillSpeed = indicatorFill.fillAmount < 0.2f ? 0.2f : indicatorFill.fillAmount;
				
			if(indicatorFill.fillAmount > 0.95f && delta > 0){
				fillSpeed /= powerbarMaxSlowdown;
			}
			
			if(indicatorFill.fillAmount > 0.8f && !max)
				StartCoroutine(PowerBarMax());
				
			indicatorFill.fillAmount += Time.deltaTime * powerbarSpeed * delta * fillSpeed;
				
			if((delta < 0 && indicatorFill.fillAmount < 0.03f) || (delta > 0 && indicatorFill.fillAmount > 0.99f))				
				delta *= -1f;
		}
		else{
			//disable the bar if it should not be shown
			if(indicator.GetBool("Active"))
				indicator.SetBool("Active", false);
		}
		
		if(ball == null && !showBar)
			return;
		
		//check if the ball is in range
		CheckBallRange();
		
		//check if we can shoot
		if(rangeCircle.GetBool("Show"))			
			Shoot();
    }
	
	//look at opponent
	void LateUpdate(){
		head.LookAt(lookAt.position);
	}
	
	//check if we can hit the ball
	void CheckBallRange(){
		float zDistance = ball == null ? 0 : Mathf.Abs(transform.position.z - ball.position.z);
		
		if(showBar || zDistance < ballRange && !ball.GetComponent<Ball>().inactive){
			CanHitBall();
		}
		else if(rangeCircle.GetBool("Show")){
			rangeCircle.SetBool("Show", false);
		}
	}
	
	//if the ball is close enough, show the range circle and for the first match show a label that tells the player to swipe
	void CanHitBall(){
		if(rangeCircle.GetBool("Show"))
			return;
		
		rangeCircle.SetBool("Show", true);
		
		if(PlayerPrefs.GetInt("Match") == 0 && !showBar && swipeLabelCount < 3){
			swipeLabel.SetTrigger("Play");
			
			swipeLabelCount++;
		}
	}
	
	//move left and right towards the target and update animations accordingly
	void Move(){
		if(target == Vector3.zero)
			return;
		
		float dist = Vector3.Distance(transform.position, target);
		moving = dist > 0.1f && !rangeCircle.GetBool("Show");
		
		if(moving){
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

			right = target.x < transform.position.x;
		}
		
		if(anim.GetBool("Right") != right)
			anim.SetBool("Right", right);
		
		if(!moving){
			rotation = 180;
		
			if(moveParticles.isPlaying)
				moveParticles.Stop();
		}
		else{
			if(right){
				rotation = -90;
			}
			else{
				rotation = 91;
			}
		
			if(!moveParticles.isPlaying)
				moveParticles.Play();
		}
		
		anim.SetBool("Moving", moving);
		
		Vector3 rot = transform.eulerAngles;
		rot.y = rotation;
		
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * turnSpeed);
	}
	
	//checks for swipe motion to see if we want to shoot the ball
	void Shoot(){
		//Vector3 currentPos = Input.mousePosition;
		if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
		{
			canShoot = true;
			startTime = Time.time;
			if (ramal.ready&&ball!=null)
			{
				startVelocity = ball.GetComponent<Rigidbody>().velocity;

				ball.GetComponent<Rigidbody>().isKinematic = true;
			}

		}

		else if(OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && canShoot){
			if (Time.time - startTime > maxDragTime)
			{
				ResetShot();
			}
			else
			{
				if(hitted)
					Hit();
				

			}
		}
	}
	
	//reset if swipe wasn't correct
	void ResetShot(){
		canShoot = false;
				
		if(lastFillAmount < 0){
			ball.GetComponent<Rigidbody>().isKinematic = false;
			ball.GetComponent<Rigidbody>().velocity = startVelocity;
		}
	}
	
	//show an animation on the power indicator so if flashes
	IEnumerator PowerBarMax(){
		max = true;
		indicator.SetTrigger("Max");
		
		yield return new WaitForSeconds(0.3f);
		
		max = false;
	}
	
	//hit the ball
	void Hit(){
		hitted=false;
		if(manager.scored==false){
			//anim.SetTrigger("Hit right");
			
			randomPitch.Set();
			hitAudio.Play();
		}
		else{
			manager.Serve();
			cameraMovement.Zoom(false);
			manager.scored = false;	
			showBar = false;
		}
		
		int max = 2 + PlayerPrefs.GetInt("Bonus max");
		
		//for the last diamond in the bonus level, break the player racket
		if(manager.bonus && manager.bonusDiamonds >= max){
			racket.gameObject.SetActive(false);
			Instantiate(brokenRacket, racket.position, racket.rotation);
		}
		
		//initialize the camera movement and score label
		if(!cameraMovement.enabled){
			cameraMovement.enabled = true;
			
			startPanel.SetTrigger("Hide");
			gamePanel.SetActive(true);
			
			if(scoreTexts != null){
				scoreTexts.SetActive(true);
				matchLabel.SetActive(false);
			}
		}
		startPos = this.transform.position;
		Vector3 currentPos=startPos;
	

		//tesku.text=startPos.x.ToString();
		//use the swipe to check the ball direction
		float difference = currentPos.x - startPos.x;
		float xPos = swipeSensitivity * -difference;
		xPos = Mathf.Clamp(xPos, -moveRange, moveRange);
		
		Vector3 random = new Vector3(xPos, 0, opponent.position.z);
			if (tech == 1)
		{
			 random.Set(xPos-3, 0, opponent.position.z);
			  tech=5;
		}
		else if (tech == 0)
		{      random.Set(xPos+3, 0, opponent.position.z);
		tech=5;
			 //currentPos = new Vector3(startPos.x - 300, startPos.y, startPos.z);
			 //tesku.text="z";
		}
		Rigidbody rb = ball.GetComponent<Rigidbody>();
		Ball ballScript = ball.GetComponent<Ball>();
			
		ballScript.SetLastHit(true);
		
		Vector3 direction = (random - transform.position).normalized;
			
		opponent.GetComponent<Opponent>().SetTarget(random);
		
		arrowHolder.LookAt(random);
		playerArrow.SetTrigger("Show");
		
		//check if this is the serve, or if we're just hitting the ball during the game and set the ball velocity
		if(lastFillAmount < 0){
			rb.velocity = direction * force + Vector3.up * upForce;
		}
		else{			
			StartCoroutine(ServeAnim(rb, direction, lastFillAmount > 0.8f));
			
			canShoot = false;
			StartCoroutine(DisableShooting());
			
			return;
		}
				
		rb.isKinematic = false;
			
		canShoot = false;
			
		StartCoroutine(cameraMovement.Shake(0.12f, 0.5f));
			
		StartCoroutine(DisableShooting());
		
	}
	
	//serve animation showing the popup text and screenshake
	IEnumerator ServeAnim(Rigidbody rb, Vector3 direction, bool powerShot){			
		//anim.SetTrigger(powerShot ? "Power serve" : "Serve");
		
		yield return new WaitForSeconds(0.28f);
		
		float barForce = force * 0.8f + (force * lastFillAmount * 0.3f);
		rb.velocity = direction * barForce + Vector3.up * upForce;
		
		encouragingTexts.ShowText(lastFillAmount);
		
		if(lastFillAmount > 0.95f && !manager.bonus){
			diamondEffect.SetTrigger("Play");
			PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") + 1);
		}
		
		randomPitch.Set();
		hitAudio.Play();
		
		rb.isKinematic = false;
			
		StartCoroutine(cameraMovement.Shake(0.12f, 0.5f));
	}
	
	//stop shooting for .7 seconds
	IEnumerator DisableShooting(){
		shoot = false;
		
		yield return new WaitForSeconds(.7f);
		
		shoot = true;
	}
	
	//show flames on the ball and play fire ball audio
	public void ComboDone(Ball ballScript){
		if(ballScript != null){
			ballScript.Flames();
			fireBallAudio.Play();
		}
	}
	
	//set target for the player character (done by the opponent)
	public void SetTarget(Vector3 pos){
		this.target = pos;
	}
	
	//enable or disable the power bar
	public void SetBar(bool active){
		showBar = active;
	}
}
