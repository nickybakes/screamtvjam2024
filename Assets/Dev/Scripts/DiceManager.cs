using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private List<GameObject> dicePrefabs;
	[Header("Properties")]
	[SerializeField] private int diceCount;
	[SerializeField] private float diceRadius;
	[SerializeField, Range(0f, 360f)] private float diceOffsetDegrees;
	[Space]
	[SerializeField] private Die[ ] _activeDice;

	public Die[ ] ActiveDice { get => _activeDice; private set => _activeDice = value; }

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
		List<Vector3> dicePoints = Utils.GetSpacedPointsOnCircle(transform.position, diceRadius, diceOffsetDegrees * Mathf.Deg2Rad, diceCount);
		for (int i = 0; i < diceCount; i++) {
			transform.GetChild(i).position = dicePoints[i];
		}
	}

	private void Awake ( ) {
		OnValidate( );

		ActiveDice = new Die[diceCount];
	}

	public void PlaceRandomDieAt (int index) {
		// Get a random die prefab
		GameObject randomDiePrefab = dicePrefabs[Random.Range(0, dicePrefabs.Count)];
		Transform diePosition = transform.GetChild(index);

		// Spawn the die prefab in the scene at the corresponding die position
		Die randomDie = Instantiate(randomDiePrefab, diePosition).GetComponent<Die>( );

		ActiveDice[index] = randomDie;
		randomDie.Index = index;
		Debug.Log($"Generating random die: {randomDie}");
	}
}
