using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
	CREATING_BOARD, CHOOSING_DIE, ROLLING_DIE, CHOPPING_FINGER
}

public class GameManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Person player;
	[SerializeField] private Person opponent;
	[SerializeField] private DiceManager diceManager;
	[SerializeField] private ItemManager itemManager;
	[Header("Properties")]
	[SerializeField] private GameState _gameState;
	[SerializeField] private Person _activePerson;
	[SerializeField] private int _selectedHandIndex;
	[SerializeField] private int _selectedDieIndex;
	[SerializeField] private Finger _selectedFinger;
	[SerializeField] private Hand _selectedHand;
	[Header("Test Fields")]
	[SerializeField] private List<GameObject> dicePrefabs;
	[SerializeField] private List<Transform> diceButtons;
	[SerializeField] private List<GameObject> itemPrefabs;
	[SerializeField] private List<Transform> itemButtons;
	[SerializeField] private Transform leftHandButton;
	[SerializeField] private Transform rightHandButton;
	[SerializeField] private TextMeshProUGUI eventText;
	[SerializeField] private TextMeshProUGUI rollText;

	/// <summary>
	///		The currently selected finger on the board
	/// </summary>
	public Finger SelectedFinger { get => _selectedFinger; set => _selectedFinger = value; }

	/// <summary>
	///		The currently selected hand on the board
	/// </summary>
	public Hand SelectedHand { get => _selectedHand; set => _selectedHand = value; }

	/// <summary>
	///		Whether or not it is the players turn
	/// </summary>
	public Person ActivePerson {
		get => _activePerson;
		private set {
			// If the turn in the game has changed, reset selected options
			if (_activePerson != value) {
				SelectedHandIndex = -1;
				SelectedDieIndex = -1;
			}

			_activePerson = value;
		}
	}

	/// <summary>
	///		The currently selected die index
	/// </summary>
	public int SelectedDieIndex {
		get => _selectedDieIndex;
		set {
			_selectedDieIndex = value;

			for (int i = 0; i < diceButtons.Count; i++) {
				diceButtons[i].GetComponent<Image>( ).color = (_selectedDieIndex == i ? Color.red : Color.white);
			}
		}
	}

	/// <summary>
	///		The currently selected hand
	/// </summary>
	public int SelectedHandIndex {
		get => _selectedHandIndex;
		set {
			_selectedHandIndex = value;

			leftHandButton.GetComponent<Image>( ).color = (_selectedHandIndex == 0 ? Color.red : Color.white);
			rightHandButton.GetComponent<Image>( ).color = (_selectedHandIndex == 1 ? Color.red : Color.white);
		}
	}

	public GameState GameState {
		get => _gameState;
		set {
			_gameState = value;

			switch (_gameState) {
				case GameState.CREATING_BOARD:
					break;
				case GameState.CHOOSING_DIE:
					break;
				case GameState.ROLLING_DIE:
					ConsumeDieAt(SelectedDieIndex);

					break;
				case GameState.CHOPPING_FINGER:


					break;
			}
		}
	}

	private void Awake ( ) {
	}

	private void Start ( ) {
		// GameState = GameState.CREATING_BOARD;
		// ActivePerson = player;
	}

	private void Update ( ) {
		switch (GameState) {
			case GameState.CREATING_BOARD:
				break;
			case GameState.CHOOSING_DIE:
				break;
			case GameState.ROLLING_DIE:
				break;
			case GameState.CHOPPING_FINGER:
				break;
		}
	}

	// NOTE: Some of these functions will probably need to be coroutines since some have animations that need to play with them
	// NOTE: For example, when placing a die, the die will need to float in from the side or something, meaning the game loop needs to wait for it to finish

	/// <summary>
	///		Consume a die at the specified index by rolling it
	/// </summary>
	/// <param name="index">The index of the die to roll</param>
	/// <returns>An integer value that is the random value on the die when it was rolled</returns>
	private void ConsumeDieAt (int index) {
		// Roll the die at the selected index to get a random value
		// int dieValue = currentDice[index].Roll( );

		// rollText.text = $"Roll: {dieValue}";
		// AddEventText($"{ActivePerson.name} rolls a {dieValue}.");

		// Remove the die that was used
		// currentDice[index] = null;

		// Get the finger at the dice roll value
		// Finger finger = ActivePerson.GetFingerAt(SelectedHandIndex, dieValue);

		/*
		// Determine what the player can do based on the dice roll
		if (finger != null) {
			AddEventText($"There is a finger at the die value, so {ActivePerson.name} gets to use an item.");
		} else {
			AddEventText($"There is not a finger at the die value, so {ActivePerson.name} gets to chop off a finger.");
		}
		*/

		// NOTE: Right now, the only thing that the player will be able to do is chop off a finger
		// AddEventText($"Cannot select items yet, so {ActivePerson.name} gets to chop off a finger.");
		GameState = GameState.CHOPPING_FINGER;
	}

	/// <summary>
	///		Consume an item by using it
	/// </summary>
	/// <param name="index">The index of the item to use</param>
	private void ConsumeItemAt (int index) {
		// Remove the item that was used
		// currentItems[index] = null;

		// There will be more code here to actually use the item
	}

	/// <summary>
	///		Confirm the die and hand selection for the specified person
	/// </summary>
	/// <param name="person">The person making the turn</param>
	public void ConfirmTurnSelection (Person person) {
		// If the die has not been selected yet, do not do this function yet
		// if (SelectedDieIndex < 0 || SelectedDieIndex >= currentDice.Length) {
		// 	return;
		// }

		// If the hand has not been selected yet, do not do this function yet
		if (SelectedHandIndex != 0 && SelectedHandIndex != 1) {
			return;
		}

		GameState = GameState.ROLLING_DIE;
	}

	/// <summary>
	///		Select a hand to roll the dice with
	/// </summary>
	/// <param name="handIndex">The hand index to select</param>
	public void SelectHand (int handIndex) {
		SelectedHandIndex = handIndex;
	}

	/// <summary>
	///		Select a die to roll
	/// </summary>
	/// <param name="dieIndex">The die index to select</param>
	public void SelectDie (int dieIndex) {
		SelectedDieIndex = dieIndex;
	}

	/// <summary>
	///		Select a finger on a hand
	/// </summary>
	/// <param name="handIndex">The hand index of the finger that was selected</param>
	/// <param name="fingerIndex">The finger index on that hand that was selected</param>
	public void SelectFinger ( ) {

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