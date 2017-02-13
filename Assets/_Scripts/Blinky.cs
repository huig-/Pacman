using UnityEngine;
using System.Collections;

//acts weird when player is on topcross

public class Blinky : GhostMove {

	void Awake() {
		this.transform.position = GameField.CellToWorld (new Tile (7, 8));
		base.Init (); //tile should be (-3,15)
		Renderer[] ex = gameObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in ex) {
			if (r.name == "<STL_BINARY>")
				_color = r.material.color;
		}	
		state = Mode.Chase;
	}

	public override Tile Chase() {
		return GameField.WorldToCell(GameObject.Find ("Player").transform.position);
	}
}
