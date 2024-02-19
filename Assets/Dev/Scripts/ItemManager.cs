using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : Singleton<ItemManager> {
	[Header("References")]
	[SerializeField] private List<GameObject> itemPrefabs;
	[SerializeField] private List<Transform> itemPositions;
	[Header("Properties")]
	[SerializeField] private float itemLineLength;
	[Space]
	[SerializeField] private Item[ ] currentItems;

	public void OnDrawGizmos ( ) {
		Gizmos.color = Color.red;
		for (int i = 0; i < itemPositions.Count; i++) {
			Gizmos.DrawSphere(itemPositions[i].position, 0.25f);
		}
	}

	private void OnValidate ( ) {
		// Make item points in a line on the table
		List<Vector3> itemPoints = Utils.GetSpacedPointsOnLine(transform.position, itemLineLength, itemPositions.Count);
		for (int i = 0; i < itemPositions.Count; i++) {
			itemPositions[i].position = itemPoints[i];
		}
	}

	protected override void Awake ( ) {
		base.Awake( );
		OnValidate( );

		currentItems = new Item[itemPositions.Count];
	}

	/// <summary>
	///		Place a random item at a specific index
	/// </summary>
	/// <param name="itemIndex">The index to place a new item at</param>
	public void PlaceRandomItemAt (int itemIndex) {
		// Get a random item prefab
		GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
		Transform itemPosition = itemPositions[itemIndex];

		// Spawn the item prefab in the scene at the corresponding item position
		Item randomItem = Instantiate(randomItemPrefab, itemPosition).GetComponent<Item>( );

		currentItems[itemIndex] = randomItem;
		Debug.Log($"Generating random item: {randomItem.Name}");
	}

	public void SwapItems (Item item1, Item item2) {
		// Get the indices of the two inputted items in the current items list
		int item1Index = Array.IndexOf(currentItems, item1);
		int item2Index = Array.IndexOf(currentItems, item2);

		// Swap the transform positions of the items
		item1.transform.SetParent(itemPositions[item2Index], false);
		item2.transform.SetParent(itemPositions[item1Index], false);

		// Swap the item positions in the array
		currentItems[item1Index] = item2;
		currentItems[item2Index] = item1;
	}
}
