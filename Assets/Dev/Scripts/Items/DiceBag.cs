using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBag : Item {
	public override IEnumerator Use ( ) {
		// Overwrite all of the current dice on the board
		yield return DiceManager.Instance.FillDicePositions(overwriteCurrentDice: true);
	}
}
