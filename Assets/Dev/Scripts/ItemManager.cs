using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private List<GameObject> itemPrefabs;
	[Header("Properties")]
	[SerializeField] private int itemCount;
	[SerializeField] private float itemRadius;
	[SerializeField, Range(0, Mathf.PI * 2)] private float itemOffsetRadians;
	[Space]
	[SerializeField] private Item[ ] currentItems;

	public void OnDrawGizmos ( ) {
		Gizmos.color = Color.red;

		for (int i = 0; i < itemCount; i++) {
			Gizmos.DrawSphere(transform.GetChild(i).position, 0.25f);
		}
	}

	private void OnValidate ( ) {
		// Make sure the number of dice positions is correct
		if (transform.childCount != itemCount) {
			Debug.LogWarning("Need to adjust the item position count!");
			return;
		}

		// Make dice points in a circle on the table
		List<Vector3> dicePoints = Utils.GetSpacedPointsOnCircle(transform.position, itemRadius, itemOffsetRadians, itemCount);
		for (int i = 0; i < itemCount; i++) {
			transform.GetChild(i).position = dicePoints[i];
		}
	}

	private void Awake ( ) {
		OnValidate( );

		currentItems = new Item[itemCount];
	}

	public void PlaceRandomDieAt (int index) {
		GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
		Transform itemPosition = transform.GetChild(index);
		Item randomItem = Instantiate(randomItemPrefab, itemPosition).GetComponent<Item>( );

		Debug.Log($"Generating random item: {randomItem.Name}");
		currentItems[index] = randomItem;
	}
}
