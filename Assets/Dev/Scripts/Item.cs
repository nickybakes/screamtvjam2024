using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private string _name;
	[SerializeField] private string _description;

	/// <summary>
	///		The name of the item
	/// </summary>
	public string Name => _name;

	/// <summary>
	///		The description of the item
	/// </summary>
	public string Description => _description;

	protected void OnMouseDown ( ) {
		GameManager.Instance.SelectItem(this);
	}
}
