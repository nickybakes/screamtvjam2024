using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
	CREATING_BOARD, CHOOSING_DIE, CHOOSING_ITEM, ROLLING_DIE, CUTTING_FINGER
}

public class GameManager : Singleton<GameManager> {
	[Header("References")]
	[SerializeField] private Person player;
	[SerializeField] private Person opponent;
	[Header("Properties")]
	[SerializeField] private GameState _gameState;
	[SerializeField] private Person activePerson;
	[SerializeField] private List<Die> selectedDice;
	[SerializeField] private List<Item> selectedItems;
	[SerializeField] private List<Finger> selectedFingers;
	[SerializeField] private List<Hand> selectedHands;
	[Header("Flags")]
	[SerializeField] private bool _canSelectDice;
	[SerializeField] private bool _canSelectItems;
	[SerializeField] private bool _canSelectFingers;
	[SerializeField] private bool _canSelectHands;
	[SerializeField] private bool canSelectAnyFinger;
	[SerializeField] private bool canSelectAnyHand;

	/// <summary>
	///		The current state of the game
	/// </summary>
	public GameState GameState {
		get => _gameState;
		set {
			// Run some functions right before the game state is switched
			switch (_gameState) {
				case GameState.CHOOSING_DIE:
					DisableDieSelection( );
					DisableItemSelection( );
					DisableHandSelection( );

					break;
			}

			_gameState = value;
			Debug.Log($"GameState set to: {Enum.GetName(typeof(GameState), value)}");

			// Run some functions as soon as the game state is set
			switch (_gameState) {
				case GameState.CREATING_BOARD:
					// Create dice
					DiceManager.Instance.PlaceRandomDieAt(0);
					DiceManager.Instance.PlaceRandomDieAt(1);
					DiceManager.Instance.PlaceRandomDieAt(2);
					DiceManager.Instance.PlaceRandomDieAt(3);

					// Create items
					ItemManager.Instance.PlaceRandomItemAt(0);
					ItemManager.Instance.PlaceRandomItemAt(1);
					ItemManager.Instance.PlaceRandomItemAt(2);
					ItemManager.Instance.PlaceRandomItemAt(3);
					ItemManager.Instance.PlaceRandomItemAt(4);

					activePerson = player;

					GameState = GameState.CHOOSING_DIE;

					break;
				case GameState.CHOOSING_DIE:
					EnableDieSelection( );
					EnableItemSelection(itemCapacity: 2);
					EnableHandSelection( );

					break;
			}
		}
	}

	/// <summary>
	///		Whether or not dice can be selected
	/// </summary>
	public bool CanSelectDice { get => _canSelectDice; private set => _canSelectDice = value; }

	/// <summary>
	///		Whether or not items can be selected
	/// </summary>
	public bool CanSelectItems { get => _canSelectItems; private set => _canSelectItems = value; }

	/// <summary>
	///		Whether or not fingers can be selected
	/// </summary>
	public bool CanSelectFingers { get => _canSelectFingers; private set => _canSelectFingers = value; }

	/// <summary>
	///		Whether or not hands can be selected
	/// </summary>
	public bool CanSelectHands { get => _canSelectHands; private set => _canSelectHands = value; }

	protected override void Awake ( ) {
		base.Awake( );

		selectedDice = new List<Die>(1);
		selectedItems = new List<Item>(1);
		selectedFingers = new List<Finger>(1);
		selectedHands = new List<Hand>(1);
	}

	private void Start ( ) {
		GameState = GameState.CREATING_BOARD;
	}

	private void Update ( ) {
		switch (GameState) {
			case GameState.ROLLING_DIE:
				break;
		}
	}

	/// <summary>
	///		Select a die
	/// </summary>
	/// <param name="die">The die to select</param>
	public void SelectDie (Die die) {
		// If a die cannot be selected right now, return
		if (!CanSelectDice) {
			return;
		}

		if (selectedDice.Contains(die)) {
			// If the inputted die was already selected, unselect it
			selectedDice.Remove(die);
		} else if (die != null) {
			// If the number of selected dice has reached it capacity, remove the first one so a new one can be added
			if (selectedDice.Capacity == selectedDice.Count) {
				selectedDice.RemoveAt(0);
			}

			// If the die is not null, then add it to be selected
			selectedDice.Add(die);
		}
	}

	/// <summary>
	///		Select an item
	/// </summary>
	/// <param name="item">The item to select</param>
	public void SelectItem (Item item) {
		// If an item cannot be selected right now, return
		if (!CanSelectItems) {
			return;
		}

		if (selectedItems.Contains(item)) {
			// If the inputted item was already selected, unselect it
			selectedItems.Remove(item);
		} else if (item != null) {
			// If the number of selected items has reached it capacity, remove the first one so a new one can be added
			if (selectedItems.Capacity == selectedItems.Count) {
				selectedItems.RemoveAt(0);
			}

			// If the item is not null, then add it to be selected
			selectedItems.Add(item);
		}
	}

	/// <summary>
	///		Select a finger
	/// </summary>
	/// <param name="finger">The finger to select</param>
	public void SelectFinger (Finger finger) {
		// If a finger cannot be selected right now, return
		if (!CanSelectFingers) {
			return;
		}

		// If the active person cannot select any finger and the finger to be selected is not part of the active person, return
		if (!canSelectAnyFinger && finger.Person != activePerson) {
			return;
		}

		if (selectedFingers.Contains(finger)) {
			// If the inputted finger was already selected, unselect it
			selectedFingers.Remove(finger);
		} else if (finger != null) {
			// If the number of selected fingers has reached it capacity, remove the first one so a new one can be added
			if (selectedFingers.Capacity == selectedFingers.Count) {
				selectedFingers.RemoveAt(0);
			}

			// If the finger is not null, then add it to be selected
			selectedFingers.Add(finger);
		}
	}

	/// <summary>
	///		Select a hand
	/// </summary>
	/// <param name="hand">The hand to select</param>
	public void SelectHand (Hand hand) {
		// If a hand cannot be selected right now, return
		if (!CanSelectHands) {
			return;
		}

		// If the active person cannot select any hand and the hand to be selected is not part of the active person, return
		if (!canSelectAnyHand && hand.Person != activePerson) {
			return;
		}

		if (selectedHands.Contains(hand)) {
			// If the inputted hand was already selected, unselect it
			selectedHands.Remove(hand);
		} else if (hand != null) {
			// If the number of selected hands has reached it capacity, remove the first one so a new one can be added
			if (selectedHands.Capacity == selectedHands.Count) {
				selectedHands.RemoveAt(0);
			}

			// If the hand is not null, then add it to be selected
			selectedHands.Add(hand);
		}
	}

	/// <summary>
	///		Enable the ability for the active person to select dice
	/// </summary>
	/// <param name="diceCapacity">Set a new capacity of dice that can be selected at one time</param>
	private void EnableDieSelection (int diceCapacity = 1) {
		CanSelectDice = true;

		// Make sure the size of the selected dice list is within the inputted capacity
		while (diceCapacity < selectedDice.Count) {
			selectedDice.RemoveAt(0);
		}

		selectedDice.Capacity = diceCapacity;
	}

	/// <summary>
	///		Enable the ability for the active person to select items
	/// </summary>
	/// <param name="itemCapacity">Set a new capacity of items that can be selected at one time</param>
	private void EnableItemSelection (int itemCapacity = 1) {
		CanSelectItems = true;

		// Make sure the size of the selected items list is within the inputted capacity
		while (itemCapacity < selectedItems.Count) {
			selectedItems.RemoveAt(0);
		}

		selectedItems.Capacity = itemCapacity;
	}

	/// <summary>
	///		Enable the ability for the active person to select fingers
	/// </summary>
	/// <param name="anyFinger">Whether or not any finger, regardless of active person, can be selected</param>
	/// <param name="fingerCapacity">Set a new capacity of fingers that can be selected at one time</param>
	private void EnableFingerSelection (bool anyFinger = false, int fingerCapacity = 1) {
		CanSelectFingers = true;
		canSelectAnyFinger = anyFinger;

		// Make sure the size of the selected fingers list is within the inputted capacity
		while (fingerCapacity < selectedFingers.Count) {
			selectedFingers.RemoveAt(0);
		}

		selectedFingers.Capacity = fingerCapacity;
	}

	/// <summary>
	///		Enable the ability for the active person to select hands
	/// </summary>
	/// <param name="anyHand">Whether or not any hand, regardless of active person, can be selected</param>
	/// <param name="handCapacity">Set a new capacity of hands that can be selected at one time</param>
	private void EnableHandSelection (bool anyHand = false, int handCapacity = 1) {
		CanSelectHands = true;
		canSelectAnyHand = anyHand;

		// Make sure the size of the selected hands list is within the inputted capacity
		while (handCapacity < selectedHands.Count) {
			selectedHands.RemoveAt(0);
		}

		selectedHands.Capacity = handCapacity;
	}

	/// <summary>
	///		Disable the ability for the active person to select dice
	/// </summary>
	private void DisableDieSelection ( ) {
		CanSelectDice = false;
		selectedDice.Clear( );
	}

	/// <summary>
	///		Disable the ability for the active person to select items
	/// </summary>
	private void DisableItemSelection ( ) {
		CanSelectItems = false;
		selectedItems.Clear( );
	}

	/// <summary>
	///		Disable the ability for the active person to select fingers
	/// </summary>
	private void DisableFingerSelection ( ) {
		CanSelectFingers = false;
		selectedFingers.Clear( );
	}

	/// <summary>
	///		Disable the ability for the active person to select hands
	/// </summary>
	private void DisableHandSelection ( ) {
		CanSelectHands = false;
		selectedHands.Clear( );
	}

	/// <summary>
	///		Check for selection counts across all things that can be selected
	/// </summary>
	/// <param name="dieCount">The minimum die count that need to be selected</param>
	/// <param name="itemCount">The minimum item count that need to be selected</param>
	/// <param name="fingerCount">The minimum finger count that need to be selected</param>
	/// <param name="handCount">The minimum hand count that need to be selected</param>
	/// <returns>true if all of the inputted amounts are less than or equal to their respective selected object amounts, false otherwise</returns>
	public bool CheckForSelectionCounts (int dieCount, int itemCount, int fingerCount, int handCount) {
		return (
			selectedDice.Count >= dieCount &&
			selectedItems.Count >= itemCount &&
			selectedFingers.Count >= fingerCount &&
			selectedHands.Count >= handCount
		);
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