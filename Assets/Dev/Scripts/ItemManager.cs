using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : Singleton<ItemManager> {
	[Header("References")]
	[SerializeField] private List<GameObject> itemPrefabs;
	[Header("Properties")]
	[SerializeField] private int itemCount;
	[SerializeField] private float itemLineLength;
	[Space]
	[SerializeField] private Item[ ] currentItems;

	public void OnDrawGizmos ( ) {
		Gizmos.color = Color.red;

		for (int i = 0; i < itemCount; i++) {
			Gizmos.DrawSphere(transform.GetChild(i).position, 0.25f);
		}
	}

	private void OnValidate ( ) {
		// Make sure the number of item positions is correct
		if (transform.childCount != itemCount) {
			Debug.LogWarning("Need to adjust the item position count!");
			return;
		}

		// Make item points in a line on the table
		List<Vector3> itemPoints = Utils.GetSpacedPointsOnLine(transform.position, itemLineLength, itemCount);
		for (int i = 0; i < itemCount; i++) {
			transform.GetChild(i).position = itemPoints[i];
		}
	}

	private void Awake ( ) {
		OnValidate( );

		currentItems = new Item[itemCount];
	}

	/// <summary>
	///		Place a random item at a specific index
	/// </summary>
	/// <param name="itemIndex">The index to place a new item at</param>
	public void PlaceRandomItemAt (int itemIndex) {
		// Get a random item prefab
		GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
		Transform itemPosition = transform.GetChild(itemIndex);

		// Spawn the item prefab in the scene at the corresponding item position
		Item randomItem = Instantiate(randomItemPrefab, itemPosition).GetComponent<Item>( );

		currentItems[itemIndex] = randomItem;
		Debug.Log($"Generating random item: {randomItem.Name}");
	}
}
