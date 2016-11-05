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
			colorChanged = true;
		}
	}
	public Color? BackgroundColor
	{ 
		get
		{
			return _background;
		}
		set
		{
			_background = value;
			colorChanged = true;
		}
	}

	public Color DefaultColor = Color.white;
	private SpriteRenderer sr;

	private Color? _color = null;
	private Color? _background = null;
	private bool colorChanged = true;

	// Use this for initialization
	void Start()
	{
		sr = gameObject.transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if( colorChanged ) ColorChange();
	}

	public void MoveTo( TetrisBlockScript destination )
	{
		destination.BlockColor = BlockColor;

		Clear();
	}

	public void Clear()
	{
		BlockColor = null;
		BackgroundColor = null;
	}

	void ColorChange()
	{
		sr.color = BlockColor ?? BackgroundColor ?? DefaultColor;
	}

	public bool Occupied
	{
		get { return BlockColor != null; }
	}
}
