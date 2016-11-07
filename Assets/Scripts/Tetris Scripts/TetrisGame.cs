using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisGame : MonoBehaviour
{
	[Header("Dimensions:")]
	public int Width = 10;
	public int Height = 20;
	public Vector3 StartingLocation = new Vector3( 0, 0 );
	public Vector2 PrefabSize = new Vector2();

	[Header("Colors:")]
	public Color BGColor1;
	public Color BGColor2;
	public Color ShadowColor;

	[Header("Prefab")]
	public GameObject BlockPrefab;

	[Header("Input Manager:")]
	public InputManger InputManager;

	private GameObject[,] blocks;
	private TetrisBlockScript[,] blockScripts;
	private Tetronimo currentBlock;

	// Use this for initialization
	void Start()
	{
		blocks = new GameObject[Height, Width];
		blockScripts = new TetrisBlockScript[Height, Width];

		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				blocks[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					gameObject.transform
				);

				blockScripts[i, j] = blocks[i, j].GetComponent<TetrisBlockScript>();
				blockScripts[i, j].DefaultColor = (((j + i % 2) % 2 == 0) ? (BGColor1) : (BGColor2));
			}
		}

		Tetronimo.ShadowColor = ShadowColor;
		UpdateBlocks();
		currentBlock = Tetronimo.CreateRandomTetronimo( blockScripts );
	}

	public bool Lost
	{
		get
		{
			for( int i = 0; i < Width; i++ )
			{
				if( blockScripts[0, i].Occupied )
					return true;
			}
			return false;
		}
	}

	private TetronimoType? heldBlock = null;
	public TetronimoType? HeldBlock{ get{ return heldBlock; } }
	public event System.EventHandler OnHold;

	void Hold()
	{
		currentBlock.Clear();
		TetronimoType? temp = (currentBlock != null) ? (TetronimoType?)currentBlock.BlockType : null;
		if( heldBlock != null )
			currentBlock = Tetronimo.CreateNewTetronimo( blockScripts, heldBlock.GetValueOrDefault() );
		else
			currentBlock = null;
		heldBlock = temp;

		if( OnHold != null ) OnHold( this, System.EventArgs.Empty );
	}

	// Update is called once per frame
	void Update()
	{
		TetrisAction action = InputManager.HandleInput();

		if( action == TetrisAction.Hold ) Hold();

		if( currentBlock == null )
		{
			// if the previous frame had a drop that cleared resolve the clear
			foreach( int i in CheckClears() )
			{
				CollapseRow( i );
			}

			// Check for loss conditions if loss reset the board
			if( Lost )
			{
				Reset();
			}

			currentBlock = Tetronimo.CreateRandomTetronimo( blockScripts );
		}

		if( currentBlock != null ) currentBlock = currentBlock.Update( action );
	}

	public void Reset()
	{
		foreach( TetrisBlockScript b in blockScripts )
		{
			b.Clear();
		}

		heldBlock = null;
		OnHold( this, System.EventArgs.Empty );
	}

	public void CollapseRow( int row )
	{
		for( int i = 0; i < Width; i++ )
		{
			blockScripts[row, i].Clear();
		}

		for( int i = row; i >= 1; i-- )
		{
			for( int j = 0; j < Width; j++ )
			{
				blockScripts[i - 1, j].MoveTo( blockScripts[i, j] );
			}
		}
	}

	public IList CheckClears()
	{
		ArrayList result = new ArrayList();

		for( int i = 0; i < Height; i++ )
		{
			bool clearable = true;

			for( int j = 0; j < Width; j++ )
			{
				if( !blockScripts[i, j].Occupied )
					clearable = false;
			}

			if( clearable )
				result.Add( i );
		}

		return result;
	}

	void UpdateBlocks()
	{
		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				Vector3 position = new Vector3(
					                   StartingLocation.x + PrefabSize.x * j,
					                   StartingLocation.y - PrefabSize.y * i,
					                   StartingLocation.z
				                   );

				blocks[i, j].transform.localPosition = position;
			}
		}
	}
		
}
