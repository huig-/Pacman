using UnityEngine;
using System.Collections;

public class Clyde : GhostMove {

	GameObject _playerGO;

	// Use this for initialization
	void OnEnable () {
		this.transform.position = GameField.CellToWorld (new Tile (9,9));
		_playerGO = GameObject.Find ("Player");
		base.Init (); //tile should be 24,-3
	}
	
	public override Tile Chase () {
		if (GameField.distance (GameField.WorldToCell(this.transform.position), 
			GameField.WorldToCell(_playerGO.transform.position)) >= 8) {
			return GameField.WorldToCell (_playerGO.transform.position);
		} else {
			return corner;
		}
	}
}
