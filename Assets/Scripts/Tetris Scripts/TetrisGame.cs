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
				if( Scripts[0, i].Occupied )
					return true;
			}
			return false;
		}
	}
		
	public TetronimoType? HeldBlock{ get{ return heldBlock; } }
	public TetronimoType[] QueuedBlocks{ get{ return queue.GetTypes(); } }
	public GameObject[,] Blocks{ get{ return board.Blocks; } }
	public TetrisBlockScript[,] Scripts{ get{ return board.Scripts; } }

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
		Tetronimo.ShadowColor = ShadowColor;
		queue = new TetronimoQueue();
	}

	void Start()
	{
		currentBlock = Tetronimo.CreateNewTetronimo( Scripts, queue.GetNextBlock() );
	}

	private void Hold()
	{
		currentBlock.Clear();
		TetronimoType? temp = (currentBlock != null) ? (TetronimoType?)currentBlock.BlockType : null;
		if( heldBlock != null )
			currentBlock = Tetronimo.CreateNewTetronimo( Scripts, heldBlock.GetValueOrDefault() );
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
				
			currentBlock = Tetronimo.CreateNewTetronimo( Scripts, queue.GetNextBlock() );

			if( OnBlockDropped != null ) OnBlockDropped( this, System.EventArgs.Empty );
		}

		if( currentBlock != null ) currentBlock = currentBlock.Update( action );
	}

	public void Reset()
	{
		board.Reset();

		heldBlock = null;
		OnHold( this, System.EventArgs.Empty );

		queue.Reset();
	}

	public void CollapseRow( int row )
	{
		for( int i = 0; i < Width; i++ )
		{
			Scripts[row, i].Clear();
		}

		for( int i = row; i >= 1; i-- )
		{
			for( int j = 0; j < Width; j++ )
			{
				Scripts[i - 1, j].MoveTo( Scripts[i, j] );
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
				if( !Scripts[i, j].Occupied )
					clearable = false;
			}

			if( clearable )
				result.Add( i );
		}

		return result;
	}
		
}
