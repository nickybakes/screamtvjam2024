using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private List<GameObject> dicePrefabs;
	[Header("Properties")]
	[SerializeField] private int diceCount;
	[SerializeField] private float diceRadius;
	[SerializeField, Range(0, Mathf.PI * 2)] private float diceOffsetRadians;
	[Space]
	[SerializeField] private Die[ ] currentDice;

	public void OnDrawGizmos ( ) {
		Gizmos.color = Color.blue;

		for (int i = 0; i < diceCount; i++) {
			Gizmos.DrawCube(transform.GetChild(i).position, Vector3.one * 0.5f);
		}
	}

	private void OnValidate ( ) {
		// Make sure the number of dice positions is correct
		if (transform.childCount != diceCount) {
			Debug.LogWarning("Need to adjust the dice position count!");
			return;
		}

		// Make dice points in a circle on the table
		List<Vector3> dicePoints = Utils.GetSpacedPointsOnCircle(transform.position, diceRadius, diceOffsetRadians, diceCount);
		for (int i = 0; i < diceCount; i++) {
			transform.GetChild(i).position = dicePoints[i];
		}
	}

	private void Awake ( ) {
		OnValidate( );

		currentDice = new Die[diceCount];
	}

	public void PlaceRandomDieAt (int index) {
		GameObject randomDiePrefab = dicePrefabs[Random.Range(0, dicePrefabs.Count)];
		Transform diePosition = transform.GetChild(index);
		Die randomDie = Instantiate(randomDiePrefab, diePosition).GetComponent<Die>( );

		Debug.Log($"Generating random die: {randomDie.DieString}");
		currentDice[index] = randomDie;
	}
}
