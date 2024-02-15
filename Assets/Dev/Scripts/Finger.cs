using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private int _heath;

	/// <summary>
	///		The current health of this finger
	/// </summary>
	public int Health { get => _heath; set => _heath = value; }
}