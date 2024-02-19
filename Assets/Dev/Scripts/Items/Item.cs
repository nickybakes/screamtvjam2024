using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
	[Header("References")]
	[SerializeField] private BoxCollider mouseClickCollider;
	[Header("Properties")]
	[SerializeField] private string _name;

	/// <summary>
	///		The name of the item
	/// </summary>
	public string Name => _name;

	protected void Update ( ) {
		mouseClickCollider.enabled = GameManager.Instance.CanSelectItems;
	}

	protected void OnMouseDown ( ) {
		GameManager.Instance.SelectItem(this);
	}

	/// <summary>
	///		Use this item
	/// </summary>
	/// <returns></returns>
	public abstract IEnumerator Use ( );
}
