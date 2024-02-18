using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceManager : Singleton<DiceManager> {
	[Header("References")]
	[SerializeField] private List<GameObject> dicePrefabs;
	[Header("Properties")]
	[SerializeField] private int diceCount;
	[SerializeField] private float diceLineLength;
	[Space]
	[SerializeField] private Die[ ] activeDice;

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
		List<Vector3> dicePoints = Utils.GetSpacedPointsOnLine(transform.position, diceLineLength, diceCount);
		for (int i = 0; i < diceCount; i++) {
			transform.GetChild(i).position = dicePoints[i];
		}
	}

	protected override void Awake ( ) {
		base.Awake( );

		OnValidate( );

		activeDice = new Die[diceCount];
	}

	/// <summary>
	///		Place a random die at a specific index
	/// </summary>
	/// <param name="dieIndex">The index to place a new die at</param>
	public void PlaceRandomDieAt (int dieIndex) {
		// Get a random die prefab
		GameObject randomDiePrefab = dicePrefabs[Random.Range(0, dicePrefabs.Count)];
		Transform diePosition = transform.GetChild(dieIndex);

		// Spawn the die prefab in the scene at the corresponding die position
		Die randomDie = Instantiate(randomDiePrefab, diePosition).GetComponent<Die>( );

		activeDice[dieIndex] = randomDie;
		Debug.Log($"Generating random die: {randomDie}");
	}
	
	/// <summary>
	///		Roll the inputted die
	/// </summary>
	/// <param name="die">The die to roll</param>
	/// <returns>The integer value that is the result of rolling the die</returns>
	public int RollDie (Die die) {
		return RollDieAt(Array.IndexOf(activeDice, die));
	}

	/// <summary>
	///		Roll the die at the inputted index
	/// </summary>
	/// <param name="dieIndex">The index of the die to roll</param>
	/// <returns>The integer value that is the result of rolling the die</returns>
	public int RollDieAt (int dieIndex) {
		// Roll the inputted die and get a die value
		int dieValue = activeDice[dieIndex].Roll( );

		// Remove the die from the active dice
		Destroy(activeDice[dieIndex]);
		activeDice[dieIndex] = null;

		// Generate a new die
		PlaceRandomDieAt(dieIndex);

		return dieValue;
	}
}
