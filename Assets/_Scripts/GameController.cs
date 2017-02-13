using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject pacdotPrefab;
	public GameObject powerUpPrefab;
	static public Transform PACDOT_ANCHOR;
	public TextMesh scoreLabel;
	public TextMesh livesLabel;
	//public Material materialRef;

	public bool __________________;

	private int _score;
	private int _lives;
	private int _totalDots;
	private int _dotsConsumed = 0;

	void Awake() {
		_score = 0;
		_lives = 2;
		scoreLabel = GameObject.Find ("ScoreCounter").GetComponent<TextMesh> ();
		livesLabel = GameObject.Find ("LivesCounter").GetComponent<TextMesh> ();
		if (PACDOT_ANCHOR == null) {
			GameObject go = new GameObject ("_PACDOT_ANCHOR");
			PACDOT_ANCHOR = go.transform;
		}
		//Renderer[] rends = GameObject.Find ("Maze").GetComponentsInChildren<Renderer> ();
		//for (int i = 0; i < rends.Length; i++) {
		//	rends [i].material = materialRef;
		//}
	}

	void Start() {
		InitMaze ();
		StartCoroutine (StartCountdown());
	}

	void InitMaze() {
		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 17; j++) {
				if (GameField.grid [i,j] == 1 && !(GameField.startPos.i == i && GameField.startPos.j == j)) {
					GameObject go;
					if (GameField.IsPowerUp (new Tile (i, j))) {
						go = Instantiate (powerUpPrefab) as GameObject;
					} else {
						go = Instantiate (pacdotPrefab) as GameObject;
					}
					Vector3 pos = GameField.CellToWorld(new Tile(i,j));
					go.transform.position = pos;
					go.tag = "Pacdot";
					go.transform.parent = PACDOT_ANCHOR;
				}
			}
		}
		_totalDots = GameObject.FindGameObjectsWithTag ("Pacdot").Length;
	}

	void InitGhosts() {
		GameObject.Find ("Blinky").GetComponent<GhostMove>().Reset();
		GameObject.Find ("Pinky").GetComponent<GhostMove>().Reset();
		GameObject.Find ("Inky").GetComponent<GhostMove>().Reset();
		GameObject.Find ("Clyde").GetComponent<GhostMove>().Reset();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PacDotConsumed(int pacdot_points, bool powerUp) {
		_dotsConsumed++;
		_score += pacdot_points;
		scoreLabel.text = _score.ToString();
		if (_dotsConsumed == 30) {
			GameObject.Find ("Inky").GetComponent<GhostMove> ().state = GhostMove.Mode.Chase;
		} else if (_dotsConsumed == _totalDots / 3) {
			GameObject.Find ("Clyde").GetComponent<GhostMove> ().state = GhostMove.Mode.Chase;
		}
		if (_dotsConsumed == _totalDots)
			GameOver (true);
		if (powerUp) {
			GameObject.Find ("Blinky").GetComponent<GhostMove>().ChangeToFrightened();
			GameObject.Find ("Pinky").GetComponent<GhostMove>().ChangeToFrightened();
			GameObject.Find ("Inky").GetComponent<GhostMove>().ChangeToFrightened();
			GameObject.Find ("Clyde").GetComponent<GhostMove>().ChangeToFrightened();
		}
	}

	public void GhostConsumed() {
		_score += 200;
		scoreLabel.text = _score.ToString();
	}

	public void Dead() {
		//animation and stuff TODO
		_lives--;
		InitGhosts ();
		GameObject.Find ("Player").GetComponent<Player> ().Reset ();
		livesLabel.text = _lives.ToString ();
		if (_lives < 0) {
			SceneManager.LoadScene ("_Scene_0");
		}
		StartCoroutine (StartCountdown ());
	}

	void GameOver(bool completed) {
		if (completed)
			print ("Game Over");
	}

	IEnumerator StartCountdown() {
		GameObject.Find ("Blinky").GetComponent<GhostMove>().WaitMode();
		GameObject.Find ("Pinky").GetComponent<GhostMove>().WaitMode();
		GameObject.Find ("Inky").GetComponent<GhostMove>().WaitMode();
		GameObject.Find ("Clyde").GetComponent<GhostMove>().WaitMode();
		GameObject.Find ("Player").GetComponent<Player> ().keys = false;
		GameObject countdownGO = GameObject.Find ("Countdown");
		TextMesh countdownText = countdownGO.GetComponent<TextMesh> ();
		countdownText.text = "3";
		yield return new WaitForSeconds (1);
		countdownText.text = "2";
		yield return new WaitForSeconds (1);
		countdownText.text = "1";
		yield return new WaitForSeconds (1);
		countdownText.text = "START";
		yield return new WaitForSeconds (1);
		countdownText.text = "";
		GameObject.Find ("Player").GetComponent<Player> ().keys = true;
		GameObject.Find ("Blinky").GetComponent<GhostMove> ().state = GhostMove.Mode.Chase;
		GameObject.Find ("Pinky").GetComponent<GhostMove> ().state = GhostMove.Mode.Chase;
		if (_dotsConsumed >= 30)
			GameObject.Find ("Inky").GetComponent<GhostMove> ().state = GhostMove.Mode.Chase;
		else
			GameObject.Find ("Inky").GetComponent<GhostMove> ().state = GhostMove.Mode.Caged;
		if (_dotsConsumed >= _totalDots / 3)
			GameObject.Find ("Clyde").GetComponent<GhostMove> ().state = GhostMove.Mode.Chase;
		else
			GameObject.Find ("Clyde").GetComponent<GhostMove> ().state = GhostMove.Mode.Caged;
	}
}
