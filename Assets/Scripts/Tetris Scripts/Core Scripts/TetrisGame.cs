using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGame : MonoBehaviour
{
	public Color ShadowColor;

	public InputManger InputManager;

	[Header("Queue Parameters")]
	[SerializeField]
	private MinoType[] minosToSpawn = Mino.TETROMINO_TYPES;
	[SerializeField]
	private int queueSize = 5;
	[SerializeField]
	private int queueLookback = 4;
	[SerializeField]
	private int queueTries = 4;

	private TetrisBoard board;
	private GameObject[,] blocks;
	private TetrisBlockScript[,] blockScripts;
	private Mino currentBlock;
	private MinoType heldBlock = null;
	private RandomItemGenerator<MinoType> queue;

	public bool Lost
	{
		get
		{
			for( int i = 0; i < Width; i++ )
			{
				if( Scripts[1, i].Occupied )
					return true;
			}
			return false;
		}
	}
		
	public MinoType HeldBlock{ get{ return heldBlock; } }
	public MinoType[] QueuedBlocks{ get{ return queue.GetObjects(); } }
	public TetrisBoard Board{ get{ return board; } }
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
		public IList ClearedRows{ get; private set; }

		public RowCollapseEventArgs( IList clearedRows )
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
		board = gameObject.GetComponent<TetrisBoard>();
		Mino.ShadowColor = ShadowColor;
		queue = new RandomItemGenerator<MinoType>( minosToSpawn, queueSize, queueLookback, queueTries );
	}

	void Start()
	{
		GameStart();
	}

	void GameStart()
	{
		currentBlock = Mino.CreateNewMino( board, queue.GetNextItem() );
		if( OnStart != null ) OnStart( this, System.EventArgs.Empty );
	}

	void OnEnable()
	{
		board.OnBoardChanged += OnBoardChanged;
	}

	void OnDisable()
	{
		board.OnBoardChanged -= OnBoardChanged;
	}

	private void Hold()
	{
		// TODO: find a better solution for the multiple shadows problem
		currentBlock.Destroy();
		MinoType temp = (currentBlock != null) ? currentBlock.BlockType : null;
		if( heldBlock != null )
			currentBlock = Mino.CreateNewMino( board, heldBlock );
		else
			currentBlock = Mino.CreateNewMino( board, queue.GetNextItem() );
		heldBlock = temp;

		if( OnHold != null ) OnHold( this, System.EventArgs.Empty );
	}


	void OnBoardChanged (object sender, System.EventArgs e)
	{
		IList clears = CheckClears();
		// if the previous frame had a drop that cleared resolve the clear
		foreach( int i in clears )
		{
			CollapseRow( i );
		}
		if( clears.Count > 0 && OnRowCollapse != null ) OnRowCollapse( this, new RowCollapseEventArgs( clears ) );

		// Check for loss conditions if loss reset the board
		if( Lost )
		{
			Reset();
			return;
		}
	}

	// Update is called once per frame
	void Update()
	{
		TetrisAction action = InputManager.HandleInput();

		if( action == TetrisAction.Hold && currentBlock != null ) Hold();

		if( currentBlock == null )
		{
			MinoType nextTetronimo = queue.GetNextItem();
			if( OnBlockDropped != null ) OnBlockDropped( this, System.EventArgs.Empty );
			currentBlock = Mino.CreateNewMino( board, nextTetronimo );
		}

		if( currentBlock != null ) currentBlock = currentBlock.Update( action );
	}

	public void Reset()
	{
		board.Reset();
		heldBlock = null;
		if( currentBlock != null ) currentBlock.Destroy();
		currentBlock = null;
		queue.Reset();

		GameStart();
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
	}

	public IList CheckClears()
	{
		ArrayList result = new ArrayList();

		for( int i = 0; i < Height; i++ )
		{
			if( CheckClear( i ) )
				result.Add( i );
		}

		return result;
	}

	public bool CheckClear( int row )
	{
		for( int i = 0; i < Width; i++ )
		{
			if( !Scripts[row, i].Occupied )
				return false;
		}
		return true;
	}
		
}
