using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Finger[ ] _fingers;

	/// <summary>
	///		A list of the fingers on this hand
	/// </summary>
	public Finger[ ] Fingers { get => _fingers; private set => _fingers = value; }

	/// <summary>
	///		The number of fingers still on this hand
	/// </summary>
	public int FingerCount => Fingers.Count(finger => finger != null);
}