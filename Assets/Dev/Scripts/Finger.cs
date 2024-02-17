using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameManager gameManager;
	[Header("Properties")]
	[SerializeField] private int _health;

	/// <summary>
	///		The current health of this finger
	/// </summary>
	public int Health { get => _health; set => _health = value; }

	private void OnValidate ( ) {
		gameManager = FindObjectOfType<GameManager>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void OnMouseDown ( ) {
		// If a finger cannot currently be selected, then return from this function
		if (!gameManager.CanSelectFinger) {
			return;
		}

		gameManager.SelectedFinger = this;
	}
}