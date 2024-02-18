using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameManager gameManager;
	[SerializeField] private Hand _hand;
	[Header("Properties")]
	[SerializeField] private int _health;
	[SerializeField] private int _index;

	/// <summary>
	///		The current health of this finger
	/// </summary>
	public int Health { get => _health; set => _health = value; }

	/// <summary>
	///		The index of this die in the dice manager list
	/// </summary>
	public int Index { get => _index; set => _index = value; }

	/// <summary>
	///		The hand that this finger belongs to
	/// </summary>
	public Hand Hand { get => _hand; private set => _hand = value; }

	/// <summary>
	///		The person that this finger belongs to
	/// </summary>
	public Person Person => Hand.Person;

	private void OnValidate ( ) {
		gameManager = FindObjectOfType<GameManager>( );
		Hand = GetComponentInParent<Hand>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void OnMouseDown ( ) {
		gameManager.SelectedFinger = this;
	}

	/// <summary>
	///		Cut this finger off of its hand
	/// </summary>
	public void Cut ( ) {
		Hand.CutFingerAt(Index);
	}
}