using UnityEngine;
using System.Collections;
using Tetris;

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
	private TetrisBoard board;
	private float timeTillDrop = 0;

	// Use this for initialization
	void Awake() 
	{
		game = gameObject.GetComponent<TetrisGame>();
		board = gameObject.GetComponent<TetrisBoard>();
	}

	void Start()
	{
		timeTillDrop = TimePeriod;
	}

	void OnEnable()
	{
		board.RowCollapsed += OnCollaspse;
		game.BlockDropped += OnBlockDropped;
	}

	void OnDisable()
	{
		board.RowCollapsed -= OnCollaspse;
		game.BlockDropped -= OnBlockDropped;
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

	void OnCollaspse( object sender, RowCollapseEventArgs args )
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
		bool blockPlaced = false;
		Point pos = new Point();
		do
		{
			pos.x = Random.Range( 0, game.BoardController.Width );
			for( pos.y = 1; pos.y < game.BoardController.Height; pos.y++ )
			{
				if( board.Controller[pos].Occupied )
					break;
		 	}
			pos.y--;
			board.Controller[pos].Color = BlockColor.black;
		 	blockPlaced = true;
			if( AvoidClears && board.Controller.CheckClear( pos.y ) )
		 	{
				board.Controller[pos].Color = null;
		 		blockPlaced = false;
		 	}
		 } while( !blockPlaced );

		board.Controller.PlaceBlock( pos, BlockColor.black );

		//IEnumerator fade = FadeIn( column, i - 1 );
		//StartCoroutine( fade );
	}

	//private IEnumerator FadeIn( int x, int y )
	//{
	//	TetrisBlockScript block = game.Scripts[y, x];

	//	for( float i = 0; block.Occupied && i <= 1; i += .1f )
	//	{
	//		block.BlockColor = new Color( 0, 0, 0, i );
	//		yield return new WaitForSeconds( (FadeTime / 1000) / 10 );
	//	}
	//}

}

