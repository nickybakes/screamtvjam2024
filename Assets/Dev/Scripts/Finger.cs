using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FingerType {
	THUMB, INDEX, MIDDLE, RING, PINKY
}

public class Finger : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private int _health;

	/// <summary>
	///		The current health of this finger
	/// </summary>
	public int Health { get => _health; set => _health = value; }
}