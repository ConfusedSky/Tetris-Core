﻿namespace Tetris
{
	public interface IInputManager
	{
		TetrisAction HandleInput( float deltaTime );
	}
}
