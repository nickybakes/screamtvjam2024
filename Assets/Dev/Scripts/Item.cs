using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private string _name;

	public string Name => _name;
}
