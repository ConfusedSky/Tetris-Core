using System;

namespace Tetris
{
	public class Game
	{
		public Game( IInputManager input, BaseTetrisBoard board, RandomItemGenerator<MinoType> queue )
		{
			inputManager = input;
			this.board = board;
			this.queue = queue;

			currentBlock = Mino.CreateNewMino( board, queue.GetNextItem() );
			OnEnable();
		}

		private IInputManager inputManager;
		private BaseTetrisBoard board;
		private Mino currentBlock;
		private MinoType heldBlock = null;
		private RandomItemGenerator<MinoType> queue;

		public BaseTetrisBoard Board { get { return board; } }
		public MinoType HeldBlock { get { return heldBlock; } }
		public MinoType[] QueuedBlocks{ get { return queue.GetObjects(); } }

		public event System.EventHandler BlockHeld;
		public event System.EventHandler BlockDropped;
		public event System.EventHandler Started;

		public void OnEnable()
		{
			board.BoardChanged += OnBoardChanged;
		}

		public void OnDisable()
		{
			board.BoardChanged -= OnBoardChanged;
		}

		void OnBoardChanged (object sender, System.EventArgs e)
		{
			// Check for loss conditions if loss reset the board
			if( board.Lost )
			{
				Reset();
				return;
			}
		}

		public void GameStart()
		{
			currentBlock.BlockType = queue.GetNextItem();
            Started?.Invoke(this, System.EventArgs.Empty);
        }

		private void Hold()
		{
			MinoType temp = currentBlock.BlockType;
			if( heldBlock != null )
				currentBlock.BlockType = heldBlock;
			else
				currentBlock.BlockType = queue.GetNextItem();
			heldBlock = temp;

            BlockHeld?.Invoke(this, System.EventArgs.Empty);
        }

		// Update is called once per frame
		public void Update( float deltaTime )
		{
			TetrisAction action = inputManager.HandleInput( deltaTime );

			if( action == TetrisAction.Hold ) Hold();

			if (currentBlock == null)
				return;

			if( currentBlock.Alive )
				currentBlock = currentBlock.Update( action );
			else
			{
				MinoType nextTetronimo = queue.GetNextItem();
                BlockDropped?.Invoke(this, System.EventArgs.Empty);
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
}

