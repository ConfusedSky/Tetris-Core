using UnityEngine;
using System.Collections;

public class TetrisBoard : MonoBehaviour 
{
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
