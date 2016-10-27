using UnityEngine;
using System.Collections;

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
		blockScripts[height - 1, 3].BlockColor = Color.black;
		blockScripts[height - 1, 5].BlockColor = Color.black;
		blockScripts[height - 2, 3].BlockColor = Color.black;
		blockScripts[height - 6, 7].BlockColor = Color.black;
		currentBlock = Tetronimo.CreateRandomTetronimo( blockScripts );
	}

	// Update is called once per frame
	void Update()
	{
		TetrisAction action = InputManager.HandleInput();

		if(currentBlock != null) currentBlock.Update( action );
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
