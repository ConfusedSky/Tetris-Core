using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisGame))]
public class AnnoyingBlock : MonoBehaviour
{
	public float FadeTime = 100;
	public float TimePeriod = 1f;

	public bool ActivateOnCollapse = false;
	public bool ActivateOnDrop = false;
	public bool ActivateOnTimePeriod = false;
	public bool AvoidClears = true;

	public int BlocksPerCollapse = 1;
	public int BlocksPerDrop = 1;
	public int BlocksPerTimePeriod = 1;

	private TetrisGame game;
	private float timeTillDrop = 0;

	// Use this for initialization
	void Awake() 
	{
		game = gameObject.GetComponent<TetrisGame>();
	}

	void Start()
	{
		timeTillDrop = TimePeriod;
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

	void Update()
	{
		if( ActivateOnTimePeriod )
		{
			timeTillDrop -= Time.deltaTime;

			if( timeTillDrop < 0 )
			{
				timeTillDrop += TimePeriod;
				PlaceBlocks( BlocksPerTimePeriod );
			}
		}
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
			if( AvoidClears && game.CheckClear( i - 1 ) )
			{
				game.Scripts[i - 1, column].BlockColor = null;
				blockPlaced = false;
			}
		} while( !blockPlaced );

		game.Board.PlaceBlock( new Point( column, i-1 ), Color.black );

		// IEnumerator fade = FadeIn( column, i - 1 );
		// StartCoroutine( fade );
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

