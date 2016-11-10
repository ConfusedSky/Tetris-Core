using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGame : MonoBehaviour
{
	[Header("Colors:")]
	public Color ShadowColor;

	[Header("Input Manager:")]
	public InputManger InputManager;

	private TetrisBoard board;
	private GameObject[,] blocks;
	private TetrisBlockScript[,] blockScripts;
	private Tetronimo currentBlock;
	private TetronimoType? heldBlock = null;
	private TetronimoQueue queue;

	public bool Lost
	{
		get
		{
			for( int i = 0; i < Width; i++ )
			{
				if( blockScripts[0, i].Occupied )
					return true;
			}
			return false;
		}
	}
		
	public TetronimoType? HeldBlock{ get{ return heldBlock; } }
	public TetronimoType[] QueuedBlocks{ get{ return queue.GetTypes(); } }
	public GameObject[,] Blocks{ get{ return blocks; } }
	public TetrisBlockScript[,] Scripts{ get{ return blockScripts; } }

	// These properties only exist for backwards compatibility
	public int Width{ get{ return board.Width; } }
	public int Height{ get { return board.Height; } }
	public Vector3 StartingLocation{ get{ return board.StartingLocation; } }
	public Vector2 PrefabSize{ get{ return board.PrefabSize; } }

	public Color BGColor1{ get{ return board.BGColor1; } }
	public Color BGColor2{ get{ return board.BGColor2; } }

	public GameObject BlockPrefab{ get{ return board.BlockPrefab; } }

	public class RowCollapseEventArgs : System.EventArgs
	{
		public int RowNumber{ get; private set; }

		public RowCollapseEventArgs( int row )
		{
			RowNumber = row;
		}
	}

	public event System.EventHandler OnHold;
	public event System.EventHandler OnBlockDropped;
	public event System.EventHandler<RowCollapseEventArgs> OnRowCollapse;

	// Use this for initialization
	void Awake()
	{
		board = gameObject.GetComponent<TetrisBoard>();
		blocks = new GameObject[Height, Width];
		blockScripts = new TetrisBlockScript[Height, Width];

		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				blocks[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					gameObject.transform
				);

				blockScripts[i, j] = blocks[i, j].GetComponent<TetrisBlockScript>();
				blockScripts[i, j].DefaultColor = (((j + i % 2) % 2 == 0) ? (BGColor1) : (BGColor2));
			}
		}

		Tetronimo.ShadowColor = ShadowColor;
		UpdateBlocks();
		queue = new TetronimoQueue();
		currentBlock = Tetronimo.CreateNewTetronimo( blockScripts, queue.GetNextBlock() );
	}

	private void Hold()
	{
		currentBlock.Clear();
		TetronimoType? temp = (currentBlock != null) ? (TetronimoType?)currentBlock.BlockType : null;
		if( heldBlock != null )
			currentBlock = Tetronimo.CreateNewTetronimo( blockScripts, heldBlock.GetValueOrDefault() );
		else
			currentBlock = null;
		heldBlock = temp;

		if( OnHold != null ) OnHold( this, System.EventArgs.Empty );
	}

	// Update is called once per frame
	void Update()
	{
		TetrisAction action = InputManager.HandleInput();

		if( action == TetrisAction.Hold ) Hold();

		if( currentBlock == null )
		{
			// if the previous frame had a drop that cleared resolve the clear
			foreach( int i in CheckClears() )
			{
				CollapseRow( i );
			}

			// Check for loss conditions if loss reset the board
			if( Lost )
			{
				Reset();
			}
				
			currentBlock = Tetronimo.CreateNewTetronimo( blockScripts, queue.GetNextBlock() );

			if( OnBlockDropped != null ) OnBlockDropped( this, System.EventArgs.Empty );
		}

		if( currentBlock != null ) currentBlock = currentBlock.Update( action );
	}

	public void Reset()
	{
		foreach( TetrisBlockScript b in blockScripts )
		{
			b.Clear();
		}

		heldBlock = null;
		OnHold( this, System.EventArgs.Empty );

		queue.Reset();
	}

	public void CollapseRow( int row )
	{
		for( int i = 0; i < Width; i++ )
		{
			blockScripts[row, i].Clear();
		}

		for( int i = row; i >= 1; i-- )
		{
			for( int j = 0; j < Width; j++ )
			{
				blockScripts[i - 1, j].MoveTo( blockScripts[i, j] );
			}
		}

		if( OnRowCollapse != null ) OnRowCollapse( this, new RowCollapseEventArgs( row ) );
	}

	public IList CheckClears()
	{
		ArrayList result = new ArrayList();

		for( int i = 0; i < Height; i++ )
		{
			bool clearable = true;

			for( int j = 0; j < Width; j++ )
			{
				if( !blockScripts[i, j].Occupied )
					clearable = false;
			}

			if( clearable )
				result.Add( i );
		}

		return result;
	}

	private void UpdateBlocks()
	{
		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				Vector3 position = new Vector3(
					                   StartingLocation.x + PrefabSize.x * j,
					                   StartingLocation.y - PrefabSize.y * i,
					                   StartingLocation.z
				                   );

				blocks[i, j].transform.localPosition = position;
			}
		}
	}
		
}
