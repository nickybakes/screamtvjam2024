using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	[Header("References")]
	[SerializeField] private BoxCollider mouseClickCollider;
	[Header("Properties")]
	[SerializeField] private string _name;

	/// <summary>
	///		The name of the item
	/// </summary>
	public string Name => _name;

	private void Update ( ) {
		mouseClickCollider.enabled = GameManager.Instance.CanSelectItems;
	}

	private void OnMouseDown ( ) {
		GameManager.Instance.SelectItem(this);
	}
}
