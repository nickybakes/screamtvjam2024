using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceManager : Singleton<DiceManager> {
	[Header("References")]
	[SerializeField] private GameObject dicePrefab;
	[SerializeField] private List<Transform> dicePositions;
	[Header("Properties")]
	[SerializeField] private float diceLineLength;
	[Space]
	[SerializeField] private Die[ ] currentDice;

	public void OnDrawGizmos ( ) {
		Gizmos.color = Color.blue;
		for (int i = 0; i < dicePositions.Count; i++) {
			Gizmos.DrawCube(dicePositions[i].position, Vector3.one * 0.5f);
		}
	}

	private void OnValidate ( ) {
		// Make dice points in a circle on the table
		List<Vector3> dicePoints = Utils.GetSpacedPointsOnLine(transform.position, diceLineLength, dicePositions.Count);
		for (int i = 0; i < dicePositions.Count; i++) {
			dicePositions[i].position = dicePoints[i];
		}
	}

	protected override void Awake ( ) {
		base.Awake( );
		OnValidate( );

		currentDice = new Die[dicePositions.Count];
	}

	/// <summary>
	///		Place a random die at a specific index
	/// </summary>
	/// <param name="dieIndex">The index to place a new die at</param>
	public IEnumerator PlaceRandomDieAt (int dieIndex) {
		// If there is already a die at the specified index, destroy it so a new one can be spawned
		if (currentDice[dieIndex] != null) {
			Destroy(currentDice[dieIndex].gameObject);
			currentDice[dieIndex] = null;
		}

		// Spawn the die prefab in the scene at the corresponding die position
		Die randomDie = Instantiate(dicePrefab, dicePositions[dieIndex]).GetComponent<Die>( );

		currentDice[dieIndex] = randomDie;
		Debug.Log($"Generating random die: {randomDie}");

		yield return null;
	}

	/// <summary>
	///		Get a list of random dice from the current dice list
	/// </summary>
	/// <param name="diceCount">The number of random dice to get</param>
	/// <returns>A list of randomly selected dice from the current dice list. No two dice will be the same</returns>
	public Die[ ] GetRandomDice (int diceCount) {
		return Utils.GetRandomUniqueArrayItems(currentDice, diceCount);
	}

	/// <summary>
	///		Roll the inputted die
	/// </summary>
	/// <param name="die">The die to roll</param>
	/// <returns>The integer value that is the result of rolling the die</returns>
	public IEnumerator RollDie (Die die) {
		// Roll the inputted die and get a die value
		yield return die.Roll( );

		// Remove the die from the active dice
		currentDice[Array.IndexOf(currentDice, die)] = null;
		Destroy(die.gameObject);

		yield return null;
	}

	/// <summary>
	///		Fill all of the empty dice positions on the table with a new die
	/// </summary>
	/// <returns></returns>
	public IEnumerator FillDicePositions (bool overwriteCurrentDice = false) {
		// Loop through the entire current dice list
		for (int i = 0; i < currentDice.Length; i++) {
			// If there is a die that has not been set yet, place a new die
			if (overwriteCurrentDice || currentDice[i] == null) {
				yield return PlaceRandomDieAt(i);
			}
		}

		yield return null;
	}
}
