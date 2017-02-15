using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class TetrisBlockScript : MonoBehaviour
{
	// if color is null it also means that this position is empty
	public Color? BlockColor
	{ 
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}

	public Color DefaultColor = Color.white;
	private SpriteRenderer sr;

	private Color? _color = null;

	// Use this for initialization
	void Start()
	{
		sr = gameObject.transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update()
	{
		sr.color = BlockColor ?? DefaultColor;
	}

	public void Clear()
	{
		BlockColor = null;
	}
}
