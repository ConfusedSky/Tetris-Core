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
		game.OnBlockDropped += new System.EventHandler( OnBlockDropped );
	}

	void Start()
	{
		OnBlockDropped( game, System.EventArgs.Empty );
	}

	void OnDisable()
	{
		game.OnBlockDropped -= new System.EventHandler( OnBlockDropped );
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
