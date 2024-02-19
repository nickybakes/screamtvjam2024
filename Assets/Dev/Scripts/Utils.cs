using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	/// <summary>
	///		Create a list of points that are evenly spaced around an arc
	/// </summary>
	/// <param name="center">The center position of the arc of points</param>
	/// <param name="startAngle">The starting angle of the arc</param>
	/// <param name="endAngle">The ending angle of the arc</param>
	/// <param name="radius">The radius of the arc</param>
	/// <param name="pointCount">The number of points to space around the arc</param>
	/// <returns>A list of points that are evenly spaced around a arc</returns>
	public static List<Vector3> GetSpacedPointsOnArc (Vector3 center, float startAngle, float endAngle, float radius, int pointCount) {
		List<Vector3> points = new List<Vector3>( );
		float angleSpacing = (endAngle - startAngle) / pointCount;

		for (int i = 0; i < pointCount; i++) {
			float x = center.x + (radius * Mathf.Cos((angleSpacing * i) + startAngle));
			float y = center.y;
			float z = center.z + (radius * Mathf.Sin((angleSpacing * i) + startAngle));

			points.Add(new Vector3(x, y, z));
		}

		return points;
	}

	/// <summary>
	///		Create a list of points that are evenly spaced around a circle
	/// </summary>
	/// <param name="center">The center of the circle</param>
	/// <param name="radius">The radius of the circle</param>
	/// <param name="pointCount">The number of points to evenly space around the circle</param>
	/// <returns>A list of points that are evenly spaced around a circle</returns>
	public static List<Vector3> GetSpacedPointsOnCircle (Vector3 center, float radius, int pointCount) {
		return GetSpacedPointsOnArc(center, 0f, Mathf.PI * 2, radius, pointCount);
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

		for (int i = 0; i < pointCount; i++) {
			float fraction = (float) i / (pointCount - 1);
			points.Add(new Vector3((fraction - 0.5f) * length, 0f, 0f) + center);
		}

		return points;
	}
}
