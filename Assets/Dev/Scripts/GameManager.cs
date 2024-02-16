using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Person _player;
	[SerializeField] private Person _opponent;
	[Header("Properties")]
	[SerializeField] private bool _isPlayerTurn;

	/// <summary>
	///		A reference to the player
	/// </summary>
	public Person Player => _player;

	/// <summary>
	///		A reference to the opponent
	/// </summary>
	public Person Opponent { get => _opponent; private set => _opponent = value; }

	public bool IsPlayerTurn {
		get => _isPlayerTurn;
		private set {
			_isPlayerTurn = value;

			if (_isPlayerTurn) {
				Debug.Log("It is the player's turn.");
			} else {
				Debug.Log("It is the opponents turn.");
			}
		}
	}

	private void Start ( ) {
		IsPlayerTurn = true;
	}

	private void Update ( ) {
		if (IsPlayerTurn) {

		} else {

		}
	}
}

/*

Game setup:
- Three dice are placed on the board
- Five items are placed on the board
* The audience chooses a random number of fingers to chop off in the beginning of the game

During a turn:
- Person selects the die they want to roll and the hand they want to roll it with
- The person may also choose to swap the location of two items on the board
- The person then rolls the die. Each value on the die corresponds to a finger on the hand they rolled with
 - If the value on the die corresponds to a finger that is still attached OR is a six, the person is allowed to chop of one finger on one of their own hands or one of their opponents' hands
 - If the value on the die corresponds to a finger that has already been chopped off, the person is allowed to choose one item that corresponds to the positions of their missing fingers

In between turns:
- The audience has a random chance to take a poll to add or remove something from the game. These things could include:
 - Replacing one of the dice on the board
 - Replacing one of the items on the board
 - Removing one of your fingers/your opponents fingers
 - Adding one of your fingers/your opponents fingers back
- If there are no more items left on the board, 5 new items are placed down

 */ 