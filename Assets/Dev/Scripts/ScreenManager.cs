using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameObject chatMessagePrefab;
	[SerializeField] private Transform chatMessageParent;
	[SerializeField] private TextMeshPro chatBoxText;
	[SerializeField] private List<string> usernameList;
	[SerializeField] private List<string> messageList;
	[Header("Properties")]
	[SerializeField] private Vector2 screenSize;
	[SerializeField] private string usernameHexCode;
	[SerializeField] private float maxChatTimer;

	private float chatTimer;

	private void OnValidate ( ) {
		GetComponent<RectTransform>( ).sizeDelta = screenSize;
		GetComponent<SpriteRenderer>( ).size = screenSize;
	}

	private void Awake ( ) {
		OnValidate( );

		Debug.Log(GetComponentInChildren<TextMeshPro>( ).textInfo.pageCount);

		usernameList = new List<string>( ) { "frank", "nick", "hannah" };
		messageList = new List<string>( ) { "this is a pretty long message, should probably wrap a couple of times", "wow!", "this chat feature is so cool!!!!!" };
	}

	private void Update ( ) {
		// If the chat timer is less than or equal to 0, spawn a chat
		if (chatTimer <= 0) {
			SpawnChatMessage( );

			// Set a random chat time for the next chat to appear
			chatTimer = Random.Range(0f, maxChatTimer);

			if (chatBoxText.isTextOverflowing) {

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
