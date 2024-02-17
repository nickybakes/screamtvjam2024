using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Die : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameManager gameManager;
	[SerializeField] private List<int> faceValues;
	[Header("Properties")]
	[SerializeField] private int _index;

	/// <summary>
	///		The index of this die in the dice manager list
	/// </summary>
	public int Index { get => _index; set => _index = value; }

	private void OnValidate ( ) {
		gameManager = FindObjectOfType<GameManager>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void OnMouseDown ( ) {
		// If a die cannot currently be selected, then return from this function
		if (!gameManager.CanSelectDie) {
			return;
		}

		gameManager.SelectedDie = this;
	}

	/// <summary>
	///		Get a random value from this die
	/// </summary>
	/// <returns>A random integer face value on the die</returns>
	public int Roll ( ) => faceValues[Random.Range(0, faceValues.Count)];

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
