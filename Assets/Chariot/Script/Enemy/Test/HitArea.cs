using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {

	void Damage(BulletDamageArea.AttackInfo attackInfo){
		transform.root.SendMessage ("Damage", attackInfo);
	}
}
