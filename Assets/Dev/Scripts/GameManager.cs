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
					break;
				case GameState.ROLLING_DIE:
					break;
				case GameState.CHOPPING_FINGER:
					break;
			}
		}
	}

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

	/// <summary>
	///		The currently selected finger on the board
	/// </summary>
	public Finger SelectedFinger { get => _selectedFinger; set => _selectedFinger = value; }

	/// <summary>
	///		The currently selected hand on the board
	/// </summary>
	public Hand SelectedHand { get => _selectedHand; set => _selectedHand = value; }

	private void OnValidate ( ) {
		diceManager = FindObjectOfType<DiceManager>( );
		itemManager = FindObjectOfType<ItemManager>( );
	}

	private void Awake ( ) {
		OnValidate( );
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