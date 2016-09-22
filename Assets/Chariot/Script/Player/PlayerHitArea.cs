using UnityEngine;
using System.Collections;

public class PlayerHitArea : MonoBehaviour {

	void Damage(int power){
		transform.root.SendMessage ("Damage", power);
	}
}

