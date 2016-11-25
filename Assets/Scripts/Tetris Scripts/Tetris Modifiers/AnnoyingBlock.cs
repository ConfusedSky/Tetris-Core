using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisGame))]
public class AnnoyingBlock : MonoBehaviour
{
	public float FadeTime = 100;

	public bool ActivateOnCollapse = true;
	public bool ActivateOnDrop = false;

	public int BlocksPerDrop = 1;
	public int BlocksPerCollapse = 1;

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
		game.OnRowCollapse += OnCollaspse;
		game.OnBlockDropped += OnBlockDropped;
	}

	void OnDisable()
	{
		game.OnRowCollapse -= OnCollaspse;
		game.OnBlockDropped -= OnBlockDropped;
	}

	void OnBlockDropped (object sender, System.EventArgs e)
	{
		if( ActivateOnDrop )
			PlaceBlocks( BlocksPerDrop );
	}

	void OnCollaspse( object sender, TetrisGame.RowCollapseEventArgs args )
	{
		if( ActivateOnCollapse )
			foreach( int line in args.ClearedRows )
			{
				PlaceBlocks( BlocksPerCollapse );
			}
	}

	private void PlaceBlocks( int n )
	{
		for( int i = 0; i < n; i++ )
		{
			PlaceBlock();
		}
	}

	private void PlaceBlock()
	{
		int column;
		int i;
		int tries = 0;
		bool blockPlaced = false;
		do
		{
			column = Random.Range( 0, game.Width );
			for( i = 1; i < game.Height; i++ )
			{
				if( game.Scripts[i, column].Occupied )
					break;
			}
			game.Scripts[i-1, column].BlockColor = Color.black;
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

	private IEnumerator FadeIn( int x, int y )
	{
		TetrisBlockScript block = game.Scripts[y, x];

		for( float i = 0; block.Occupied && i <= 1; i += .1f )
		{
			block.BlockColor = new Color( 0, 0, 0, i );
			yield return new WaitForSeconds( (FadeTime / 1000) / 10 );
		}
	}

}

