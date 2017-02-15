using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TetrisGame))]
public class RowCollapseAnimation : MonoBehaviour 
{
	private TetrisBoard board;
	private GameObject[,] blocks;
	private int numCollapses = 0;

	public float DropSpeed = 10;
	// Divide the checks and movements per frame by this number to make 
	// faster drop speeds smoother and 
	// to stop the blocks from going to far and clipping
	public int DivisionsPerFrame = 4;

	// Use this for initialization
	void Awake() 
	{
		board = gameObject.GetComponent<TetrisBoard>();
	}

	void Start()
	{
		blocks = board.Blocks;
	}

	void OnEnable()
	{
		board.RowCollapsed += CollapseAnimation;
	}

	void OnDisable()
	{
		board.RowCollapsed -= CollapseAnimation;
	}

	void CollapseAnimation( object sender, Tetris.RowCollapseEventArgs args )
	{
		List<int> list = new List<int>( args.ClearedRows );
		list.Sort();
		list.Reverse();

		int offset = 0;
		foreach( int row in list )
		{
			moveAllAbove( row + offset, board.PrefabSize.y );

			IEnumerator c = moveOverTime( row + offset, board.PrefabSize.y );
			StartCoroutine( c );
			offset++;
		}
	}

	private IEnumerator moveOverTime( int rowNumber, float distance )
	{
		int division = 0;
		float location = 0;
		numCollapses++;
		while( location < distance )
		{
			float change = (DropSpeed/numCollapses) * Time.deltaTime / DivisionsPerFrame;
			moveAllAbove( rowNumber, -change );
			location += change;
			if( division == 0 ) yield return null;

			division = (division + 1) % DivisionsPerFrame;
		}
		moveAllAbove( rowNumber, location-distance );
		numCollapses--;
	}

	private void moveAllAbove( int rowNumber, float distance )
	{
		for( int i = rowNumber; i >= 0; i-- )
		{
			for( int j = 0; j < blocks.GetLength( 1 ); j++ )
			{
				Vector3 position = blocks[i, j].transform.position;
				position.y += distance;
				blocks[i, j].transform.position = position;
			}
		}
	}
}
