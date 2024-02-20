using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameObject chatMessagePrefab;
	[SerializeField] private RectTransform chatMessageParent;
	[SerializeField] private RectTransform rectTransform;
	[Header("Properties")]
	[SerializeField] private List<string> usernameList;
	[SerializeField] private List<string> messageList;
	[SerializeField] private string usernameHexCode;
	[SerializeField] private float maxChatTimer;

	private float chatTimer;

	private void Awake ( ) {
		usernameList = new List<string>( ) { "frank", "nick", "hannah" };
		messageList = new List<string>( ) { "this is a pretty long message, should probably wrap a couple of times", "wow!", "this chat feature is so cool!!!!!" };
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

		chatTimer -= Time.deltaTime;
	}

	public void SpawnChatMessage ( ) {
		// Get username from the list
		string username = usernameList[Random.Range(0, usernameList.Count)];
		if (Random.Range(0f, 1f) < 0.5f) {
			username += $"{Random.Range(0, 1000)}";
		}

		// Get message from the list
		string message = messageList[Random.Range(0, messageList.Count)];

		// Create the chat message text
		TextMeshPro chatMessageText = Instantiate(chatMessagePrefab, chatMessageParent).GetComponent<TextMeshPro>( );
		chatMessageText.text = $"<color={usernameHexCode}><b>{username}</b></color>: {message}";
	}
}
