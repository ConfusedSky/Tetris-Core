using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisGame : MonoBehaviour
{
	[Header("Dimensions:")]
	public int width = 10;
	public int height = 20;
	public Vector3 StartingLocation = new Vector3( 0, 0 );
	public Vector2 PrefabSize = new Vector2();

	[Header("Colors:")]
	public Color bgColor1;
	public Color bgColor2;

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
		blocks = new GameObject[height, width];
		blockScripts = new TetrisBlockScript[height, width];

		for( int i = 0; i < height; i++ )
		{
			for( int j = 0; j < width; j++ )
			{
				blocks[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					gameObject.transform
				);

				blockScripts[i, j] = blocks[i, j].GetComponent<TetrisBlockScript>();
				blockScripts[i, j].DefaultColor = (((j + i % 2) % 2 == 0) ? (bgColor1) : (bgColor2));
			}
		}

		UpdateBlocks();
		currentBlock = Tetronimo.CreateRandomTetronimo( blockScripts );
	}

	public bool Lost
	{
		get
		{
			for( int i = 0; i < width; i++ )
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
		for( int i = 0; i < width; i++ )
		{
			blockScripts[row, i].Clear();
		}

		for( int i = row; i >= 1; i-- )
		{
			for( int j = 0; j < width; j++ )
			{
				blockScripts[i - 1, j].MoveTo( blockScripts[i, j] );
			}
		}
	}

	public IList CheckClears()
	{
		ArrayList result = new ArrayList();

		for( int i = 0; i < height; i++ )
		{
			bool clearable = true;

			for( int j = 0; j < width; j++ )
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
		for( int i = 0; i < height; i++ )
		{
			for( int j = 0; j < width; j++ )
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
