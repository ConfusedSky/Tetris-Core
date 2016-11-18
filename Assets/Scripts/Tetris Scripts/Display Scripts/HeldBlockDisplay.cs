using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisBlockScript))]
public class HeldBlockDisplay : MonoBehaviour {

	public GameObject TetrisPanel;

	private TetrisGame game;
	private TetrisBlockScript blockScript;

	void Awake()
	{
		game = TetrisPanel.GetComponent<TetrisGame>();
		blockScript = gameObject.GetComponent<TetrisBlockScript>();
	}

	void OnEnable()
	{
		game.OnHold += new System.EventHandler( OnHoldAction );
		game.OnStart += new System.EventHandler( OnHoldAction );
	}

	void OnDisable()
	{
		game.OnHold -= new System.EventHandler( OnHoldAction );
		game.OnStart -= new System.EventHandler( OnHoldAction );
	}
	
	void OnHoldAction( object sender, System.EventArgs e )
	{
		if( game.HeldBlock == null )
			blockScript.BlockColor = null;
		else
			blockScript.BlockColor = ((TetronimoType)game.HeldBlock).Color();
	}
}
