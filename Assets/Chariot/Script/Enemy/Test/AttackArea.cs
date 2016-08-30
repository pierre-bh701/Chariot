using UnityEngine;
using System.Collections;

public class AttackArea : MonoBehaviour {
	
	//ステータスの数値だけのスクリプト、ここから値を引っ張ってくる
	EnemyStatus status;
	//攻撃判定のコライダ
	Collider attackCollider;

	// Use this for initialization
	void Start () {
		//ステータスのスクリプトをオブジェクトの親から持ってくる
		status = transform.root.GetComponent<EnemyStatus> ();
		//コライダの取得
		attackCollider = GetComponent<Collider>();
	}

	public class AttackInfo{
		public int attackPower;//この攻撃の攻撃力
		public Transform attacker;//攻撃者
	}

	AttackInfo GetAttackInfo(){
		//攻撃力の計算
		AttackInfo attackInfo = new AttackInfo ();
		attackInfo.attackPower = status.Power;
		attackInfo.attacker = transform.root;

		return attackInfo;
	}

	//攻撃が当たった
	void OnTriggerEnter(Collider other){
		//Debug.Log ("Hit" + other.tag);
		//攻撃が当たった相手のDamageメッセージを送る
		if (other.tag == "Player") {
			other.SendMessage ("Damage", GetAttackInfo ());
		}
	}

	//攻撃判定を有効化
	public void OnAttack(){
		attackCollider.enabled = true;
	}

	//攻撃判定を無効化
	public void OnAttackTermination(){
		attackCollider.enabled = false;
	}

}
