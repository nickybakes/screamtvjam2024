using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Finger _thumb;
	[SerializeField] private Finger _indexFinger;
	[SerializeField] private Finger _middleFinger;
	[SerializeField] private Finger _ringFinger;
	[SerializeField] private Finger _pinkyFinger;
	[Header("Properties")]
	[SerializeField] private bool _isRightHand;

	/// <summary>
	///		A reference to the thumb on this hand
	/// </summary>
	public Finger Thumb { get => _thumb; private set => _thumb = value; }

	/// <summary>
	///		A reference to the index finger on this hand
	/// </summary>
	public Finger IndexFinger { get => _indexFinger; private set => _indexFinger = value; }

	/// <summary>
	///		A reference to the middle finger on this hand
	/// </summary>
	public Finger MiddleFinger { get => _middleFinger; private set => _middleFinger = value; }

	/// <summary>
	///		A reference to the ring finger on this hand
	/// </summary>
	public Finger RingFinger { get => _ringFinger; private set => _ringFinger = value; }

	/// <summary>
	///		A reference to the pinky finger on this hand
	/// </summary>
	public Finger PinkyFinger { get => _pinkyFinger; private set => _pinkyFinger = value; }

	/// <summary>
	///		The number of fingers that this hand has
	/// </summary>
	public int FingerCount => (Thumb ? 1 : 0) + (IndexFinger ? 1 : 0) + (MiddleFinger ? 1 : 0) + (RingFinger ? 1 : 0) + (PinkyFinger ? 1 : 0);

	/// <summary>
	///		Whether or not this hand is a right hand
	/// </summary>
	public bool IsRightHand => _isRightHand;

	/// <summary>
	///		Check to see if a finger is at a specific index
	/// </summary>
	/// <param name="index">The index of the finger to check</param>
	/// <returns>
	///		<strong>true</strong> if the finger at the specified index is not null, <strong>false</strong> otherwise
	/// </returns>
	public bool IsFingerAtIndex (int index) {
		switch (index) {
			case 0:
				return IsRightHand ? Thumb : PinkyFinger;
			case 1:
				return IsRightHand ? IndexFinger : RingFinger;
			case 2:
				return MiddleFinger;
			case 3:
				return IsRightHand ? RingFinger : IndexFinger;
			case 4:
				return IsRightHand ? PinkyFinger : Thumb;
			default:
				Debug.LogWarning("Finger index out of range.");
				return false;
		}
	}
}