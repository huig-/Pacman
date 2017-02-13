using UnityEngine;
using System.Collections;

public class PacDot : MonoBehaviour {

	public static int points = 10;
	public static GameController gController;
	// Use this for initialization
	void Start () {
		gController = GameObject.Find ("Main Camera").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			gController.PacDotConsumed (points, false);
			Destroy (this.gameObject);
			//RenderTexture.active = null;
			//unity RenderTexture warning: Destroying active render texture. Switching to main context.
		}
	}
}
