using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TetrisGame))]
public class TetrisSizeGizmo : MonoBehaviour
{
	void OnDrawGizmos()
	{
		TetrisGame game = gameObject.GetComponent<TetrisGame>();

		float width = game.width * game.PrefabSize.x;
		float height = game.height * game.PrefabSize.y;
		Vector3 size = new Vector3( width, height, 0 );

		float x = transform.position.x + game.StartingLocation.x + 
			      width / 2 - game.PrefabSize.x / 2;
		float y = transform.position.y + game.StartingLocation.y - 
			      height / 2 + game.PrefabSize.y / 2;
		float z = transform.position.z + game.StartingLocation.z;

		Vector3 position = new Vector3( x, y, z );

		Gizmos.DrawCube(position, size);
	}
}
