using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tetris;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGame : MonoBehaviour
{
	public Color ShadowColor;

	public UnityInputManager InputManager;

	[Header("Queue Parameters")]
	[SerializeField]
	private int queueSize = 5;
	[SerializeField]
	private int queueLookback = 4;
	[SerializeField]
	private int queueTries = 4;

	private Tetris.Game game;
	private bool initialized = false;
		
	public MinoType HeldBlock{ get{ return game.HeldBlock; } }
	public MinoType[] QueuedBlocks{ get{ return game.QueuedBlocks; } }
	public RectangularTetrisBoard Board{ get{ return (RectangularTetrisBoard)game.Board; } }

	public event System.EventHandler BlockHeld;
	public event System.EventHandler BlockDropped;
	public event System.EventHandler Started;

	// Use this for initialization
	void Start()
	{
		Mino.ShadowColor = ShadowColor.ToBlockColor();
		RandomItemGenerator<MinoType> queue = new RandomItemGenerator<MinoType>( Tetromino.TETROMINO_TYPES, queueSize, queueLookback, queueTries );
		BaseTetrisBoard board = GetComponent<TetrisBoard> ().Controller;
		game = new Game (InputManager, board, queue);

		initialized = true;
		OnEnabled();
		game.GameStart();
	}

	void OnEnabled()
	{
		if (initialized) {
			game.OnEnable();
			game.BlockHeld += Game_BlockHeld;
			game.BlockDropped += Game_BlockDropped;
			game.Started += Game_Started;
		}
	}

	void Game_BlockHeld (object sender, System.EventArgs e)
	{
		if (BlockHeld != null )
			BlockHeld (this, e);
	}

	void Game_BlockDropped (object sender, System.EventArgs e)
	{
		if (BlockDropped != null)
			BlockDropped (this, e);
	}

	void Game_Started (object sender, System.EventArgs e)
	{
		if (Started != null)
			Started (this, e);
	}

	void OnDisabled()
	{
		game.OnDisable();
		game.BlockHeld -= Game_BlockHeld;
		game.BlockDropped -= Game_BlockDropped;
		game.Started -= Game_Started;
	}

	void Update()
	{
		game.Update (Time.deltaTime);
	}
}
