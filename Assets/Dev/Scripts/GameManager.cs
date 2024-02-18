using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
	CREATING_BOARD, CHOOSING_DIE, CHOOSING_ITEM, ROLLING_DIE, CUTTING_FINGER
}

public class GameManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Person player;
	[SerializeField] private Person opponent;
	[SerializeField] private DiceManager diceManager;
	[SerializeField] private ItemManager itemManager;
	[Space]
	[SerializeField] private Button confirmChoiceButton;
	[Header("Properties")]
	[SerializeField] private GameState _gameState;
	[SerializeField] private Person _activePerson;
	[SerializeField] private Die _selectedDie;
	[SerializeField] private Item _selectedItem;
	[SerializeField] private Finger _selectedFinger;
	[SerializeField] private Hand _selectedHand;
	[Header("Flags")]
	[SerializeField] private bool _canSelectDie;
	[SerializeField] private bool _canSelectItem;
	[SerializeField] private bool _canSelectFinger;
	[SerializeField] private bool _canSelectHand;
	[SerializeField] private bool canSelectAnyFinger;
	[SerializeField] private bool canSelectAnyHand;
	[Space]
	[SerializeField] private bool confirmChoice;

	/// <summary>
	///		The current state of the game
	/// </summary>
	public GameState GameState {
		get => _gameState;
		set {
			_gameState = value;

			switch (_gameState) {
				case GameState.CREATING_BOARD:
					StartCoroutine(CreateBoardState( ));
					break;
				case GameState.CHOOSING_DIE:
					StartCoroutine(ChoosingDieState( ));
					break;
				case GameState.ROLLING_DIE:
					StartCoroutine(RollingDieState( ));
					break;
				case GameState.CUTTING_FINGER:
					StartCoroutine(CuttingFingerState( ));
					break;
				case GameState.CHOOSING_ITEM:
					StartCoroutine(ChoosingItemState( ));
					break;
				default:
					Debug.LogWarning("There is no coroutine for the currently selected game state!");
					break;
			}
		}
	}

	/// <summary>
	///		The currently active person, or the person whose turn it is
	/// </summary>
	public Person ActivePerson { get => _activePerson; private set => _activePerson = value; }

	/// <summary>
	///		The currently selected die
	/// </summary>
	public Die SelectedDie {
		get => _selectedDie;
		set {
			// Always allow this value to be set to null
			if (value == null) {
				_selectedDie = value;
				return;
			}

			// If a die cannot currently be selected, return
			if (!CanSelectDie) {
				return;
			}

			_selectedDie = value;
		}
	}

	/// <summary>
	///		The currently selected item
	/// </summary>
	public Item SelectedItem {
		get => _selectedItem;
		set {
			// Always allow this value to be set to null
			if (value == null) {
				_selectedItem = value;
				return;
			}

			// If an item cannot currently be selected, return
			if (!CanSelectItem) {
				return;
			}

			_selectedItem = value;
		}
	}

	/// <summary>
	///		The currently selected finger
	/// </summary>
	public Finger SelectedFinger {
		get => _selectedFinger;
		set {
			// Always allow this value to be set to null
			if (value == null) {
				_selectedFinger = value;
				return;
			}

			// If a finger cannot currently be selected, return
			if (!CanSelectFinger) {
				return;
			}

			// If the player cannot select any finger and the finger is not part of the active person, return
			if (!canSelectAnyFinger && value.Person != ActivePerson) {
				return;
			}

			_selectedFinger = value;
		}
	}

	/// <summary>
	///		The currently selected hand
	/// </summary>
	public Hand SelectedHand {
		get => _selectedHand;
		set {
			// Always allow this value to be set to null
			if (value == null) {
				_selectedHand = value;
				return;
			}

			// If a hand cannot currently be selected, return
			if (!CanSelectHand) {
				return;
			}

			// If the player cannot select any hand and the hand is not part of the active person, return
			if (!canSelectAnyHand && value.Person != ActivePerson) {
				return;
			}

			_selectedHand = value;
		}
	}

	/// <summary>
	///		Whether or not a die can be selected
	/// </summary>
	public bool CanSelectDie { get => _canSelectDie; set => _canSelectDie = value; }

	/// <summary>
	///		Whether or not an item can be selected
	/// </summary>
	public bool CanSelectItem { get => _canSelectItem; set => _canSelectItem = value; }

	/// <summary>
	///		Whether or not a hand can be selected
	/// </summary>
	public bool CanSelectHand { get => _canSelectHand; set => _canSelectHand = value; }

	/// <summary>
	///		Whether or not a finger can be selected
	/// </summary>
	public bool CanSelectFinger { get => _canSelectFinger; set => _canSelectFinger = value; }

	private void OnValidate ( ) {
		diceManager = FindObjectOfType<DiceManager>( );
		itemManager = FindObjectOfType<ItemManager>( );
	}

	private void Awake ( ) {
		OnValidate( );

		// Set on click events for buttons
		confirmChoiceButton.onClick.AddListener(( ) => confirmChoice = true);
	}

	private void Start ( ) {
		GameState = GameState.CREATING_BOARD;
	}

	private void Update ( ) {
		switch (GameState) {
			case GameState.CHOOSING_DIE:
				// Only make the player's choice confirmable if they have selected a hand and a die
				confirmChoiceButton.interactable = SelectedDie != null && SelectedHand != null;

				break;
		}
	}

	private IEnumerator CreateBoardState ( ) {
		// Create dice
		diceManager.PlaceRandomDieAt(0);
		diceManager.PlaceRandomDieAt(1);
		diceManager.PlaceRandomDieAt(2);

		// Create items
		itemManager.PlaceRandomItemAt(0);
		itemManager.PlaceRandomItemAt(1);
		itemManager.PlaceRandomItemAt(2);
		itemManager.PlaceRandomItemAt(3);
		itemManager.PlaceRandomItemAt(4);

		ActivePerson = player;

		GameState = GameState.CHOOSING_DIE;

		yield return null;
	}

	private IEnumerator ChoosingDieState ( ) {
		// Enable selection flags
		CanSelectDie = true;
		CanSelectHand = true;

		// Wait for the active person to make a selection and confirm it
		yield return new WaitUntil(( ) => confirmChoice);
		confirmChoice = false;
		Debug.Log($"Die was selected: {SelectedDie}");
		Debug.Log($"Hand was selected: {SelectedHand.name}");

		ResetFlags( );

		GameState = GameState.ROLLING_DIE;

		yield return null;
	}

	private IEnumerator RollingDieState ( ) {
		// Roll the dice
		int dieValue = SelectedDie.Roll( );
		Debug.Log($"Die was rolled for a value of {dieValue}");

		// Remove the selected die from the list of dice and replace it with a new die
		diceManager.ActiveDice[SelectedDie.Index] = null;
		diceManager.PlaceRandomDieAt(SelectedDie.Index);
		Destroy(SelectedDie.gameObject);

		// Get the finger that corresponds to the die value
		// A die value of 6 does not map to a finger index but should still be used
		Finger finger = SelectedHand.GetFingerAt(dieValue - 1);
		Debug.Log($"There is {(finger == null ? "not" : "")} a finger at finger index {dieValue - 1}");

		ResetSelections( );

		// Determine what the player can do based on the die value
		if (finger != null || dieValue == 6) {
			// If the finger is not null or the die value was 6, then the active player gets to chop off a finger
			Debug.Log($"{ActivePerson.name} can chop off a finger");
			GameState = GameState.CUTTING_FINGER;
		} else {
			// If the finger is null, the the active person gets to choose an item
			Debug.Log($"{ActivePerson.name} can select an item");
			GameState = GameState.CHOOSING_ITEM;
		}

		yield return null;
	}

	private IEnumerator CuttingFingerState ( ) {
		// Enable selection flags
		CanSelectFinger = true;
		canSelectAnyFinger = true;

		// Wait for the active person to select a finger
		yield return new WaitUntil(( ) => SelectedFinger != null);
		Debug.Log($"{ActivePerson.name} chopped {SelectedFinger.Hand.name} {SelectedFinger.name}");

		// Cut the selected finger
		SelectedFinger.Cut( );

		ResetFlags( );
		ResetSelections( );

		GameState = GameState.CHOOSING_DIE;

		yield return null;
	}

	private IEnumerator ChoosingItemState ( ) {
		GameState = GameState.CHOOSING_DIE;

		yield return null;
	}

	private void ResetFlags ( ) {
		CanSelectDie = false;
		CanSelectItem = false;
		CanSelectFinger = false;
		CanSelectHand = false;
		canSelectAnyFinger = false;
		canSelectAnyHand = false;
	}

	private void ResetSelections ( ) {
		SelectedDie = null;
		SelectedItem = null;
		SelectedFinger = null;
		SelectedHand = null;
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