using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	
	public float speed;
	public float multiplier;
	public float maxSpeed;
	public float neutralSlowConstant;
	public float neutralSlowBounds;
	public float turnaroundSlowConstant;
	public float jumpForce;
	public float downForce;
	public float bounceForce;
	public int playerNum;
	private string addTo;
	private Rigidbody2D rb;
	private bool grounded;
	public GameObject gameOverText;
	public GameObject line;
	private bool amShooting;
	private GameObject lining;
	public float shotSpeed;
	private float regTime;
	private Animator animate;
	public AudioClip popSound;
	public AudioClip jumpSound;
	public GameObject jumpDust;

	// Use this for initialization
	void Start () {
		animate = GetComponent<Animator> ();
		regTime = Time.timeScale;
		grounded = false;
		speed = 0;
		if (playerNum > 1) {
			addTo = "2";
		} else {
			addTo = "";
		}
		rb = GetComponent<Rigidbody2D> ();
		amShooting = false;
		animate.Play ("Stand");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Reset")) {
			Time.timeScale = regTime;
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}


		//Walking
		if (Input.GetButton ("Horizontal" + addTo)) {
			var direction = Input.GetAxis ("Horizontal" + addTo);

			if (direction < 0) {
				//left movement

				if (speed > 0) {
					//if it's moving in the opposite direction
					speed = speed * turnaroundSlowConstant;
				}
				speed = speed - Time.deltaTime * multiplier;
			}
			if (direction > 0) {
				//right movement

				if (speed < 0) {
					//if it's moving in the opposite direction
					speed = speed * turnaroundSlowConstant;
				}
				speed = speed + Time.deltaTime * multiplier;
			}
		} 
		if (!Input.GetButton ("Horizontal" + addTo)) {
			animate.Play ("Stand");
			if ((speed > neutralSlowBounds) || (speed < (-1 * neutralSlowBounds))) {
				speed = speed * neutralSlowConstant;
			} else {
				speed = 0;
			}
		}
		if (speed > maxSpeed) {
			speed = maxSpeed;
		} else if (speed < (-1 * maxSpeed)) {
			speed = -1 * maxSpeed;
		}
		if (Input.GetButtonDown ("Jump" + addTo) && grounded) {
			Instantiate (jumpDust, transform.position - new Vector3 (0, 1.5f, 0), Quaternion.identity);
			GetComponent<AudioSource> ().PlayOneShot(jumpSound);
			rb.AddForce (transform.up * jumpForce);
		}
		if (speed != 0 && grounded) {
			animate.Play ("Walk");
		}

		rb.velocity = new Vector2 (speed, rb.velocity.y);

		if (!amShooting) {
			if (Input.GetButtonDown ("Shoot")) {
				amShooting = true;
				rb.AddForce (transform.up * -1f * downForce);
			}
		}


//		if (!amShooting) {
//			if (Input.GetButtonDown ("Shoot" + addTo)) {
//				amShooting = true;
//				lining = Instantiate (line, transform.position - new Vector3 (.5f, 4f, 0), Quaternion.identity) as GameObject;
//			}
//		} else {
//			if (amShooting) {
//				amShooting = lining.GetComponent<Shot> ().shooting;
//			}
//		}
	}

	void OnCollisionStay2D(Collision2D coll){
		if(coll.gameObject.tag == "Floor") {
			grounded = true;
			amShooting = false;
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Ball") {
//			if (Mathf.Abs (transform.position.x - coll.gameObject.transform.position.x) < Mathf.Abs (transform.position.y - coll.gameObject.transform.position.y) && transform.position.y > coll.gameObject.transform.position.y) {
			if (transform.position.y > coll.gameObject.transform.position.y + 1.5f){
				amShooting = false;
				PlayPopSound ();
				coll.gameObject.GetComponent<Ball> ().Pop();
				rb.velocity = new Vector2 (rb.velocity.x, 0);
				rb.AddForce (transform.up * bounceForce);
			} else {
				GameOver ();
			}
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if(coll.gameObject.tag == "Floor") {
			grounded = false;
			animate.Play ("Jump");
		}
	}

	void GameOver() {
		gameOverText.SetActive (true);
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		foreach (GameObject ball in balls) {
			ball.GetComponent<Ball> ().SetHasEnded (true);
		}
		Time.timeScale = 0f;
	}

	public void PlayPopSound(){
		GetComponent<AudioSource> ().PlayOneShot (popSound);
	}
}
