using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
	CREATING_BOARD, CHOOSING_DIE, CHOOSING_ITEM, ROLLING_DIE, CHOPPING_FINGER
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
	[Space]
	[SerializeField] private bool _canSelectDie;
	[SerializeField] private Die _selectedDie;
	[Space]
	[SerializeField] private bool _canSelectItem;
	[SerializeField] private Item _selectedItem;
	[Space]
	[SerializeField] private bool _canSelectFinger;
	[SerializeField] private Finger _selectedFinger;
	[Space]
	[SerializeField] private bool _canSelectHand;
	[SerializeField] private Hand _selectedHand;

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
	///		Whether or not a die can be selected
	/// </summary>
	public bool CanSelectDie { get => _canSelectDie; set => _canSelectDie = value; }

	/// <summary>
	///		The currently selected die
	/// </summary>
	public Die SelectedDie { get => _selectedDie; set => _selectedDie = value; }

	/// <summary>
	///		Whether or not an item can be selected
	/// </summary>
	public bool CanSelectItem { get => _canSelectItem; set => _canSelectItem = value; }

	/// <summary>
	///		The currently selected item
	/// </summary>
	public Item SelectedItem { get => _selectedItem; set => _selectedItem = value; }

	/// <summary>
	///		Whether or not a finger can be selected
	/// </summary>
	public bool CanSelectFinger { get => _canSelectFinger; set => _canSelectFinger = value; }

	/// <summary>
	///		The currently selected finger
	/// </summary>
	public Finger SelectedFinger { get => _selectedFinger; set => _selectedFinger = value; }

	/// <summary>
	///		Whether or not a hand can be selected
	/// </summary>
	public bool CanSelectHand { get => _canSelectHand; set => _canSelectHand = value; }

	/// <summary>
	///		The currently selected hand
	/// </summary>
	public Hand SelectedHand { get => _selectedHand; set => _selectedHand = value; }

	private void OnValidate ( ) {
		diceManager = FindObjectOfType<DiceManager>( );
		itemManager = FindObjectOfType<ItemManager>( );
	}

	private void Awake ( ) {
		OnValidate( );

		CanSelectDie = false;
		CanSelectItem = false;
		CanSelectFinger = false;
		CanSelectHand = false;
	}

	private void Start ( ) {
		GameState = GameState.CREATING_BOARD;
		// ActivePerson = player;
	}

	private IEnumerator CreateBoardState ( ) {
		// Create dice
		diceManager.PlaceRandomDieAt(0);
		diceManager.PlaceRandomDieAt(1);
		diceManager.PlaceRandomDieAt(2);

		// Create items
		itemManager.PlaceRandomDieAt(0);
		itemManager.PlaceRandomDieAt(1);
		itemManager.PlaceRandomDieAt(2);
		itemManager.PlaceRandomDieAt(3);
		itemManager.PlaceRandomDieAt(4);

		GameState = GameState.CHOOSING_DIE;

		yield return null;
	}

	private IEnumerator ChoosingDieState ( ) {
		// Allow the active person to select a dice
		CanSelectDie = true;

		// Wait for the active person to make a selection
		yield return new WaitUntil(( ) => SelectedDie != null);
		CanSelectDie = false;
		Debug.Log($"Die was selected: {SelectedDie}");

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
		SelectedDie = null;

		GameState = GameState.CHOOSING_DIE;

		yield return null;
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