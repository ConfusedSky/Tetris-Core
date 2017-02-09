using UnityEngine;
using System.Collections;
using Tetris;

[RequireComponent(typeof(TetrisBlockScript))]
public class UpcomingBlocksDisplay : MonoBehaviour 
{
	public int Number;

	public TetrisGame Game;
	private TetrisBlockScript blockScript;

	void Awake()
	{
		blockScript = gameObject.GetComponent<TetrisBlockScript>();
	}

	void OnEnable()
	{
		Game.OnBlockDropped += OnBlockDropped;
		Game.OnStart += OnBlockDropped;
		Game.OnHold += OnBlockDropped;
	}

	void Start()
	{
		
	}

	void OnDisable()
	{
		Game.OnBlockDropped -= OnBlockDropped;
		Game.OnStart -= OnBlockDropped;
		Game.OnHold -= OnBlockDropped;
	}

	void OnBlockDropped( object sender, System.EventArgs e )
	{
		MinoType[] queuedBlocks = Game.QueuedBlocks;

		if( Number >= queuedBlocks.Length )
			blockScript.BlockColor = null;
		else
			blockScript.BlockColor = queuedBlocks[Number].BlockColor.ToUnityColor();
	}
}
