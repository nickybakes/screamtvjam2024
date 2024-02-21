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

	/// <summary>
	///		Get a list of random unqiue items from an array
	/// </summary>
	/// <typeparam name="T">The type of list to put in and return from this function</typeparam>
	/// <param name="array">The array to get the random items from</param>
	/// <param name="count">The number of random items to return</param>
	/// <returns>A list of random items from the input array that are all different. If the inputted count is greater than the length of the array, the entire starting array will be returned with no extra values.</returns>
	public static T[ ] GetRandomUniqueArrayItems<T> (T[ ] array, int count) {
		// If the item count is equal or greater than the array length, just return the entire array
		if (count >= array.Length) {
			return array;
		}

		// Create lists to store available indices and the randomly selected items
		List<int> availableIndices = new List<int>( );
		T[ ] randomItems = new T[count];

		// Populate the indices array with every index in the array
		for (int i = 0; i < array.Length; i++) {
			availableIndices.Add(i);
		}

		// Get random array items without having the same indices
		for (int i = 0; i < count; i++) {
			// Get a random available index
			int randomIndex = Random.Range(0, availableIndices.Count);
			int randomIndexValue = availableIndices[randomIndex];

			// Add the item at the random index to the random array
			randomItems[i] = array[randomIndexValue];
			availableIndices.RemoveAt(randomIndex);
		}

		return randomItems;
	}
}
