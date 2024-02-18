using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Finger[ ] fingers;
	[SerializeField] private Person _person;

	/// <summary>
	///		The person that this hand belongs to
	/// </summary>
	public Person Person { get => _person; private set => _person = value; }

	/// <summary>
	///		The number of fingers still on this hand
	/// </summary>
	public int FingerCount => fingers.Count(finger => finger != null);

	private void OnValidate ( ) {
		Person = GetComponentInParent<Person>( );

		// Set finger indices
		for (int i = 0; i < fingers.Length; i++) {
			fingers[i].Index = i;
		}
	}

	private void Awake ( ) {
		OnValidate( );
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
		Finger finger = fingers[fingerIndex];
		fingers[fingerIndex] = null;

		Destroy(finger.gameObject);
	}
}