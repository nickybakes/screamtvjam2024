using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Hand _leftHand;
	[SerializeField] private Hand _rightHand;

	/// <summary>
	///		The name of this person
	/// </summary>
	public string Name => transform.name;

	/// <summary>
	///		A reference to the left hand of this person
	/// </summary>
	public Hand LeftHand { get => _leftHand; set => _leftHand = value; }

	/// <summary>
	///		A reference to the right hand of this person
	/// </summary>
	public Hand RightHand { get => _rightHand; set => _rightHand = value; }

	/// <summary>
	///		The total finger count of this person
	/// </summary>
	public int FingerCount => LeftHand.FingerCount + RightHand.FingerCount;

	/// <summary>
	///		Get a reference to a finger at the specified index and on the specified hand
	/// </summary>
	/// <param name="handType">The hand to check the finger of</param>
	/// <param name="fingerIndex">The index to check the finger at</param>
	/// <returns>
	///		A finger object if there is a finger at the specified index, null otherwise
	/// </returns>
	public Finger GetFingerAt (HandType handType, int fingerIndex) {
		if (handType == HandType.RIGHT) {
			return RightHand.GetFingerAtIndex(fingerIndex);
		}

		return LeftHand.GetFingerAtIndex(fingerIndex);
	}
}
