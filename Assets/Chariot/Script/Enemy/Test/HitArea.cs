using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {

	//void Damage(BulletDamageArea.AttackInfo attackInfo){
	void Damage(AttackInfo attackInfo){ //BulletDamageAreaからAttackInfo()を受け取る
		transform.root.SendMessage ("Damage", attackInfo); //Wargに情報を送る
	}
}
