using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tetris;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGame : MonoBehaviour
{
	public Color ShadowColor;

	public IInputManager InputManager;

	[Header("Queue Parameters")]
	[SerializeField]
	private int queueSize = 5;
	[SerializeField]
	private int queueLookback = 4;
	[SerializeField]
	private int queueTries = 4;

	private TetrisBoard board;
	private Mino currentBlock;
	private MinoType heldBlock = null;
	private RandomItemGenerator<MinoType> queue;
		
	public MinoType HeldBlock{ get{ return heldBlock; } }
	public MinoType[] QueuedBlocks{ get{ return queue.GetObjects(); } }
	public BaseTetrisBoard BoardLogic{ get{ return board.Controller; } }
	public TetrisBoard Board{ get{ return board; } }

	public event System.EventHandler OnHold;
	public event System.EventHandler OnBlockDropped;
	public event System.EventHandler OnStart;

	// Use this for initialization
	void Awake()
	{
		board = gameObject.GetComponent<TetrisBoard>();
		Mino.ShadowColor = ShadowColor.ToBlockColor();
		queue = new RandomItemGenerator<MinoType>( Tetromino.TETROMINO_TYPES, queueSize, queueLookback, queueTries );
	}

	void Start()
	{
		currentBlock = Mino.CreateNewMino( BoardLogic, queue.GetNextItem() );
		GameStart();
	}

	void GameStart()
	{
		currentBlock.BlockType = queue.GetNextItem();
		if( OnStart != null ) OnStart( this, System.EventArgs.Empty );
	}


}
