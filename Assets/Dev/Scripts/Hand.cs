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
	///		Convert a regular finger index to a handed finger index
	/// </summary>
	/// <param name="index">The index to convert</param>
	/// <returns>A handed index</returns>
	private FingerType GetHandedFingerTypeFromIndex (int index) => (FingerType) (IsRightHand ? index : 4 - index);

	/// <summary>
	///		Get a finger at the specified index
	/// </summary>
	/// <param name="index">The index of the finger to get</param>
	/// <returns>A reference to the finger if there is one at the index, null otherwise</returns>
	public Finger GetFingerAtIndex (int index) {
		// If the index is out of range of the dictionary, return null
		if (index < 0 || index > 4) {
			Debug.LogWarning("Finger index out of range.");
			return null;
		}

		return fingerDictionary[GetHandedFingerTypeFromIndex(index)];
	}

	/// <summary>
	///		Set a finger at the specified index
	/// </summary>
	/// <param name="index">The index of the finger to set</param>
	/// <param name="finger">The new finger reference to set, can be set to null to remove the finger</param>
	public void SetFingerAtIndex (int index, Finger finger) {
		// If the index is out of range of the dictionary, return null
		if (index < 0 || index > 4) {
			Debug.LogWarning("Finger index out of range.");
			return;
		}

		fingerDictionary[GetHandedFingerTypeFromIndex(index)] = finger;
	}
}