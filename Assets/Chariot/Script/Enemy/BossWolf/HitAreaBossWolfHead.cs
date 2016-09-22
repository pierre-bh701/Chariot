using UnityEngine;
using System.Collections;

public class HitAreaBossWolfHead : MonoBehaviour {

	//void Damage(BulletDamageArea.AttackInfo attackInfo){
	void Damage(int attackInfo){ //BulletDamageAreaからAttackInfo()を受け取る
		transform.root.SendMessage ("Damage", attackInfo*1.5f); //Wargに情報を送る
	}

	void OnTriggerEnter(){
		transform.root.SendMessage ("HitAreaHeadOff");
		transform.root.SendMessage ("HitAreaBodyOff");
	}
}
