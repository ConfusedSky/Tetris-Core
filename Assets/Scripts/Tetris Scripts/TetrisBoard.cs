using UnityEngine;
using System.Collections;

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

	public GameObject[,] Blocks{ get{ return blocks; } }
	public TetrisBlockScript[,] Scripts{ get { return scripts; } }

	void Awake()
	{
		background = new GameObject[Height, Width];
		blocks = new GameObject[Height, Width];
		scripts = new TetrisBlockScript[Height, Width];

		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				Blocks[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					BlockFolder.transform
				);

				Scripts[i, j] = Blocks[i, j].GetComponent<TetrisBlockScript>();
				Scripts[i, j].DefaultColor = new Color( 0, 0, 0, 0 );

				background[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					BackgroundFolder.transform
				);

				background[i, j].GetComponent<TetrisBlockScript>().DefaultColor = 
					(((j + i % 2) % 2 == 0) ? (BGColor1) : (BGColor2));
			}
		}
	}

	void Start()
	{
		UpdateBlocks();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Reset()
	{
		foreach( TetrisBlockScript b in Scripts )
		{
			b.Clear();
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
