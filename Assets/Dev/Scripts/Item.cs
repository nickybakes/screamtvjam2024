using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameManager gameManager;
	[Header("Properties")]
	[SerializeField] private string _name;
	[SerializeField] private int _index;

	/// <summary>
	///		The name of the item
	/// </summary>
	public string Name => _name;

	/// <summary>
	///		The index of this die in the dice manager list
	/// </summary>
	public int Index { get => _index; set => _index = value; }

	private void OnValidate ( ) {
		gameManager = FindObjectOfType<GameManager>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void OnMouseDown ( ) {
		gameManager.SelectedItem = this;
	}
}
