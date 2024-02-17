using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Hand : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Finger[ ] _fingers;
	[SerializeField] private Palm _palm;

	/// <summary>
	///		A list of the fingers on this hand
	/// </summary>
	public Finger[ ] Fingers { get => _fingers; private set => _fingers = value; }

	/// <summary>
	///		A references to the palm of this hand
	/// </summary>
	public Palm Palm { get => _palm; private set => _palm = value; }

	/// <summary>
	///		The number of fingers still on this hand
	/// </summary>
	public int FingerCount => Fingers.Count(finger => finger != null);
}