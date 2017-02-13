using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameField : MonoBehaviour {

	public static Tile startPos = new Tile(13,8);
	public static Tile cageEntrance = new Tile (7, 8);
	public static Tile barrier = new Tile (8, 8);
	public static Vector3 leftTp = new Vector3 (-8, 12, 0);
	public static Vector3 rightTp = new Vector3 (8, 12, 0);

	public static Tile powerUpLU = new Tile (2, 0);
	public static Tile powerUpLD = new Tile (15, 0);
	public static Tile powerUpRU = new Tile (2, 16);
	public static Tile powerUpRD = new Tile (15, 16);

	private static List<string> _allowedInCage = new List<string>();

	public static int[,] grid = new int[,]{
	 //  0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 
		{1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1}, //0
		{1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1}, //1
		{1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1}, //2
		{1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1}, //3
		{1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1}, //4
		{1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1}, //5
		{0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0}, //6
		{1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1}, //7
		{1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1}, //8
		{1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1}, //9
		{1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1}, //10
		{1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1}, //11
		{0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0}, //12
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, //13
		{1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1}, //14
		{1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1}, //15
		{0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0}, //16
		{1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1}, //17
		{1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1}, //18
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}  //19
	};

	public static int[,] intersections = new int[,]{
	 //  0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 
		{0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0}, //0
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //1
		{0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0}, //2
		{1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1}, //3
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //4
		{0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0}, //5
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //6
		{0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0}, //7
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //8
		{1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1}, //9
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //10
		{0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0}, //11
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //12
		{0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0}, //13
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //14
		{0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0}, //15
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //16
		{0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0}, //17
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //18
		{0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0}  //19
	};



	public static Vector3 CellToWorld(Tile t) {
		return new Vector3 (t.j - 8, 21 - t.i, 0);
	}

	public static Tile WorldToCell(Vector3 pos) {
		return new Tile (21 - (int)pos.y, 8 + (int)pos.x);
	}

	public static float distance(Tile t0, Tile t1) {
		return Mathf.Sqrt (Mathf.Pow(t1.j - t0.j, 2) + Mathf.Pow(t1.i - t0.i, 2));
	}

	public static bool Valid(Tile pos, Vector3 dir, string tag) {
		if (pos.i == cageEntrance.i && pos.j == cageEntrance.j && dir.Equals (Vector3.down)
		    && _allowedInCage.Contains (tag))
			return true;
		else if (pos.i == barrier.i && pos.j == barrier.j && dir.Equals (Vector3.down)
		         && _allowedInCage.Contains (tag)) 
			return true;
		else if (pos.i == 9 && pos.j == 0 && dir.Equals (Vector3.left))
			return true;
		else if (pos.i == 9 && pos.j == 16 && dir.Equals (Vector3.right))
			return true;
		else if (pos.i - 1 >= 0 && dir.Equals (Vector3.up))
			return grid [pos.i - 1, pos.j] == 1;
		else if (pos.i + 1 < grid.GetLength(0) && dir.Equals (Vector3.down))
			return grid [pos.i + 1, pos.j] == 1;
		else if (pos.j - 1 >= 0 && dir.Equals (Vector3.left))
			return grid [pos.i, pos.j - 1] == 1;
		else if (pos.j + 1 < grid.GetLength(1) && dir.Equals (Vector3.right))
			return grid [pos.i, pos.j + 1] == 1;
		else
			return false;
	}

	public static bool IsPowerUp(Tile t) {
		if (t.i == powerUpLD.i && t.j == powerUpLD.j)
			return true;
		else if (t.i == powerUpLU.i && t.j == powerUpLU.j)
			return true;
		else if (t.i == powerUpRD.i && t.j == powerUpRD.j)
			return true;
		else if (t.i == powerUpRU.i && t.j == powerUpRU.j)
			return true;
		else
			return false;
	}

	public static void EnableBarrier(string ghost) {
		_allowedInCage.Add (ghost);
		intersections [7, 8] = 1;
	}

	public static void DisableBarrier(string ghost) {
		_allowedInCage.Remove (ghost);
		if (_allowedInCage.Count == 0)
			intersections [7, 8] = 0;
	}
}
