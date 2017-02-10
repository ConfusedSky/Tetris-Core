using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tetris;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGame : MonoBehaviour
{
	public Color ShadowColor;

	public InputManger InputManager;

	[Header("Queue Parameters")]
	[SerializeField]
	private int queueSize = 5;
	[SerializeField]
	private int queueLookback = 4;
	[SerializeField]
	private int queueTries = 4;

	private BaseTetrisBoard board;
	private Mino currentBlock;
	private MinoType heldBlock = null;
	private RandomItemGenerator<MinoType> queue;
		
	public MinoType HeldBlock{ get{ return heldBlock; } }
	public MinoType[] QueuedBlocks{ get{ return queue.GetObjects(); } }
	public BaseTetrisBoard Board{ get{ return board; } }

	public class RowCollapseEventArgs : System.EventArgs
	{
		public IList<int> ClearedRows{ get; private set; }

		public RowCollapseEventArgs( IList<int> clearedRows )
		{
			ClearedRows = clearedRows;
		}
	}

	public event System.EventHandler OnHold;
	public event System.EventHandler OnBlockDropped;
	public event System.EventHandler<RowCollapseEventArgs> OnRowCollapse;
	public event System.EventHandler OnStart;

	// Use this for initialization
	void Awake()
	{
		Mino.ShadowColor = ShadowColor.ToBlockColor();
		queue = new RandomItemGenerator<MinoType>( Tetromino.TETROMINO_TYPES, queueSize, queueLookback, queueTries );
	}

	void Start()
	{
		board = gameObject.GetComponent<TetrisBoard>().Controller;
		currentBlock = Mino.CreateNewMino( board, queue.GetNextItem() );
		GameStart();
	}

	void GameStart()
	{
		currentBlock.BlockType = queue.GetNextItem();
		if( OnStart != null ) OnStart( this, System.EventArgs.Empty );
	}

	void OnEnable()
	{
		board.BoardChanged += OnBoardChanged;
	}

	void OnDisable()
	{
		board.BoardChanged -= OnBoardChanged;
	}

	private void Hold()
	{
		MinoType temp = currentBlock.BlockType;
		if( heldBlock != null )
			currentBlock.BlockType = heldBlock;
		else
			currentBlock.BlockType = queue.GetNextItem();
		heldBlock = temp;

		if( OnHold != null ) OnHold( this, System.EventArgs.Empty );
	}


	void OnBoardChanged (object sender, System.EventArgs e)
	{
		IList<int> clears = board.CheckClears();
		// if the previous frame had a drop that cleared resolve the clear
		foreach( int i in clears )
		{
			board.CollapseRow( i );
		}
		if( clears.Count > 0 && OnRowCollapse != null ) OnRowCollapse( this, new RowCollapseEventArgs( clears ) );

		// Check for loss conditions if loss reset the board
		if( board.Lost )
		{
			Reset();
			return;
		}
	}

	// Update is called once per frame
	void Update()
	{
		TetrisAction action = InputManager.HandleInput();

		if( action == TetrisAction.Hold ) Hold();

		if (currentBlock == null)
			return;

		if( currentBlock.Alive )
			currentBlock = currentBlock.Update( action );
		else
		{
			MinoType nextTetronimo = queue.GetNextItem();
			if( OnBlockDropped != null ) OnBlockDropped( this, System.EventArgs.Empty );
			currentBlock.BlockType = nextTetronimo;
		}
	}

	public void Reset()
	{
		board.Reset();
		heldBlock = null;
		queue.Reset();

		GameStart();
	}
}
