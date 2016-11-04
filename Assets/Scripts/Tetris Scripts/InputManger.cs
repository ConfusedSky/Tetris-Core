using System;
using UnityEngine;

[Serializable]
public class InputManger
{
	private enum InputState
	{
		LeftDown,
		RightDown,
		None
	}

	[Header("Repeater Parameters:")]
	public float RepeatDelay = 0.15f;
	public float RepeatRate = 0.05f;

	[Header("Controls:")]
	public string LeftButton = "Left";
	public string RightButton = "Right";
	public string DownButton = "Down";
	public string DropButton = "Drop";
	public string RotateRightButton = "RotateRight";

	private InputState state = InputState.None;
	private float timeTillRepeat = 0;

	public TetrisAction HandleInput()
	{
		timeTillRepeat -= Time.deltaTime;

		if( Input.GetButtonDown( DropButton ) )
		{
			return TetrisAction.Drop;
		}
		else if( Input.GetButtonDown( LeftButton ) )
		{
			state = InputState.LeftDown;
			timeTillRepeat = RepeatDelay;
			return TetrisAction.Left;
		}
		else if( Input.GetButtonDown( RightButton ) )
		{
			state = InputState.RightDown;
			timeTillRepeat = RepeatDelay;
			return TetrisAction.Right;
		}
		else if( Input.GetButtonDown( DownButton ) )
		{
			return TetrisAction.Down;
		}
		else if( Input.GetButtonDown( RotateRightButton ) )
		{
			return TetrisAction.RotateRight;
		}
		else if( state == InputState.LeftDown )
		{
			if( !Input.GetButton( LeftButton ) )
			{
				state = InputState.None;
			}
			else if( timeTillRepeat <= 0 )
			{
				timeTillRepeat += RepeatRate;
				return TetrisAction.Left;
			}
		}
		else if( state == InputState.RightDown )
		{
			if( !Input.GetButton( RightButton ) )
			{
				state = InputState.None;
			}
			else if( timeTillRepeat <= 0 )
			{
				timeTillRepeat += RepeatRate;
				return TetrisAction.Right;
			}
		}

		return TetrisAction.None;
	}
}
