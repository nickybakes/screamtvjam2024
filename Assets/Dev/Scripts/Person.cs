using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Hand _leftHand;
	[SerializeField] private Hand _rightHand;

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
}
