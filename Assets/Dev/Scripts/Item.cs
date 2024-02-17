using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameManager gameManager;
	[Header("Properties")]
	[SerializeField] private string _name;

	/// <summary>
	///		The name of the item
	/// </summary>
	public string Name => _name;

	private void OnValidate ( ) {
		gameManager = FindObjectOfType<GameManager>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void OnMouseDown ( ) {
		// If an item cannot currently be selected, then return from this function
		if (!gameManager.CanSelectItem) {
			return;
		}

		gameManager.SelectedItem = this;
	}
}
