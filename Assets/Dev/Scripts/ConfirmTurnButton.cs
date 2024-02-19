using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmTurnButton : MonoBehaviour {
	[Header("References")]
	[SerializeField] private BoxCollider mouseClickCollider;

	private void Update ( ) {
		// This button can only be enabled when at least one die and one hand are selected
		mouseClickCollider.enabled = GameManager.Instance.CheckForSelectionCounts(1, 0, 0, 1);
	}

	private void OnMouseDown ( ) {
		GameManager.Instance.GameState = GameState.ROLLING_DIE;
	}
}
