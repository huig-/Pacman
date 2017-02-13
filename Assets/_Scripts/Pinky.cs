using UnityEngine;
using System.Collections;

public class Pinky : GhostMove {

	GameObject _playerGO;

	// Use this for initialization
	void Start () {
		this.transform.position = GameField.CellToWorld (new Tile (9,8));
		_playerGO = GameObject.Find ("Player");
		base.Init (); //tile should be (-3,-3)
		state = Mode.Chase;
	}
	
	public override Tile Chase() {
		Vector3 pos = _playerGO.transform.position;
		Vector3 dir = _playerGO.GetComponent<Player>().direction;
		return GameField.WorldToCell (pos + 4 * dir);
	}
}
