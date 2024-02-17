using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Hand[ ] _hands;

	/// <summary>
	///		A list of the hands on this person
	/// </summary>
	public Hand[ ] Hands { get => _hands; private set => _hands = value; }

	/// <summary>
	///		The total finger count of this person
	/// </summary>
	public int FingerCount => Hands[0].FingerCount + Hands[1].FingerCount;

	/// <summary>
	///		Get a reference to a finger at the specified index and on the specified hand
	/// </summary>
	/// <param name="handType">The hand index to check the finger of</param>
	/// <param name="fingerIndex">The index to check the finger at</param>
	/// <returns>
	///		A finger object if there is a finger at the specified index, null otherwise
	/// </returns>
	public Finger GetFingerAt (int handIndex, int fingerIndex) => Hands[handIndex].Fingers[fingerIndex];
}
