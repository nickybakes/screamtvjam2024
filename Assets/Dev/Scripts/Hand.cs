using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private FingerTypeDictionary fingerDictionary;
	[Header("Properties")]
	[SerializeField] private bool _isRightHand;

	/// <summary>
	///		A reference to the thumb on this hand
	/// </summary>
	public Finger Thumb {
		get => fingerDictionary[FingerType.THUMB];
		private set => fingerDictionary[FingerType.THUMB] = value;
	}

	/// <summary>
	///		A reference to the index finger on this hand
	/// </summary>
	public Finger IndexFinger {
		get => fingerDictionary[FingerType.INDEX];
		private set => fingerDictionary[FingerType.INDEX] = value;
	}

	/// <summary>
	///		A reference to the middle finger on this hand
	/// </summary>
	public Finger MiddleFinger {
		get => fingerDictionary[FingerType.MIDDLE];
		private set => fingerDictionary[FingerType.MIDDLE] = value;
	}

	/// <summary>
	///		A reference to the ring finger on this hand
	/// </summary>
	public Finger RingFinger {
		get => fingerDictionary[FingerType.RING];
		private set => fingerDictionary[FingerType.RING] = value;
	}

	/// <summary>
	///		A reference to the pinky finger on this hand
	/// </summary>
	public Finger PinkyFinger {
		get => fingerDictionary[FingerType.PINKY];
		private set => fingerDictionary[FingerType.PINKY] = value;
	}

	/// <summary>
	///		The number of fingers that this hand has
	/// </summary>
	public int FingerCount {
		get {
			int sum = 0;

			// Loop through all fingers on the hand
			for (int i = 0; i < 5; i++) {
				// If the current finger is not null, then increment the sum of fingers
				if (GetFingerAtIndex(i)) {
					sum++;
				}
			}

			return sum;
		}
	}

	/// <summary>
	///		Whether or not this hand is a right hand
	/// </summary>
	public bool IsRightHand => _isRightHand;

	/// <summary>
	///		Check to see if a finger is at a specific index
	/// </summary>
	/// <param name="index">The index of the finger to check</param>
	/// <returns>
	///		A reference to the finger if there is one at the index, null otherwise
	/// </returns>
	public Finger GetFingerAtIndex (int index) {
		// If the index is out of range of the dictionary, return null
		if (index < 0 || index > 4) {
			Debug.LogWarning("Finger index out of range.");
			return null;
		}

		return fingerDictionary[(FingerType) (IsRightHand ? index : 4 - index)];
	}

	public void RemoveFingerAtIndex (int index) {
		// Get the finger at the specified index
		Finger finger = GetFingerAtIndex(index);

		// If the finger is equal to null, then return and remove nothing
		if (!finger) {
			return;
		}


	}
}