using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public bool shooting;
	public float shotSpeed;
	public Player Mans;

	// Use this for initialization
	void Start () {
		Mans = FindObjectOfType<Player>();
		shooting = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3 (0, shotSpeed * .5f, 0));
		transform.localScale = transform.localScale - new Vector3 (0, shotSpeed, 0);
		transform.position.Set (transform.position.x, Mans.transform.position.y, 0);
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Floor") {
			shooting = false;
			gameObject.SetActive (false);
		}

		if (coll.gameObject.tag == "Ball") {
			Mans.PlayPopSound();
			shooting = false;
			coll.gameObject.GetComponent<Ball> ().Pop ();
			gameObject.SetActive (false);
		}
	}
}
