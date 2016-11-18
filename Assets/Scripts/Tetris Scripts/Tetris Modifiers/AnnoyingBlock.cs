using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisGame))]
public class AnnoyingBlock : MonoBehaviour
{
	public float FadeTime = 25;

	private TetrisGame game;

	// Use this for initialization
	void Awake() 
	{
		game = gameObject.GetComponent<TetrisGame>();
	}

	void Start()
	{
	}

	void OnEnable()
	{
		game.OnRowCollapse += SpawnBlock;
	}

	void OnDisable()
	{
		game.OnRowCollapse -= SpawnBlock;
	}

	void SpawnBlock( object sender, TetrisGame.RowCollapseEventArgs args )
	{
		int column;
		int i;
		foreach( int line in args.ClearedRows )
		{
			int tries = 0;
			bool blockPlaced = false;
			do
			{
				column = Random.Range( 0, game.Width );
				i = 1;
				for( i = 1; i < game.Height; i++ )
				{
					if( game.Scripts[i, column].Occupied )
						break;
				}
				game.Scripts[i - 1, column].BlockColor = Color.black;
				blockPlaced = true;
				if( game.CheckClear( i - 1 ) )
				{
					game.Scripts[i - 1, column].BlockColor = null;
					blockPlaced = false;
				}
				tries++;
			} while( !blockPlaced );

			IEnumerator fade = FadeIn( column, i - 1 );
			StartCoroutine( fade );
		}
	}

	private IEnumerator FadeIn( int x, int y )
	{
		TetrisBlockScript block = game.Scripts[y, x];

		for( int i = 0; i < 256; i += 8 )
		{
			block.BlockColor = new Color32( 0, 0, 0, (byte)i );
			yield return new WaitForSeconds( (FadeTime / 1000) / 32 );
		}
	}

}

