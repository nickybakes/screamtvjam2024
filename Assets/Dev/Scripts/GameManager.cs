using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Person _player;
	[SerializeField] private Person _opponent;
	[Header("Properties")]
	[SerializeField] private bool _isPlayerTurn;
	[SerializeField] private HandType _selectedHandType;
	[SerializeField] private int _selectedDieIndex;
	[SerializeField] private Die[ ] currentDice;
	[SerializeField] private Item[ ] currentItems;
	[Header("Test")]
	[SerializeField] private List<GameObject> dicePrefabs;
	[SerializeField] private List<Transform> diceButtons;
	[SerializeField] private List<GameObject> itemPrefabs;
	[SerializeField] private List<Transform> itemButtons;
	[SerializeField] private Transform leftHandButton;
	[SerializeField] private Transform rightHandButton;
	[SerializeField] private TextMeshProUGUI eventText;
	[SerializeField] private TextMeshProUGUI rollText;

	/// <summary>
	///		A reference to the player
	/// </summary>
	public Person Player { get => _player; private set => _player = value; }

	/// <summary>
	///		A reference to the opponent
	/// </summary>
	public Person Opponent { get => _opponent; private set => _opponent = value; }

	/// <summary>
	///		Whether or not it is the players turn
	/// </summary>
	public bool IsPlayerTurn {
		get => _isPlayerTurn;
		private set {
			// If the turn in the game has changed, reset selected options
			if (_isPlayerTurn != value) {
				SelectedHandType = HandType.NONE;
				SelectedDieIndex = -1;
			}

			_isPlayerTurn = value;
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
	public HandType SelectedHandType {
		get => _selectedHandType;
		set {
			_selectedHandType = value;

			leftHandButton.GetComponent<Image>( ).color = (_selectedHandType == HandType.LEFT ? Color.red : Color.white);
			rightHandButton.GetComponent<Image>( ).color = (_selectedHandType == HandType.RIGHT ? Color.red : Color.white);
		}
	}

	private void Awake ( ) {
		currentDice = new Die[3];
		currentItems = new Item[5];
	}

	private void Start ( ) {
		// Generate starting dice
		PlaceRandomDie(0);
		PlaceRandomDie(1);
		PlaceRandomDie(2);

		// Generate starting items
		PlaceRandomItem(0);
		PlaceRandomItem(1);
		PlaceRandomItem(2);
		PlaceRandomItem(3);
		PlaceRandomItem(4);

		IsPlayerTurn = true;
	}

	private void Update ( ) {
		if (IsPlayerTurn) {
			// Player will be selecting hand and die to use
			// Player then hits the "confirm" button to roll the die
		} else {

		}
	}

	/// <summary>
	///		Place a random die on the board at the specified index
	/// </summary>
	/// <returns>The die object that was placed on the board</returns>
	private Die PlaceRandomDie (int index) {
		Die randomDie = dicePrefabs[Random.Range(0, dicePrefabs.Count)].GetComponent<Die>( );
		currentDice[index] = randomDie;

		// Temporary UI code
		diceButtons[index].GetComponentInChildren<TextMeshProUGUI>( ).text = "Select Die\n" + randomDie.DieString;

		return randomDie;
	}

	/// <summary>
	///		Place a random item on the board at the specified index
	/// </summary>
	/// <returns>The item object that was placed on the board</returns>
	private Item PlaceRandomItem (int index) {
		Item randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Count)].GetComponent<Item>( );
		currentItems[index] = randomItem;

		// Temporary UI code
		itemButtons[index].GetComponentInChildren<TextMeshProUGUI>( ).text = randomItem.Name;

		return randomItem;
	}

	/// <summary>
	///		TEMPORARY | Select a hand to roll the dice with.
	/// </summary>
	/// <param name="handType">The type of hand to select in integer form</param>
	public void SelectHand (int handType) {
		SelectedHandType = (HandType) handType;
	}

	/// <summary>
	///		TEMPORARY | Select a die to roll
	/// </summary>
	/// <param name="dieIndex">The die index on the board</param>
	public void SelectDie (int dieIndex) {
		SelectedDieIndex = dieIndex;
	}

	/// <summary>
	///		Confirm the die and hand selection for the specified person
	/// </summary>
	/// <param name="person">The person making the turn</param>
	public void ConfirmTurnSelection (Person person) {
		AddEventText($"{person.Name} chooses ({currentDice[SelectedDieIndex].DieString}) as die and rolls with their {(SelectedHandType == HandType.RIGHT ? "right" : "left")} hand.");

		// Roll the dice and get a dice value
		int dieValue = currentDice[SelectedDieIndex].Roll( );

		rollText.text = $"Roll: {dieValue}";

		AddEventText($"{person.Name} rolls a {dieValue}.");

		// Remove the dice that was used
		currentDice[SelectedDieIndex] = null;

		// Get the finger at the dice roll value
		Finger finger = person.GetFingerAt(SelectedHandType, dieValue);

		// Determine what the player can do based on the dice roll
		if (finger) {
			AddEventText($"There is a finger at the die value, so {person.Name} gets to use an item.");
		} else {
			AddEventText($"There is not a finger at the die value, so {person.Name} gets to chop off a finger.");
		}

		// Place a new die
		PlaceRandomDie(SelectedDieIndex);
	}

	private void AddEventText (string text) => eventText.text = "-> " + text + "\n" + eventText.text;
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