using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tetris;

public class TetrisBoard : MonoBehaviour 
{
	[Header("Parent Objects:")]
	public GameObject BlockFolder;
	public GameObject BackgroundFolder;

	[Header("Dimensions:")]
	public int Width = 10;
	public int Height = 20;
	public Vector3 StartingLocation = new Vector3( 0, 0 );
	public Vector2 PrefabSize = new Vector2();

	[Header("Colors:")]
	public Color BGColor1;
	public Color BGColor2;

	[Header("Prefab")]
	public GameObject BlockPrefab;

	private GameObject[,] background;
	private GameObject[,] blocks;
	private TetrisBlockScript[,] scripts;
	private RectangularTetrisBoard logic;

	public GameObject[,] Blocks{ get{ return blocks; } }
	public TetrisBlockScript[,] Scripts{ get { return scripts; } }
	public RectangularTetrisBoard Controller{ get { return logic; } }

	public event System.EventHandler OnBoardChanged;

	void Awake()
	{
		background = new GameObject[Height, Width];
		blocks = new GameObject[Height, Width];
		scripts = new TetrisBlockScript[Height, Width];

		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				// Set up the actual blocks
				Blocks[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					BlockFolder.transform
				);

				// Assign the scripts to the variable and make their background invisible
				Scripts[i, j] = Blocks[i, j].GetComponent<TetrisBlockScript>();
				Scripts[i, j].DefaultColor = new Color( 0, 0, 0, 0 );

				// Make the actual background
				// The background is separate because 
				background[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					BackgroundFolder.transform
				);

				background[i, j].GetComponent<TetrisBlockScript>().DefaultColor = 
					(((j + i % 2) % 2 == 0) ? (BGColor1) : (BGColor2));
			}
		}

		logic = new RectangularTetrisBoard( Width, Height, 1 );
	}

	void OnEnable()
	{
		logic.BoardChanged += Logic_BoardChanged;
	}

	void OnDisable()
	{
		logic.BoardChanged += Logic_BoardChanged;
	}

	void Logic_BoardChanged (object sender, System.EventArgs e)
	{
		if (OnBoardChanged != null)
			OnBoardChanged (this, System.EventArgs.Empty);
	}

	void Start()
	{
		UpdateBlocks();
	}

	void Update()
	{
		// Todo: add some kind of soft board changed which updates whenever the background changes for efficency
		for( int i = 0; i < Height; i++ ) {
			for( int j = 0; j < Width; j++ ){
				Scripts[i,j].BackgroundColor = logic.GetBlockAt( new Point( j, i ) ).DisplayedColor.ToUnityColor();
			}
		}
	}

	private void UpdateBlocks()
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

				Blocks[i, j].transform.localPosition = position;
				background[i, j].transform.localPosition = position;
			}
		}
	}

}
