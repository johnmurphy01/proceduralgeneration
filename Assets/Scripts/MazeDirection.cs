using UnityEngine;

public enum MazeDirection{
	North,
	East,
	South,
	West
}

public static class MazeDirections
{
	public const int Count = 4;

	private static IntVector2[] vectors = 
	{
		new IntVector2(0,1),
		new IntVector2(1,0),
		new IntVector2(0,-1),
		new IntVector2(-1,0)
	};

	private static MazeDirection[] opposites = 
	{
		MazeDirection.South,
		MazeDirection.West,
		MazeDirection.North,
		MazeDirection.East
	};

	private static Quaternion[] rotations =
	{
		Quaternion.identity,
		Quaternion.Euler (0f, 90f, 0f),
		Quaternion.Euler (0f, 180f, 0f),
		Quaternion.Euler (0f, 270f, 0f)
	};

	public static MazeDirection RandomValue
	{
		get
		{
			return (MazeDirection)Random.Range(0,Count);
		}
	}

	public static MazeDirection GetOpposite(this MazeDirection direction)
	{
		return opposites [(int)direction];
	}

	public static Quaternion ToRotation(this MazeDirection direction)
	{
		return rotations [(int)direction];
	}

	public static IntVector2 ToIntVector2 (this MazeDirection direction)
	{
		return vectors[(int)direction];
	}
}