using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostMove : MonoBehaviour {

	public enum Mode {Scatter, Chase, Frightened, Dead, Caged};

	public float speed = 1.0f;
	public Tile corner;
	public float tScatter = 1.0f;
	public float tChase = 25.0f;
	public float tFrightened = 10.0f;
	public static GameController gController; 

	public bool _________________;

	private Mode _state;
	private Vector3 _direction;
	private Tile _pos;
	private Tile _dest;
	private float _time;
	private Tile _initialPos;
	private float _frightenedSpeed;
	private float _speed;
	protected Color _color;

	void Awake() {
		Renderer[] ex = gameObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in ex) {
			if (r.name == "<STL_BINARY>")
				_color = r.material.color;
		}	
	}

	// Use this for initialization
	void Start () {
		if (gController == null) {
			gController = GameObject.Find ("Main Camera").GetComponent<GameController> ();
		}
	}

	public void WaitMode() {
		speed = 0;
		color = _color;
	}

	public Tile initialPos {
		get { return _initialPos; }
	}

	public Color color {
		get {
			return _color;
		}
		set {
			Renderer[] ex = gameObject.GetComponentsInChildren<Renderer> ();
			foreach (Renderer r in ex) {
				if (r.name == "<STL_BINARY>")
					r.material.color = value;
			}
		}
	}

	public void Init() {
		_pos = GameField.WorldToCell(this.transform.position);
		_initialPos = _pos;
		_state = Mode.Caged;
		//_dest = Scatter ();
		_direction = FindNextDirection ();
		_time = Time.time;
		_speed = speed;
		_frightenedSpeed = _speed / 2;
	}

	public Mode state {
		get {
			return _state;
		}
		set {
			_state = value;
			_time = Time.time;
			switch (_state) {
			case Mode.Chase:
				speed = _speed;
				_dest = Chase ();
				break;
			case Mode.Scatter:
				_dest = Scatter ();
				break;
			case Mode.Dead:
				_state = Mode.Dead;
				speed = _speed;
				color = Color.black;
				GameField.EnableBarrier (gameObject.tag);
				break;
			}
		}
	}
	
	void FixedUpdate () {
		if (transform.position == GameField.CellToWorld(_pos)) {
			bool stateChanged = false;
			if (_state == Mode.Scatter && Time.time - _time >= tScatter) {
				_state = Mode.Chase;
				_time = Time.time;
				stateChanged = true;
			} else if (_state == Mode.Chase && Time.time - _time >= tChase) {
				_state = Mode.Scatter;
				_time = Time.time;
				stateChanged = true;
			} else if (_state == Mode.Frightened && Time.time - _time >= tFrightened) {
				_state = Mode.Chase;
				_time = Time.time;
				EndFrightened ();
			} else if (_state == Mode.Dead && GameField.CellToWorld (Dead ()) == (transform.position)) { //equals not working
				_state = Mode.Chase;
				_time = Time.time;
				EndFrightened ();
				GameField.DisableBarrier (gameObject.tag);
			} else if (_state == Mode.Caged) {
				_state = Mode.Caged;
				_time = Time.time;
				//GameField.DisableBarrier (gameObject.tag);
			}
			if (GameField.intersections [_pos.i, _pos.j] == 1) {
				switch (_state) {
				case Mode.Chase:
					_dest = Chase ();
					break;
				case Mode.Scatter:
					_dest = Scatter ();
					break;
				case Mode.Frightened:
					_dest = Frightened ();
					break;
				case Mode.Dead:
					_dest = Dead ();
					break;
				}
				if (stateChanged)
					_direction = ReverseDirection ();
				else
					_direction = FindNextDirection ();
			} else {
				if (stateChanged)
					_direction = ReverseDirection ();
				else if (!GameField.Valid (_pos, _direction, gameObject.tag)) {
					_direction = FindNextDirection ();
				}
			}
			Vector3 posWorld = GameField.CellToWorld (_pos);
			posWorld += _direction;
			_pos = GameField.WorldToCell (posWorld);
		}

		transform.position = Vector3.MoveTowards (transform.position, GameField.CellToWorld(_pos), Time.deltaTime * speed);
	}

	Vector3 FindNextDirection() {
		if (_state == Mode.Caged)
			return Vector3.zero;
		float[] distances = new float[4]; //0 up, 1 left, 2 down, 3 right: priority
		if (_direction.Equals (Vector3.down) || !GameField.Valid (_pos, Vector3.up, gameObject.tag))
			distances [0] = -1;
		if (_direction.Equals (Vector3.right) || !GameField.Valid (_pos, Vector3.left, gameObject.tag))
			distances[1] = -1;
		if (_direction.Equals (Vector3.up) || !GameField.Valid (_pos, Vector3.down, gameObject.tag))
			distances[2] = -1;
		if (_direction.Equals (Vector3.left) || !GameField.Valid (_pos, Vector3.right, gameObject.tag))
			distances [3] = -1;

		if (distances [0] != -1) 
			distances [0] = GameField.distance (NextTile(_pos, Vector3.up), _dest);
		if (distances [1] != -1)
			distances [1] = GameField.distance (NextTile(_pos, Vector3.left), _dest);
		if (distances [2] != -1)
			distances [2] = GameField.distance (NextTile(_pos, Vector3.down), _dest);
		if (distances [3] != -1)
			distances [3] = GameField.distance (NextTile(_pos, Vector3.right), _dest);

		for (int i = 0; i < 4; i++) {
			if (distances [i] == -1)
				distances [i] = Mathf.Infinity;
		}
	
		int min = 0;
		for (int i = 1; i < 4; i++) {
			if (distances [i] < distances [min])
				min = i;
		}

		if (min == 0)
			return Vector3.up;
		else if (min == 1)
			return Vector3.left;
		else if (min == 2)
			return Vector3.down;
		else
			return Vector3.right;
	}

	Tile NextTile(Tile current, Vector3 dir) {
		if (current.i == 9 && current.j == 0 && dir.Equals (Vector3.left))
			return new Tile (9, 16); //leftTp to rightTp
		else if (current.i == 9 && current.j == 16 && dir.Equals (Vector3.right))
			return new Tile (9, 0); //rightTp to leftTp
		else if (dir.Equals (Vector3.left))
			return new Tile (current.i, current.j - 1);
		else if (dir.Equals (Vector3.right))
			return new Tile (current.i, current.j + 1);
		else if (dir.Equals (Vector3.up))
			return new Tile (current.i - 1, current.j);
		else
			return new Tile (current.i + 1, current.j);
	}

	Vector3 ReverseDirection() {
		if (_direction.Equals (Vector3.up))
			return Vector3.down;
		else if (_direction.Equals (Vector3.down))
			return Vector3.up;
		else if (_direction.Equals (Vector3.left))
			return Vector3.right;
		else 
			return Vector3.left;
	}

	public virtual Tile Scatter() {
		return corner;
	}

	public virtual Tile Chase() {
		return corner;
	}

	public virtual Tile Frightened() {
		int i = GameField.grid.GetLength (0);
		int j = GameField.grid.GetLength (1);
		return new Tile (Random.Range (0, i), Random.Range (0, j));
	}

	public virtual Tile Dead() {
		return new Tile (9, 8); //cage center
	}

	void OnTriggerEnter(Collider other) {
		switch (_state) {
		case Mode.Frightened:
			if (other.tag == "Player") {
				gController.GhostConsumed (); //show animation
				state = Mode.Dead;
				//ChangeToDead();
			}
			break;
		case Mode.Chase:
		case Mode.Scatter:
			if (other.tag == "Player")
				gController.Dead();
			break;
		}
	}

	public void TpLeft() {
		_pos = GameField.WorldToCell(new Vector3 (7,12,0));
	}

	public void TpRight() {
		_pos = GameField.WorldToCell(new Vector3 (-7, 12, 0));
	}

	public void Reset() {
		_direction = Vector3.zero;
		_pos = _initialPos;
		transform.position = GameField.CellToWorld(_initialPos);
	}

	public void ChangeToDead() {
		_state = Mode.Dead;
		speed = _speed;
		GetComponent<Renderer> ().material.color = Color.black;
		GameField.EnableBarrier (gameObject.tag);
	}

	public void ChangeToFrightened() {
		if (_state != Mode.Caged && _state != Mode.Dead) {
			_state = Mode.Frightened;
			speed = _frightenedSpeed;
			color = Color.blue;
		}
		_time = Time.time;
	}

	public void EndFrightened() {
		speed = _speed;
		color = _color;
	}
}
