using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Hand _hand;
	[SerializeField] private BoxCollider mouseClickCollider;
	[Header("Properties")]
	[SerializeField] private int _health;

	/// <summary>
	///		The current health of this finger
	/// </summary>
	public int Health {
		get => _health;
		set {
			_health = value;

			/// TODO: Set model type based on health value
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

	private void Update ( ) {
		mouseClickCollider.enabled = GameManager.Instance.CanSelectFingers;
	}

	private void OnMouseDown ( ) {
		GameManager.Instance.SelectFinger(this);
	}

	/// <summary>
	///		Cut this finger off of its hand
	/// </summary>
	public void Cut ( ) {
		Hand.CutFinger(this);
	}
}