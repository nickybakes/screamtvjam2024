using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Die : MonoBehaviour {
	[Header("References")]
	[SerializeField] private BoxCollider mouseClickCollider;
	[SerializeField] private Material[ ] faceMaterials;
	[SerializeField] private Rigidbody dieRigidbody;
	[Header("Properties")]
	[SerializeField] private int[ ] faceValues;
	[SerializeField] private int minFaceValueSum;
	[SerializeField] private int _topFaceValue;
	[SerializeField] private float maxAngularVelocity;
	[SerializeField] private float maxLinearVelocity;

	/// <summary>
	///		The face value of this die's face that is facing upwards
	/// </summary>
	public int TopFaceValue { get => _topFaceValue; private set => _topFaceValue = value; }

	private void Start ( ) {
		faceValues = new int[6];

		int faceSum = 0;
		// Generate random face values for the die
		for (int i = 0; i < faceValues.Length; i++) {
			// Generate a number between 1 and 6
			int randomFaceValue = Random.Range(1, 7);
			faceValues[i] = randomFaceValue;
			faceSum += randomFaceValue;
		}

		// Make sure the sum of all the faces is above a minimum value
		while (faceSum < minFaceValueSum) {
			// Pick a random face and add 1 to its value
			faceValues[Random.Range(0, 6)]++;
			faceSum++;
		}

		/// TODO: Set face materials
		/// Face materials need to match a specific order of directions in order to accuratly get the top value of the die when it is rolled
		/// Faces will be applied in the order: up, forward, right, -forward (back), -right (left), -up (down)
	}

	private void Update ( ) {
		mouseClickCollider.enabled = GameManager.Instance.CanSelectDice;
	}

	private void OnMouseDown ( ) {
		GameManager.Instance.SelectDie(this);
	}

	/// <summary>
	///		Get a random value from this die
	/// </summary>
	/// <returns>A random integer face value on the die</returns>
	public IEnumerator Roll ( ) {
		// Generate random force vectors to apply to the die
		float linearX = Random.Range(-0.1f, 0.1f);
		float linearY = Random.Range(0f, 1f);
		float linearZ = Random.Range(-0.1f, 0.1f);
		Vector3 linearVector = new Vector3(linearX, linearY, linearZ).normalized * maxLinearVelocity;

		float angularX = Random.Range(-1f, 1f);
		float angularY = Random.Range(-1f, 1f);
		float angularZ = Random.Range(-1f, 1f);
		Vector3 angularVector = new Vector3(angularX, angularY, angularZ).normalized * maxAngularVelocity;

		// Instantly set the velocities of the die
		dieRigidbody.velocity = linearVector;
		dieRigidbody.angularVelocity = angularVector;

		// Wait until the die has landed on the table and is stationary
		yield return new WaitUntil(( ) => dieRigidbody.angularVelocity.magnitude < 0.01f && dieRigidbody.velocity.magnitude < 0.01f);

		// Get the face that is currently facing upwards
		float maxYValue = 0f;
		// Check top face
		if (transform.up.y > maxYValue) {
			maxYValue = transform.up.y;
			TopFaceValue = faceValues[0];
		}

		// Check front face
		if (transform.forward.y > maxYValue) {
			maxYValue = transform.forward.y;
			TopFaceValue = faceValues[1];
		}

		// Check right face
		if (transform.right.y > maxYValue) {
			maxYValue = transform.right.y;
			TopFaceValue = faceValues[2];
		}

		// Check rear face
		if (-transform.forward.y > maxYValue) {
			maxYValue = -transform.forward.y;
			TopFaceValue = faceValues[3];
		}

		// Check left face
		if (-transform.right.y > maxYValue) {
			maxYValue = -transform.right.y;
			TopFaceValue = faceValues[4];
		}

		// Check bottom face
		if (-transform.up.y > maxYValue) {
			maxYValue = -transform.up.y;
			TopFaceValue = faceValues[5];
		}

		yield return null;
	}

	public override string ToString ( ) {
		string dieString = "";
		for (int i = 0; i < faceValues.Length; i++) {
			dieString += faceValues[i];

			if (i < faceValues.Length - 1) {
				dieString += " ";
			}
		}

		return dieString;
	}
}
