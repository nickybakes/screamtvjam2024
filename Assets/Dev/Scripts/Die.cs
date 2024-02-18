using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Die : MonoBehaviour {
	[Header("References")]
	[SerializeField] private BoxCollider mouseClickCollider;
	[Header("Properties")]
	[SerializeField] private List<int> faceValues;

	private void Update ( ) {
		mouseClickCollider.enabled = GameManager.Instance.CanSelectDice;
	}

	private void OnMouseDown ( ) {
		GameManager.Instance.SelectDie(this);
	}

	/// <summary>
	///		Get a random value from this die
	/// </summary>
	/// <returns>A random integer face value on the die</returns>
	public int Roll ( ) {
		return faceValues[Random.Range(0, faceValues.Count)];
	}

	public override string ToString ( ) {
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
