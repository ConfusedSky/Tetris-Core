using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TetrisGame))]
public class GarbageGenerator : MonoBehaviour {
	private TetrisBoard board;

	int nn = 0;

	// Use this for initialization
	void Start () {
		board = gameObject.GetComponent<TetrisBoard>();
		//board.BoardChanged += Board_BoardChanged;
	}

	void Board_BoardChanged (object sender, System.EventArgs e)
	{
		if (board.Controller.Lost)
			nn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Generates n lines of garbage
	/// </summary>
	/// <param name="n">Number of lines to generate.</param>
	public void GenerateGarbage(int n)
	{
		nn = n;
		for (int i = 0; i < nn && !board.Controller.Lost; i++)
			GenerateGarbage ();
	}

	public void GenerateGarbage()
	{
		board.Controller.ReverseCollapse (1);
		if (nn != 0) {
			int x = Random.Range (0, board.Controller.Width);
			int y = board.Controller.Height - 1;
			board.Controller.PlaceBlocks (
				Enumerable.Range (0, board.Controller.Width).Where ((int q) => q != x).Select ((int q) => new Tetris.Point (q, y)),
				Tetris.BlockColor.black);
		}
	}
}
