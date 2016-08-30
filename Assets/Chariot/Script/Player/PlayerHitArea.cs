using UnityEngine;
using System.Collections;

public class PlayerHitArea : MonoBehaviour {

	void Damage(AttackArea.AttackInfo attackInfo){
		transform.root.SendMessage ("Damage", attackInfo);
	}
}

