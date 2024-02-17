using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	/// <summary>
	///		Create a list of points that are evenly spaced around a circle
	/// </summary>
	/// <param name="center">The center position of the circle of points</param>
	/// <param name="radius">The radius of the circle</param>
	/// <param name="offsetRadians">The number of radians to offset the points by</param>
	/// <param name="pointCount">The number of points to space around the circle</param>
	/// <returns>A list of points that are evenly spaced around a circle</returns>
	public static List<Vector3> GetSpacedPointsOnCircle (Vector3 center, float radius, float offsetRadians, int pointCount) {
		List<Vector3> points = new List<Vector3>( );
		float angleSpacing = (Mathf.PI * 2) / pointCount;

		for (int i = 0; i < pointCount; i++) {
			float x = center.x + (radius * Mathf.Cos((angleSpacing * i) + offsetRadians));
			float y = center.y;
			float z = center.z + (radius * Mathf.Sin((angleSpacing * i) + offsetRadians));

			points.Add(new Vector3(x, y, z));
		}

		return points;
	}

	/// <summary>
	///		Create a list of points that are evenly spaced along a line
	/// </summary>
	/// <param name="center">The center of the line to generate the points on</param>
	/// <param name="length">The length of the line</param>
	/// <param name="pointCount">The number of points to space along the line</param>
	/// <returns>A list of points that are evenly spaced along a line</returns>
	public static List<Vector3> GetSpacedPointsOnLine (Vector3 center, float length, int pointCount) {
		List<Vector3> points = new List<Vector3>( );
		Vector3 startPoint = center - new Vector3(length / 2f, 0f, 0f);

		for (int i = 0; i < pointCount; i++) {
			float fraction = (float) i / (pointCount - 1);
			points.Add(new Vector3(fraction * length, 0f, 0f) + startPoint);
		}

		return points;
	}
}
