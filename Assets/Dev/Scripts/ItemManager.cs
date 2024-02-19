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
}
