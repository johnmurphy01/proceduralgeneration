using UnityEngine;
using System.Collections;

public class MazeCell : MonoBehaviour
{
	public IntVector2 coordinates;
	public MazeRoom room;

	private int initializedEdgeCount;
	private MazeEdgeCell[] edges = new MazeEdgeCell[MazeDirections.Count];

	public void Initialize(MazeRoom room)
	{
		room.Add (this);
		transform.GetChild (0).GetComponent<Renderer> ().material = room.settings.floorMaterial;
	}

	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MazeDirections.Count;
		}
	}

	public MazeDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range (0, MazeDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MazeDirections.Count; i++) {
				if (edges [i] == null) {
					if (skips == 0) {
						return (MazeDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

	public MazeEdgeCell GetEdge (MazeDirection direction)
	{
		return edges [(int)direction];
	}

	public void SetEdge (MazeDirection direction, MazeEdgeCell edge)
	{
		edges [(int)direction] = edge;
		initializedEdgeCount += 1;
	}
}
