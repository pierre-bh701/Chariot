using UnityEngine;
using System.Collections;

public class AttackScratchArea : MonoBehaviour {

	//ステータスの数値だけのスクリプト、ここから値を引っ張ってくる
	EnemyStatusBossWolf status;
	//攻撃判定のコライダ
	Collider attackCollider;
	int attackPower = 10;

	// Use this for initialization
	void Start () {
		//ステータスのスクリプトをオブジェクトの親から持ってくる
		status = transform.root.GetComponent<EnemyStatusBossWolf> ();
		//コライダの取得
		attackCollider = GetComponent<Collider>();
		attackCollider.enabled = false;
	}

	int GetAttackInfo(){
		return this.attackPower;
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
