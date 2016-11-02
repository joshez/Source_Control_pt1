using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Vector3 direction;
	public GameObject ballMediumPrefabL;
	public GameObject ballMediumPrefabR;
	public bool smallest;
	private bool hasEnded;

	// Use this for initialization
	void Start () {
		hasEnded = false;
	}

	// Update is called once per frame
	void Update () {
		if (!hasEnded) {
			transform.Translate (direction);
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			direction.x = -1 * direction.x;
		}
		if (coll.gameObject.tag == "Floor") {
			direction.y = -1 * direction.y;
		}
		if (coll.gameObject.tag == "Ball") {
			if (Mathf.Abs(transform.position.x - coll.gameObject.transform.position.x) > Mathf.Abs(transform.position.y - coll.gameObject.transform.position.y)){
				direction.x = -1 * direction.x;
			} else {
				direction.y = -1 * direction.y;
			}
		}
	}

	public void Pop(){
		gameObject.SetActive (false);
		if (!smallest) {
			Instantiate (ballMediumPrefabL, gameObject.transform.position - new Vector3 (4f,0,0), Quaternion.identity);
			Instantiate (ballMediumPrefabR, gameObject.transform.position + new Vector3 (4f,0,0), Quaternion.identity);
		}
	}

	public void SetHasEnded(bool now){
		hasEnded = now;
	}
}
