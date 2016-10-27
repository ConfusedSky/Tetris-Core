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
	public float RepeatDelay = 0.1f;
	public float RepeatRate = 0.03f;

	[Header("Controls:")]
	public string LeftButton = "Left";
	public string RightButton = "Right";

	private InputState state = InputState.None;
	private float timeTillRepeat = 0;

	public TetrisAction HandleInput()
	{
		timeTillRepeat -= Time.deltaTime;

		if( Input.GetButtonDown( LeftButton ) )
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
