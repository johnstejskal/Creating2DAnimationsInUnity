using UnityEngine;
using System.Collections;


//==========================
// Author - @JohnStejskal
// www.johnstejskal.com
// johnstejskal@gamail.com
//==========================

public class PlayerController : MonoBehaviour {

	public float walkSpeed = 1; // player left right walk speed
	private bool _isGrounded = true; // is player on the ground?

	Animator animator;

	//some flags to check when certain animations are playing
	bool _isPlaying_crouch = false;
	bool _isPlaying_walk = false;
	bool _isPlaying_hadooken = false;

	//animation states - the values in the animator conditions
	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;
	const int STATE_CROUCH = 2;
	const int STATE_JUMP = 3;
	const int STATE_HADOOKEN = 4;
	
	string _currentDirection = "left";
	int _currentAnimationState = STATE_IDLE;


	// Use this for initialization
	void Start()
	{
		//define the animator attached to the player
		animator = this.GetComponent<Animator>();
	}
	
	// FixedUpdate is used insead of Update because I used rigidbody2D physics for the jump
	void FixedUpdate()
	{
		//if Space ket is down
		if (Input.GetKeyDown (KeyCode.Space))
		{
			changeState (STATE_HADOOKEN);	
		}
		else if (Input.GetKey ("up") && !_isPlaying_hadooken && !_isPlaying_crouch)
		{
			if(_isGrounded)
			{
				_isGrounded = false;
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 250)); //simple jump code using unity physics
				changeState(STATE_JUMP);
			}
		}
		else if (Input.GetKey ("down"))
		{
			changeState(STATE_CROUCH);	
		}
		else if (Input.GetKey ("right") && !_isPlaying_hadooken )
		{
			changeDirection ("right");
			transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);

			if(_isGrounded)
			changeState(STATE_WALK);
			
		} 
		else if (Input.GetKey ("left") && !_isPlaying_hadooken)
		{
			changeDirection ("left");
			transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);

			if(_isGrounded)
			changeState(STATE_WALK);
			
		}
		else
		{	
			if(_isGrounded)
			changeState(STATE_IDLE);
		}


		//check if crouch animation is playing
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("ken_crouch"))
			_isPlaying_crouch = true;	
		else
			_isPlaying_crouch = false;

		//check if hadooken animation is playing
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("ken_hadooken"))
			_isPlaying_hadooken = true;	
		else
			_isPlaying_hadooken = false;

		//check if strafe animation is playing	
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("ken_walk"))
			_isPlaying_walk = true;	
		else
			_isPlaying_walk = false;


	}

	//--------------------------------------
	// Change the players animation state
	//--------------------------------------
	void changeState(int state){


		if (_currentAnimationState == state)
		return;

		switch (state) {
				
		case STATE_WALK:
			animator.SetInteger ("state", STATE_WALK);
			break;

		case STATE_CROUCH:
			animator.SetInteger ("state", STATE_CROUCH);
			break;

		case STATE_JUMP:
			animator.SetInteger ("state", STATE_JUMP);
			break;

		case STATE_IDLE:
			animator.SetInteger ("state", STATE_IDLE);
			break;
			
		case STATE_HADOOKEN:
			animator.SetInteger ("state", STATE_HADOOKEN);
			break;

		}

		_currentAnimationState = state;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.name == "Floor")
		{
			_isGrounded = true;
			changeState(STATE_IDLE);

		}
		
	}


	//--------------------------------------
	// Flip player sprite for left/right walking
	//--------------------------------------
	void changeDirection(string direction)
	{

		if (_currentDirection != direction)
		{
			if (direction == "right")
			{
				transform.Rotate (0, 180, 0);
				_currentDirection = "right";
			} 
			else if (direction == "left") 
			{
				transform.Rotate (0, -180, 0);
				_currentDirection = "left";
			}
		}
		
	}

}
