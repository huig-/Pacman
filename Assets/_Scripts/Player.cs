using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed =1.0f;

	public bool ______________;

	private Vector3 _pos;
	private Vector3 _direction;
	private Vector3 _nextDir;
	private Vector3 _initialPos;
	private bool _keys;

	// Use this for initialization
	void Start () {
		keys = true;
		_pos = transform.position;
		_initialPos = transform.position;
		_direction = Vector3.zero;
		_nextDir = Vector3.zero; 
	}

	public Vector3 initialPos {
		get { return _initialPos; }
	}

	public bool keys {
		set;
		get;
	}

	void FixedUpdate () {

		if (!keys)
			return;

		if (Input.GetKey (KeyCode.UpArrow)) {
			_nextDir = Vector3.up;
		}
		else if (Input.GetKey (KeyCode.DownArrow)) {
			_nextDir = Vector3.down;
		}
		else if (Input.GetKey (KeyCode.LeftArrow)) {
			_nextDir = Vector3.left;
		}
		else if (Input.GetKey (KeyCode.RightArrow)) {
			_nextDir = Vector3.right;
		}

		if (transform.position == _pos && _nextDir != Vector3.zero && Valid (_nextDir)) {
			Rotate ();
			_direction = _nextDir;
			_pos += _direction;
		} else if (transform.position == _pos && _direction != Vector3.zero && Valid(_direction)) {
			_pos += _direction;
		}

		transform.position = Vector3.MoveTowards (transform.position, _pos, Time.deltaTime * speed);
	}

	void Rotate() {
		if (_nextDir.Equals (Vector3.right)) {
			gameObject.transform.rotation = Quaternion.AngleAxis (180, Vector3.up);
		} else if (_nextDir.Equals (Vector3.left)) {
			gameObject.transform.rotation = Quaternion.AngleAxis (0, Vector3.up);
		} else if (_nextDir.Equals (Vector3.down)) {
				gameObject.transform.rotation = Quaternion.AngleAxis (90, new Vector3 (0, 0, 1));
		}
		else if (_nextDir.Equals (Vector3.up)) {
			gameObject.transform.rotation = Quaternion.AngleAxis(180, Vector3.up) 
				* Quaternion.AngleAxis (270, new Vector3 (0, 0, 1));
		}
	}

	bool Valid(Vector3 dir) {
		Vector3 pos = transform.position;
		dir += new Vector3 (dir.x * 0.45f, dir.y * 0.45f, 0);
		RaycastHit hit;
		Physics.Linecast (pos, pos + dir, out hit);
		if (hit.collider) {
			return hit.collider.tag != "Maze";
		} else
			return true;
	}

	public Vector3 direction {
		get{
			return _direction;
		}
	}

	public void TpLeft() {
		_pos = new Vector3 (7,12,0);
	}

	public void TpRight() {
		_pos = new Vector3 (-7, 12, 0);
	}

	public void Reset() {
		_direction = Vector3.zero;
		_nextDir = Vector3.zero;
		_pos = _initialPos;
		transform.position = _initialPos;
		gameObject.transform.rotation = Quaternion.AngleAxis (0, Vector3.up);
	}
}
