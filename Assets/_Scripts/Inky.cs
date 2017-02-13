using UnityEngine;
using System.Collections;

public class Inky : GhostMove {

	GameObject _playerGO;
	GameObject _blinkyGO;

	// Use this for initialization
	void OnEnable () {
		this.transform.position = GameField.CellToWorld (new Tile (9,7));
		_playerGO = GameObject.Find ("Player");
		_blinkyGO = GameObject.Find ("Blinky");
		base.Init (); //tile should be 24,15
	}
	
	public override Tile Chase () {
		Vector3 playerPos = _playerGO.transform.position;
		Vector3 playerDir = _playerGO.GetComponent<Player> ().direction;
		Vector3 twoTilesAhead = playerPos + 2 * playerDir;
		Vector3 dir = twoTilesAhead - _blinkyGO.transform.position;
		return GameField.WorldToCell (dir * 2);
	}
}
