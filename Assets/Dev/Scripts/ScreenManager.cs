using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ScreenState {
	CHAT, POLL
}

public class ScreenManager : Singleton<ScreenManager> {
	[Header("References")]
	[SerializeField] private GameObject chatMessagePrefab;
	[SerializeField] private RectTransform chatMessageParent;
	[SerializeField] private RectTransform chatPollParent;
	[SerializeField] private Transform playerBarParent;
	[SerializeField] private Transform opponentBarParent;
	[SerializeField] private TextMeshPro viewerCountText;
	[SerializeField] private Sprite pollFilledSprite;
	[SerializeField] private Sprite pollOutlineSprite;
	[SerializeField] private Person player;
	[SerializeField] private Person opponent;
	[Header("Properties")]
	[SerializeField] private ScreenState _screenState;
	[SerializeField] private List<string> usernameList;
	[SerializeField] private List<string> messageList;
	[SerializeField] private float maxChatTimer;
	[SerializeField] private float maxViewerTimer;

	private float chatTimer;
	private float viewerTimer;

	public ScreenState ScreenState {
		get => _screenState;
		set {
			_screenState = value;

			// Enable or disable certain ui elements based on the new screen state
			chatMessageParent.gameObject.SetActive(_screenState == ScreenState.CHAT);
			chatPollParent.gameObject.SetActive(_screenState == ScreenState.POLL);

			// Run some functions based on the new screen state
			switch (_screenState) {
				case ScreenState.POLL:
					StartCoroutine(UpdateAudienceRating( ));
					break;
			}
		}
	}

	protected override void Awake ( ) {
		base.Awake( );

		usernameList = new List<string>( ) { "frank", "nick", "hannah" };
		messageList = new List<string>( ) { "this is a pretty long message, should probably wrap a couple of times", "wow!", "this chat feature is so cool!!!!!" };
	}

	private void Start ( ) {
		ScreenState = ScreenState.CHAT;

		// Set a random viewership value
		viewerCountText.text = $"{Random.Range(3000, 7000)}";
	}

	private void Update ( ) {
		// If the chat timer is less than or equal to 0, spawn a chat
		if (chatTimer <= 0) {
			SpawnChatMessage( );

			// Set a random chat time for the next chat to appear
			chatTimer = Random.Range(0f, maxChatTimer);

			float chatHeight = 0f;
			// Loop through all of the chat messages and remove the ones that go off the top of the screen
			for (int i = chatMessageParent.childCount - 1; i >= 0; i--) {
				// If the chat messages have not exceeded the bounds of the screen, increment the chat height counter until they do
				chatHeight += LayoutUtility.GetPreferredHeight(chatMessageParent.GetChild(i).GetComponent<RectTransform>( ));

				// If the chat height exceeds the bounds of the screen, then destroy the chat message object that overflows
				// Subtracting 2 because of the horizontal layout group element padding
				if (chatHeight > chatMessageParent.rect.height) {
					Destroy(chatMessageParent.GetChild(i).gameObject);
				}
			}
		}

		// If the viewer timer is 0, update the viewer counter on the screen
		if (viewerTimer <= 0) {
			int viewerCount = int.Parse(viewerCountText.text);
			viewerCount = Mathf.Clamp(viewerCount + Random.Range(-100, 100), 1000, 9999);
			viewerCountText.text = $"{viewerCount}";
			viewerTimer += maxViewerTimer;
		}

		// Update the values of the timer variables with how much time has passed since the last update call
		chatTimer -= Time.deltaTime;
		viewerTimer -= Time.deltaTime;
	}

	/// <summary>
	///		Create a new chat message on the audience chat screen
	/// </summary>
	private void SpawnChatMessage ( ) {
		// Get username from the list
		string username = usernameList[Random.Range(0, usernameList.Count)];
		if (Random.Range(0f, 1f) < 0.5f) {
			username += $"{Random.Range(0, 1000)}";
		}

		// Get message from the list
		string message = messageList[Random.Range(0, messageList.Count)];

		// Create the chat message text
		TextMeshPro chatMessageText = Instantiate(chatMessagePrefab, chatMessageParent).GetComponent<TextMeshPro>( );
		chatMessageText.text = $"<color=#FFE200><b>{username}</b></color>: {message}";
	}

	/// <summary>
	///		Update the audience poll rating bars
	/// </summary>
	/// <returns></returns>
	private IEnumerator UpdateAudienceRating ( ) {
		// Variables for the current player rating value which will be used to animate the bars
		int barCounter = 10;

		// Get lists for player and opponent rating bars
		SpriteRenderer[ ] playerBarList = playerBarParent.GetComponentsInChildren<SpriteRenderer>( );
		SpriteRenderer[ ] opponentBarList = opponentBarParent.GetComponentsInChildren<SpriteRenderer>( );

		// Set all bars to be invisible and to the right sprite
		foreach (SpriteRenderer playerBarSpriteRenderer in playerBarList) {
			playerBarSpriteRenderer.enabled = false;
			playerBarSpriteRenderer.sprite = player.AudienceRating >= opponent.AudienceRating ? pollFilledSprite : pollOutlineSprite;
		}
		foreach (SpriteRenderer opponentBarSpriteRenderer in opponentBarList) {
			opponentBarSpriteRenderer.enabled = false;
			opponentBarSpriteRenderer.sprite = player.AudienceRating <= opponent.AudienceRating ? pollFilledSprite : pollOutlineSprite;
		}

		// Loop and enable each bar from bottom to top
		while (barCounter > 0) {
			// Activate specific bars based on the player's and opponent's audience rating
			playerBarList[barCounter - 1].enabled = (barCounter >= 10 - player.AudienceRating);
			opponentBarList[barCounter - 1].enabled = (barCounter >= 10 - opponent.AudienceRating);

			barCounter--;
			// This means that the entire animation will take 2 seconds (0.2s x 10 bars)
			yield return new WaitForSeconds(0.2f);
		}

		// Wait a couple of seconds for the player to read the new values
		yield return new WaitForSeconds(2);

		// Return back to chat mode
		ScreenState = ScreenState.CHAT;
		yield return null;
	}
}
