using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FingerState {
	ATTACHED, PARTIAL_CUT, CUT
}

public class Finger : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Hand _hand;
	[Header("Properties")]
	[SerializeField] private FingerState _fingerState;

	public FingerState FingerState {
		get => _fingerState;
		set {
			_fingerState = value;

			switch (_fingerState) {
				case FingerState.ATTACHED:
					break;
				case FingerState.PARTIAL_CUT:
					break;
				case FingerState.CUT:
					break;
			}
		}
	}

	/// <summary>
	///		The hand that this finger belongs to
	/// </summary>
	public Hand Hand { get => _hand; private set => _hand = value; }

	/// <summary>
	///		The person that this finger belongs to
	/// </summary>
	public Person Person => Hand.Person;

	private void OnMouseDown ( ) {
		// If it is not the players turn, return from this function
		if (!GameManager.Instance.IsPlayerTurn) {
			return;
		}

		GameManager.Instance.SelectFinger(this);
	}

	/// <summary>
	///		Cut this finger off of its hand
	/// </summary>
	public void Cut ( ) {
		// Cut the finger from the hand
		FingerState = FingerState.CUT;
	}

	/// <summary>
	///		Partially cut this finger
	/// </summary>
	public void PartialCut ( ) {
		if (FingerState == FingerState.ATTACHED) {
			// If the finger is fully attached, partial cut the finger
			FingerState = FingerState.PARTIAL_CUT;
		} else if (FingerState == FingerState.PARTIAL_CUT) {
			// If the finger is already partially cut, fully cut off the finger
			Cut( );
		}
	}
}