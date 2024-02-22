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

	/// <summary>
	///		Cut a finger on a specific hand and at a specific index
	/// </summary>
	/// <param name="handIndex">The hand index of the hand that the finger is on</param>
	/// <param name="fingerIndex">The finger index to cut</param>
	public void CutFingerAt (int handIndex, int fingerIndex) {
		// Make sure the hand index is within range
		if (handIndex < 0 || handIndex > hands.Length) {
			return;
		}

		hands[handIndex].CutFingerAt(fingerIndex);
	}

	/// <summary>
	///		Get a random hand on this person
	/// </summary>
	/// <returns>A reference to the randomly selected hand</returns>
	public Hand GetRandomHand ( ) {
		return hands[Random.Range(0, 2)];
	}

	/// <summary>
	///		Get a random unqiue list of fingers from this hand
	/// </summary>
	/// <param name="fingerCount">The number of random fingers to get</param>
	/// <param name="attached">Whether or not the random fingers should be attached or not</param>
	/// <returns>A list of random fingers from the fingers on a random hand</returns>
	public Finger[ ] GetRandomFingers (int fingerCount, bool attached = true) {
		List<Finger> fingerList = new List<Finger>( );

		// Add all fingers from the hands to the finger list
		fingerList.AddRange(hands[0].GetRandomFingers(5, attached: attached));
		fingerList.AddRange(hands[1].GetRandomFingers(5, attached: attached));

		return Utils.GetRandomUniqueArrayItems(fingerList.ToArray( ), fingerCount);
	}
}
