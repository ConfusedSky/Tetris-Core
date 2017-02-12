using System;

namespace Tetris
{
	public class Game
	{
		public Game()
		{
		}

		private IInputManager InputManager;
		private BaseTetrisBoard board;
		private Mino currentBlock;
		private MinoType heldBlock = null;
		private RandomItemGenerator<MinoType> queue;

		public event System.EventHandler OnHold;
		public event System.EventHandler OnBlockDropped;
		public event System.EventHandler OnStart;

		void OnEnable()
		{
			board.BoardChanged += OnBoardChanged;
		}

		void OnDisable()
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

		void GameStart()
		{
			currentBlock.BlockType = queue.GetNextItem();
			if( OnStart != null ) OnStart( this, System.EventArgs.Empty );
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
}

