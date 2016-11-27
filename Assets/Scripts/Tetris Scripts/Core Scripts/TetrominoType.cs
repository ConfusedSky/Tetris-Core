using UnityEngine;

[System.Serializable]
public enum TetrominoType : int
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
	public static TetrominoType[] Values = 
	{ 
		TetrominoType.I,
		TetrominoType.O,
		TetrominoType.T,
		TetrominoType.J,
		TetrominoType.L,
		TetrominoType.S,
		TetrominoType.Z
	};

	public static Color Color(this TetrominoType type )
	{
		return Mino.TETROMINO_COLORS[(int)type];
	}
}