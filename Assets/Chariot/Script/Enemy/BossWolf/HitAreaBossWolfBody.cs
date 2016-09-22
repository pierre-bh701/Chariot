using UnityEngine;
using System.Collections;

public class HitAreaBossWolfBody : MonoBehaviour {

	bool bodyHit = false;

	//void Damage(BulletDamageArea.AttackInfo attackInfo){
	void Damage(int attackInfo){ //BulletDamageAreaからAttackInfo()を受け取る
		transform.root.SendMessage ("Damage", attackInfo); //Wargに情報を送る
	}

	void OnTriggerEnter(){
		transform.root.SendMessage ("HitAreaHeadOff");
		transform.root.SendMessage ("HitAreaBodyOff");
	}
}
