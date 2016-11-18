using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisBlockScript))]
public class UpcomingBlocksDisplay : MonoBehaviour {

	public GameObject TetrisPanel;
	public int Number;

	private TetrisGame game;
	private TetrisBlockScript blockScript;

	void Awake()
	{
		game = TetrisPanel.GetComponent<TetrisGame>();
		blockScript = gameObject.GetComponent<TetrisBlockScript>();
	}

	void OnEnable()
	{
		game.OnBlockDropped += OnBlockDropped;
		game.OnStart += OnBlockDropped;
		game.OnHold += OnBlockDropped;
	}

	void Start()
	{
		
	}

	void OnDisable()
	{
		game.OnBlockDropped -= OnBlockDropped;
		game.OnStart -= OnBlockDropped;
		game.OnHold -= OnBlockDropped;
	}

	void OnBlockDropped( object sender, System.EventArgs e )
	{
		TetronimoType[] queuedBlocks = game.QueuedBlocks;

		if( Number >= queuedBlocks.Length )
			blockScript.BlockColor = null;
		else
			blockScript.BlockColor = queuedBlocks[Number].Color();
	}
}
