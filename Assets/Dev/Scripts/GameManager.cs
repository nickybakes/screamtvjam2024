using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum GameState {
	CHOOSE_DIE, CHOOSE_ITEM, ROLL_DIE, CUT_FINGER, END_TURN, GAME_OVER, GAME_WIN, AUDIENCE_POLL
}

public class GameManager : Singleton<GameManager> {
	[Header("References")]
	[SerializeField] private Person player;
	[SerializeField] private Person opponent;
	[SerializeField] private TextMeshProUGUI narratorText;
	[SerializeField] private Image narratorTextBackground;
	[Header("Properties")]
	[SerializeField] private int audiencePollTurnCount;
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
	[SerializeField] private bool _canSelectMissingFinger;
	[SerializeField] private bool canSelectAnyFinger;
	[SerializeField] private bool canSelectAnyHand;

	private int audiencePollTurnCounter;
	private Coroutine narratorTextCoroutine;

	/// <summary>
	///		The current state of the game
	/// </summary>
	public GameState GameState {
		get => _gameState;
		set {
			_gameState = value;
			Debug.Log($"GameState set to: {Enum.GetName(typeof(GameState), value)}");

			// Run some functions as soon as the game state is set
			switch (_gameState) {
				case GameState.END_TURN:
					StartCoroutine(HandleEndTurnState( ));
					break;
				case GameState.CHOOSE_DIE:
					StartCoroutine(HandleChooseDieState( ));

					if (activePerson == opponent) {
						StartCoroutine(HandleOpponentChooseDieState( ));
					}
					break;
				case GameState.ROLL_DIE:
					StartCoroutine(HandleRollDieState( ));
					break;
				case GameState.CUT_FINGER:
					StartCoroutine(HandleCutFingerState( ));

					if (activePerson == opponent) {
						StartCoroutine(HandleOpponentCutFingerState( ));
					}
					break;
				case GameState.CHOOSE_ITEM:
					StartCoroutine(HandleChooseItemState( ));

					if (activePerson == opponent) {
						StartCoroutine(HandleOpponentChooseItemState( ));
					}
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

	/// <summary>
	///		Whether or not the active person can select missing fingers or attached fingers
	/// </summary>
	public bool CanSelectMissingFinger { get => _canSelectMissingFinger; private set => _canSelectMissingFinger = value; }

	protected override void Awake ( ) {
		base.Awake( );

		selectedDice = new List<Die>(1);
		selectedItems = new List<Item>(1);
		selectedFingers = new List<Finger>(1);
		selectedHands = new List<Hand>(1);
	}

	private void Start ( ) {
		// Have the player go first
		activePerson = opponent;
		audiencePollTurnCounter = -1;
		GameState = GameState.END_TURN;
	}

	#region GameState Handler Functions

	/// <summary>
	///		Handle the end turn game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleEndTurnState ( ) {
		// Check to see if an audience poll needs to occur
		audiencePollTurnCounter++;
		if (audiencePollTurnCounter == audiencePollTurnCount) {
			audiencePollTurnCounter = 0;

			// Update the chat poll UI
			ScreenManager.Instance.ScreenState = ScreenState.POLL;
			yield return new WaitUntil(( ) => ScreenManager.Instance.ScreenState == ScreenState.CHAT);
		}

		// Fill up table with new items and dice
		yield return DiceManager.Instance.FillDicePositions( );
		yield return ItemManager.Instance.FillEmptyItemPositions( );

		// Switch the player who is the active person
		activePerson = (activePerson == player ? opponent : player);
		SetNarratorText(activePerson == player ? "Your turn" : "Opponent's turn");
		GameState = GameState.CHOOSE_DIE;

		yield return null;
	}

	/// <summary>
	///		Handle the choose die game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleChooseDieState ( ) {
		// Enable hand, die, and item selection
		EnableDieSelection( );
		EnableHandSelection( );
		EnableItemSelection(itemCapacity: 2);

		// The game state gets set once the confirm turn button is pushed
		yield return null;
	}

	/// <summary>
	///		Handle the roll die game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleRollDieState ( ) {
		// Disable selection of new objects but keep previously selected object references
		DisableDieSelection( );
		DisableHandSelection( );
		DisableItemSelection( );

		// If two items were selected, swap their places
		if (selectedItems.Count == 2) {
			yield return ItemManager.Instance.SwapItems(selectedItems[0], selectedItems[1]);
		}

		// Roll the inputted die and get a die value
		yield return DiceManager.Instance.RollDie(selectedDice[0]);
		int dieValue = selectedDice[0].TopFaceValue;
		Debug.Log($"Rolled a {dieValue}");

		// Get the finger at the rolled dice value on the selected hand
		// Need to subtract 1 from the die value to turn it into an index
		Finger rolledFinger = selectedHands[0].GetFingerAt(dieValue - 1);

		// Reset all references to the selection
		DisableDieSelection(clearSelectedDice: true);
		DisableHandSelection(clearSelectedHands: true);
		DisableItemSelection(clearSelectedItems: true);

		// Determine what the active person can do based on the die value
		// If the rolled finger is not null or the die value is 6, then the active person can chop off a finger
		// If the rolled finger is null, then the active person can choose an item to use
		if (rolledFinger != null || dieValue == 6) {
			GameState = GameState.CUT_FINGER;
		} else {
			GameState = GameState.CHOOSE_ITEM;
		}

		yield return null;
	}

	/// <summary>
	///		Handle the cutting finger game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleCutFingerState ( ) {
		// Wait until the active person selects a finger
		EnableFingerSelection(anyFinger: true);
		yield return new WaitUntil(( ) => selectedFingers.Count == 1);

		// Cut off the finger that is selected
		selectedFingers[0].Cut( );

		DisableFingerSelection(clearSelectedFingers: true);

		GameState = GameState.END_TURN;

		yield return null;
	}

	/// <summary>
	///		Handle the choose item game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleChooseItemState ( ) {
		// Wait until the active person selects an item
		EnableItemSelection( );
		yield return new WaitUntil(( ) => selectedItems.Count == 1);
		DisableItemSelection( );

		// Based on the item that was selected, perform that items specific actions
		switch (selectedItems[0].Name) {
			case "Bionic Finger":
				// Enable the selection of missing fingers
				// EnableFingerSelection(anyFinger: true, missingFinger: true);

				break;
			case "Dice Bag":
				// Overwrite all of the current dice on the board
				yield return DiceManager.Instance.FillDicePositions(overwriteCurrentDice: true);

				break;
			case "Hack Saw":
				// Have the active person select 2 hands
				// EnableHandSelection(anyHand: true, handCapacity: 2);
				// yield return new WaitUntil(( ) => selectedHands.Count == 2);

				// Swap the two hands

				break;
			case "Scissors":
				// Have the active person select a finger to cut
				EnableFingerSelection(anyFinger: true);
				yield return new WaitUntil(( ) => selectedFingers.Count == 1);

				// Partially cut the finger that was selected
				selectedFingers[0].PartialCut( );

				DisableFingerSelection(clearSelectedFingers: true);

				break;
			case "Ad Break":
				// Increase the active person's rating
				ScreenManager.Instance.PlayerAudienceRating += (activePerson == player ? 2 : -2);

				break;
		}

		DisableItemSelection(clearSelectedItems: true);

		GameState = GameState.END_TURN;

		yield return null;
	}

	#endregion

	#region Opponent Handler Functions

	/// <summary>
	///		Handle the opponent's actions during the choose die game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleOpponentChooseDieState ( ) {
		// Wait a couple of seconds to make the opponent seem more human
		yield return new WaitForSeconds(Random.Range(1f, 2.5f));

		// Select random dice and hand
		SelectDie(DiceManager.Instance.GetRandomDice(1)[0]);
		SelectHand(opponent.GetRandomHand( ));

		// Have a 50% chance to swap two items around
		if (Random.Range(0f, 1f) < 1f) {
			Item[ ] randomItems = ItemManager.Instance.GetRandomItems(2);
			SelectItem(randomItems[0]);
			SelectItem(randomItems[1]);
		}

		// Wait a couple of seconds to make the opponent seem more human
		yield return new WaitForSeconds(Random.Range(0.5f, 1f));

		GameState = GameState.ROLL_DIE;

		yield return null;
	}

	/// <summary>
	///		Handle the opponent's actions during the cut finger game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleOpponentCutFingerState ( ) {
		// Wait a couple of seconds to make the opponent seem more human
		yield return new WaitForSeconds(Random.Range(1f, 2.5f));

		// Give the opponent an 85% chance to select the player when chopping off a finger, otherwise chop off one of their own
		Person randomPerson = (Random.Range(0f, 1f) < 0.85f ? player : opponent);

		// Select a random attached finger on whatever person was chosen
		SelectFinger(randomPerson.GetRandomFingers(1)[0]);

		yield return null;
	}

	/// <summary>
	///		Handle the opponent's actions during the choose item game state
	/// </summary>
	/// <returns></returns>
	private IEnumerator HandleOpponentChooseItemState ( ) {
		// Wait a couple of seconds to make the opponent seem more human
		yield return new WaitForSeconds(Random.Range(1f, 2.5f));

		// Select a random item
		SelectItem(ItemManager.Instance.GetRandomItems(1)[0]);

		yield return null;
	}

	#endregion

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
	private void EnableFingerSelection (bool anyFinger = false, int fingerCapacity = 1, bool missingFinger = false) {
		CanSelectFingers = true;
		canSelectAnyFinger = anyFinger;
		CanSelectMissingFinger = missingFinger;

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
	/// <param name="clearSelectedDice">Whether or not to clear the currently selected dice</param>
	private void DisableDieSelection (bool clearSelectedDice = false) {
		CanSelectDice = false;

		if (clearSelectedDice) {
			selectedDice.Clear( );
		}
	}

	/// <summary>
	///		Disable the ability for the active person to select items
	/// </summary>
	/// <param name="clearSelectedItems">Whether or not to clear the currently selected items</param>
	private void DisableItemSelection (bool clearSelectedItems = false) {
		CanSelectItems = false;

		if (clearSelectedItems) {
			selectedItems.Clear( );
		}
	}

	/// <summary>
	///		Disable the ability for the active person to select fingers
	/// </summary>
	/// <param name="clearSelectedFingers">Whether or not to clear the currently selected fingers</param>
	private void DisableFingerSelection (bool clearSelectedFingers = false) {
		CanSelectFingers = false;
		canSelectAnyFinger = false;

		if (clearSelectedFingers) {
			selectedFingers.Clear( );
		}
	}

	/// <summary>
	///		Disable the ability for the active person to select hands
	/// </summary>
	/// <param name="clearSelectedHands">Whether or not to clear the currently selected hands</param>
	private void DisableHandSelection (bool clearSelectedHands = false) {
		CanSelectHands = false;
		canSelectAnyHand = false;

		if (clearSelectedHands) {
			selectedHands.Clear( );
		}
	}

	/// <summary>
	///		Check for selection counts across all things that can be selected
	/// </summary>
	/// <param name="dieCount">The minimum die count that need to be selected</param>
	/// <param name="itemCount">The minimum item count that need to be selected</param>
	/// <param name="fingerCount">The minimum finger count that need to be selected</param>
	/// <param name="handCount">The minimum hand count that need to be selected</param>
	/// <returns>true if all of the inputted amounts are less than or equal to their respective selected object amounts, false otherwise</returns>
	public bool CheckForMinSelectionCounts (int dieCount, int itemCount, int fingerCount, int handCount) {
		return (
			selectedDice.Count >= dieCount &&
			selectedItems.Count >= itemCount &&
			selectedFingers.Count >= fingerCount &&
			selectedHands.Count >= handCount
		);
	}

	/// <summary>
	///		Set the narrator text on screen
	/// </summary>
	/// <param name="text">The text to display on screen</param>
	public void SetNarratorText (string text = "") {
		// If the coroutine is not null, stop it so it can be restarted
		if (narratorTextCoroutine != null) {
			StopCoroutine(narratorTextCoroutine);
			narratorTextCoroutine = null;
		}

		narratorTextCoroutine = StartCoroutine(NarratorTextProcess(text));
	}

	/// <summary>
	///		Animation process for the narration text
	/// </summary>
	/// <param name="text">The text to display on screen</param>
	/// <returns></returns>
	private IEnumerator NarratorTextProcess (string text = "") {
		// Only do this loop if the text is not empty
		if (text.Length > 0) {
			narratorText.text = text;

			// Reset alpha
			Color narratorTextColor = narratorText.color;
			Color narratorTextBackgroundColor = narratorTextBackground.color;

			narratorTextColor.a = 1f;
			narratorText.color = narratorTextColor;
			narratorTextBackgroundColor.a = 1f;
			narratorTextBackground.color = narratorTextBackgroundColor;

			// Have the text stay on screen for a little bit
			yield return new WaitForSeconds(3f);

			// Slowly decrease the alpha of the text
			float alpha = 1f;
			while (alpha > 0f) {
				// Set the color alpha
				narratorTextColor.a = alpha;
				narratorText.color = narratorTextColor;
				narratorTextBackgroundColor.a = alpha;
				narratorTextBackground.color = narratorTextBackgroundColor;

				// This fade will take 1 second
				alpha -= 0.02f;
				yield return new WaitForSeconds(0.02f);
			}

			// Make sure alpha is all the way at 0
			narratorTextColor.a = 0f;
			narratorTextBackgroundColor.a = 0f;
			narratorText.text = "";
		}

		yield return null;
	}
}