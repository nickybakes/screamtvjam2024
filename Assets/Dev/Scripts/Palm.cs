using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palm : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameManager gameManager;
	[SerializeField] private Hand hand;

	private void OnValidate ( ) {
		gameManager = FindObjectOfType<GameManager>( );
		hand = transform.GetComponentInParent<Hand>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void OnMouseDown ( ) {
		// If a hand cannot currently be selected, then return from this function
		if (!gameManager.CanSelectHand) {
			return;
		}

		gameManager.SelectedHand = hand;
	}
}
