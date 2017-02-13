using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tile {

	[SerializeField]
	private int _i;
	[SerializeField]
	private int _j;

	public Tile(int i, int j) {
		this._i = i;
		this._j = j;
	}

	public int i {
		get {
			return _i;
		}
		set {
			_i = value;
		}
	}

	public int j {
		get {
			return _j;
		}
		set {
			_j = value;
		}
	}

	public override string ToString ()
	{
		return string.Format ("[Tile: i={0}, j={1}]", i, j);
	}
}
