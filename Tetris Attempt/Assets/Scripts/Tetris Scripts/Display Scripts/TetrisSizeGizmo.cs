using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisSizeGizmo : MonoBehaviour
{
	void OnDrawGizmos()
	{
		TetrisBoard board = gameObject.GetComponent<TetrisBoard>();
		float xs = transform.lossyScale.x;
		float ys = transform.lossyScale.y;

		float width = board.Width * board.PrefabSize.x;
		float height = board.Height * board.PrefabSize.y;
		Vector3 size = new Vector3( width * xs, height * ys, 0 );

		float x = transform.position.x + (board.StartingLocation.x + 
			width / 2 - board.PrefabSize.x / 2) * xs;
		float y = transform.position.y + (board.StartingLocation.y - 
			height / 2 + board.PrefabSize.y / 2) * ys;
		float z = transform.position.z + board.StartingLocation.z;

		Vector3 position = new Vector3( x, y, z );

		Gizmos.DrawCube(position, size);
	}
}
