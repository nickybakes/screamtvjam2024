using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Die : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private List<int> faceValues;

	/// <summary>
	///		Get the face values of this die in string form
	/// </summary>
	public string DieString {
		get {
			string dieString = "";
			for (int i = 0; i < faceValues.Count; i++) {
				dieString += faceValues[i];

				if (i < faceValues.Count - 1) {
					dieString += " ";
				}
			}

			return dieString;
		}
	}

	/// <summary>
	///		Get a random value from this die
	/// </summary>
	/// <returns>A random integer face value on the die</returns>
	public int Roll ( ) => faceValues[Random.Range(0, faceValues.Count)];
}
