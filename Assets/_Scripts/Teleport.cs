using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		GameObject go = other.gameObject;
		float direction = this.transform.position.x - go.transform.position.x;
		Vector3 newPos = other.transform.position;
		if (direction > 0)
		{
			newPos.x = -8;
			if (other.tag == "Player")
				other.GetComponent<Player> ().TpRight ();
			else
				other.GetComponent<GhostMove> ().TpRight ();
		}
		else if (direction < 0)
		{
			newPos.x = 8;
			if (other.tag == "Player")
				other.GetComponent<Player> ().TpLeft ();
			else
				other.GetComponent<GhostMove> ().TpLeft ();

		}
		go.transform.position = newPos;
	}
}
