using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
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
	public IEnumerator PlaceRandomItemAt (int itemIndex) {
		// Get a random item prefab
		GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
		Transform itemPosition = itemPositions[itemIndex];

		// Spawn the item prefab in the scene at the corresponding item position
		Item randomItem = Instantiate(randomItemPrefab, itemPosition).GetComponent<Item>( );
		/// TODO: Tween position of item when it spawns

		currentItems[itemIndex] = randomItem;
		Debug.Log($"Generating random item: {randomItem.Name}");

		yield return null;
	}

	/// <summary>
	///		Get a list of random items from the current items list
	/// </summary>
	/// <param name="itemCount">The number of random items to get</param>
	/// <returns>A list of randomly selected items from the current items list. No two items will be the same</returns>
	public Item[ ] GetRandomItems (int itemCount) {
		return Utils.GetRandomUniqueArrayItems(currentItems, itemCount);
	}

	/// <summary>
	///		Switch the indices of two items
	/// </summary>
	/// <param name="item1">The first item to switch</param>
	/// <param name="item2">The second item to switch</param>
	/// <returns></returns>
	public IEnumerator SwapItems (Item item1, Item item2) {
		// Get the indices of the two inputted items in the current items list
		int item1Index = GetItemIndex(item1);
		int item2Index = GetItemIndex(item2);

		// Swap the transform positions of the items
		item1.transform.SetParent(itemPositions[item2Index], false);
		item2.transform.SetParent(itemPositions[item1Index], false);

		// Swap the item positions in the array
		currentItems[item1Index] = item2;
		currentItems[item2Index] = item1;

		yield return null;
	}

	/// <summary>
	///		Fill all of the empty item positions on the table with a new random item
	/// </summary>
	/// <returns></returns>
	public IEnumerator FillEmptyItemPositions ( ) {
		// Loop through the entire current items list
		for (int i = 0; i < currentItems.Length; i++) {
			// If there is an item that has not been set yet, place a new item
			if (currentItems[i] == null) {
				yield return PlaceRandomItemAt(i);
			}
		}

		yield return null;
	}

	/// <summary>
	///		Get the index of an item in the current item list
	/// </summary>
	/// <param name="item">The item to get the index of</param>
	/// <returns>The integer index</returns>
	public int GetItemIndex (Item item) {
		return Array.IndexOf(currentItems, item);
	}
}
