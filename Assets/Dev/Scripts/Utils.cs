using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
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
}
