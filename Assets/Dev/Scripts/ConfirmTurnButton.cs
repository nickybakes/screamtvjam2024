using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmTurnButton : MonoBehaviour {
	[Header("References")]
	[SerializeField] private TextMeshProUGUI confirmTurnText;

	private void Update ( ) {
		// This button can only be enabled when at least one die and one hand are selected
		confirmTurnText.text = (GameManager.Instance.IsPlayerTurn && GameManager.Instance.CheckForMinSelectionCounts(1, 0, 0, 1) ? "[ Confirm turn ]" : "");
	}

	/// <summary>
	///		Used for UI mouse event, when the button is clicked (duh)
	/// </summary>
	public void WhenClicked ( ) {
		GameManager.Instance.GameState = GameState.ROLL_DIE;
	}
}
