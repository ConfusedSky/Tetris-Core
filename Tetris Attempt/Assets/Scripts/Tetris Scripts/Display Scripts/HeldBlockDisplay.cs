using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisBlockScript))]
public class HeldBlockDisplay : MonoBehaviour 
{
	public TetrisGame Game;
	private TetrisBlockScript blockScript;

	void Awake()
	{
		blockScript = gameObject.GetComponent<TetrisBlockScript>();
	}

	void OnEnable()
	{
		Game.BlockHeld += new System.EventHandler( OnHoldAction );
		Game.Started += new System.EventHandler( OnHoldAction );
	}

	void OnDisable()
	{
		Game.BlockHeld -= new System.EventHandler( OnHoldAction );
		Game.Started -= new System.EventHandler( OnHoldAction );
	}
	
	void OnHoldAction( object sender, System.EventArgs e )
	{
		if( Game.HeldBlock == null )
			blockScript.BlockColor = null;
		else
			blockScript.BlockColor = Game.HeldBlock.BlockColor.ToUnityColor();
	}
}
