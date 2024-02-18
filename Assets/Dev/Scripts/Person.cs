using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Hand[ ] hands;

	/// <summary>
	///		The total finger count of this person
	/// </summary>
	public int FingerCount => hands[0].FingerCount + hands[1].FingerCount;

	/// <summary>
	///		Get a reference to a finger at the specified index and on the specified hand
	/// </summary>
	/// <param name="handIndex">The hand index to check the finger of</param>
	/// <param name="fingerIndex">The index to check the finger at</param>
	/// <returns>
	///		A finger object if there is a finger at the specified index, null otherwise
	/// </returns>
	public Finger GetFingerAt (int handIndex, int fingerIndex) {
		// Make sure the hand index is within range
		if (handIndex < 0 || handIndex > hands.Length) {
			return null;
		}

		return hands[handIndex].GetFingerAt(fingerIndex);
	}
}
