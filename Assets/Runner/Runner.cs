using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {
	public int acceleration;
	public static float distanceTraveled;

	public Vector3 jumpVelocity, boostVelocity;

	public float gameOverY;

	private static int boosts;

	private bool touchingPlatform;
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		startPosition = transform.localPosition;
		renderer.enabled = false;
		rigidbody.isKinematic = true;
		enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(touchingPlatform){
			rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
		}

		if(touchingPlatform && (Input.GetButtonDown("Jump") || Input.GetTouch(0).phase == TouchPhase.Began)){
			rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
		}
		else if(boosts > 0 && (Input.GetButtonDown("Jump") || Input.GetTouch(0).phase == TouchPhase.Began)){
			rigidbody.AddForce(boostVelocity, ForceMode.VelocityChange);
			boosts--;
			GUIManager.SetBoosts(boosts);
		}

		if(transform.localPosition.y < gameOverY){
			GameEventManager.TriggerGameOver();
		}

		distanceTraveled = transform.localPosition.x;
		GUIManager.SetDistance(distanceTraveled);
	}
	
	void OnCollisionEnter () {
		touchingPlatform = true;
	}
	
	void OnCollisionExit () {
		touchingPlatform = false;
	}

	private void GameStart () {
		boosts = 0;
		distanceTraveled = 0f;
		transform.localPosition = startPosition;
		renderer.enabled = true;
		rigidbody.isKinematic = false;
		enabled = true;
	}
	
	private void GameOver () {
		renderer.enabled = false;
		rigidbody.isKinematic = true;
		enabled = false;
	}

	public static void AddBoost(){
		boosts++;
		GUIManager.SetBoosts(boosts);
		return;
	}
}
