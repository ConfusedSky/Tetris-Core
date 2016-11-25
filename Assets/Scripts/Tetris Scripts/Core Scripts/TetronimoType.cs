using UnityEngine;

[System.Serializable]
public enum TetronimoType : int
{
	I = 0,
	O = 1,
	T = 2,
	J = 3,
	L = 4,
	S = 5,
	Z = 6
}

public static class TetronimoTypeFunctions
{
	public static TetronimoType[] Values = 
	{ 
		TetronimoType.I,
		TetronimoType.O,
		TetronimoType.T,
		TetronimoType.J,
		TetronimoType.L,
		TetronimoType.S,
		TetronimoType.Z
	};

	public static Color Color(this TetronimoType type )
	{
		return Tetronimo.TETRONIMO_COLORS[(int)type];
	}
}