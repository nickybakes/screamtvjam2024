using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.XR;

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Finger[ ] fingers;
	[SerializeField] private Person _person;
	[SerializeField] private BoxCollider mouseClickCollider;

	/// <summary>
	///		The person that this hand belongs to
	/// </summary>
	public Person Person { get => _person; private set => _person = value; }

	/// <summary>
	///		The number of fingers still on this hand
	/// </summary>
	public int FingerCount => fingers.Count(finger => finger != null);

	private void Update ( ) {
		mouseClickCollider.enabled = GameManager.Instance.CanSelectHands;
	}

	private void OnMouseDown ( ) {
		// If it is not the players turn, return from this function
		if (!GameManager.Instance.IsPlayerTurn) {
			return;
		}

		GameManager.Instance.SelectHand(this);
	}

	/// <summary>
	///		Get a finger on this hand at the specified index
	/// </summary>
	/// <param name="fingerIndex">The index of the finger to get</param>
	/// <returns>A finger object if the finger index corresponds to a still attached finger, null otherwise</returns>
	public Finger GetFingerAt (int fingerIndex) {
		// Make sure the finger index is within range
		if (fingerIndex < 0 || fingerIndex >= fingers.Length) {
			return null;
		}

		return fingers[fingerIndex];
	}

	/// <summary>
	///		Cut off a finger from this hand
	/// </summary>
	/// <param name="fingerIndex">The index of the finger to cut</param>
	public void CutFingerAt (int fingerIndex) {
		// Make sure the finger index is within range
		if (fingerIndex < 0 || fingerIndex >= fingers.Length) {
			return;
		}

		fingers[fingerIndex].Cut( );
	}

	/// <summary>
	///		Get a random unqiue list of fingers from this hand
	/// </summary>
	/// <param name="fingerCount">The number of random fingers to get</param>
	/// <param name="attached">Whether or not the random fingers should be attached or not</param>
	/// <returns>A list of random fingers from the fingers on this hand</returns>
	public Finger[ ] GetRandomFingers (int fingerCount, bool attached = true) {
		// A list of fingers that will be randomly selected from
		List<Finger> fingerList = new List<Finger>( );

		// Depending on the value of the attached variable, populate the finger list with that type of finger (either cut or attached)
		for (int i = 0; i < fingers.Length; i++) {
			if ((attached && fingers[i].FingerState != FingerState.CUT) || (!attached && fingers[i].FingerState == FingerState.CUT)) {
				fingerList.Add(fingers[i]);
			}
		}

		return Utils.GetRandomUniqueArrayItems(fingerList.ToArray( ), fingerCount);
	}
}