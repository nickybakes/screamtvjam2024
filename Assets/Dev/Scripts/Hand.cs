using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public enum HandType {
	LEFT, RIGHT, NONE
}

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private FingerTypeDictionary fingerDictionary;
	[Header("Properties")]
	[SerializeField] private HandType _handType;

	/// <summary>
	///		A reference to the thumb on this hand
	/// </summary>
	public Finger Thumb {
		get => fingerDictionary[GetHandedFingerType((int) FingerType.THUMB)];
		private set => fingerDictionary[GetHandedFingerType((int) FingerType.THUMB)] = value;
	}

	/// <summary>
	///		A reference to the index finger on this hand
	/// </summary>
	public Finger IndexFinger {
		get => fingerDictionary[GetHandedFingerType((int) FingerType.INDEX)];
		private set => fingerDictionary[GetHandedFingerType((int) FingerType.INDEX)] = value;
	}

	/// <summary>
	///		A reference to the middle finger on this hand
	/// </summary>
	public Finger MiddleFinger {
		get => fingerDictionary[GetHandedFingerType((int) FingerType.MIDDLE)];
		private set => fingerDictionary[GetHandedFingerType((int) FingerType.MIDDLE)] = value;
	}

	/// <summary>
	///		A reference to the ring finger on this hand
	/// </summary>
	public Finger RingFinger {
		get => fingerDictionary[GetHandedFingerType((int) FingerType.RING)]; 
		private set => fingerDictionary[GetHandedFingerType((int) FingerType.RING)] = value;
	}

	/// <summary>
	///		A reference to the pinky finger on this hand
	/// </summary>
	public Finger PinkyFinger {
		get => fingerDictionary[GetHandedFingerType((int) FingerType.PINKY)]; 
		private set => fingerDictionary[GetHandedFingerType((int) FingerType.PINKY)] = value;
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
	///		The type of this hand
	/// </summary>
	public HandType HandType => _handType;

	/// <summary>
	///		Convert a regular finger index to a handed finger index
	/// </summary>
	/// <param name="index">The index to convert</param>
	/// <returns>A handed index</returns>
	private FingerType GetHandedFingerType (int index) => (FingerType) (HandType == HandType.RIGHT ? index : 4 - index);

	/// <summary>
	///		Get a finger at the specified index
	/// </summary>
	/// <param name="index">The index of the finger to get</param>
	/// <returns>A reference to the finger if there is one at the index, null otherwise</returns>
	public Finger GetFingerAtIndex (int index) => fingerDictionary[GetHandedFingerType(index)];

	/// <summary>
	///		Set a finger at the specified index
	/// </summary>
	/// <param name="index">The index of the finger to set</param>
	/// <param name="finger">The new finger reference to set, can be set to null to remove the finger</param>
	public void SetFingerAtIndex (int index, Finger finger) => fingerDictionary[GetHandedFingerType(index)] = finger;
}