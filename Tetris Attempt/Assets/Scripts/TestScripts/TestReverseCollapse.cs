using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReverseCollapse : MonoBehaviour {

	public GarbageGenerator gg;
	float n;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		n = (int)GUI.HorizontalSlider (new Rect(0,200,200,10),n,0,20);
		GUI.Label (new Rect (0, 210, 100, 100), ""+n);
		if (GUI.Button (new Rect (0, 0, 200, 200), "Collapse a row"))
			gg.GenerateGarbage ((int)n);	
	}
}
