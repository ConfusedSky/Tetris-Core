using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisGame))]
public class RowCollapseAnimation : MonoBehaviour 
{
	private TetrisGame game;
	private GameObject[,] blocks;
	private int numCollapses = 0;

	public float DropSpeed = 10;

	// Use this for initialization
	void Awake() 
	{
		game = gameObject.GetComponent<TetrisGame>();
	}

	void Start()
	{
		blocks = game.Blocks;
	}

	void OnEnable()
	{
		game.OnRowCollapse += CollapseAnimation;
	}

	void OnDisable()
	{
		game.OnRowCollapse -= CollapseAnimation;
	}

	void CollapseAnimation( object sender, TetrisGame.RowCollapseEventArgs args )
	{
		ArrayList list = new ArrayList( args.ClearedRows );
		list.Sort();
		list.Reverse();

		int offset = 0;
		foreach( int row in list )
		{
			moveAllAbove( row + offset, game.PrefabSize.y );

			IEnumerator c = moveOverTime( row + offset, game.PrefabSize.y );
			StartCoroutine( c );
			offset++;
		}
	}

	private IEnumerator moveOverTime( int rowNumber, float distance )
	{
		float location = 0;
		numCollapses++;
		while( location < distance )
		{
			float change = (DropSpeed/numCollapses) * Time.deltaTime;
			moveAllAbove( rowNumber, -change );
			location += change;
			yield return null;
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
