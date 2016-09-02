using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour {
	
	//AnimatorコンポーネントクラスのSetFloat、SetBool、SetTrigger関数を利用し、PlayerAnimatorControllerが持つパラメータの値を設定しています
	Animator animator;
	EnemyStatus status;
	Vector3 prePosition;
	AttackArea attackArea;
	bool isDown = false;//倒れるフラグ
	bool attacked = false;//攻撃終了フラグ

	public bool IsAttacked(){
		return attacked;
	}

	//アニメーションイベントで呼び出す関数、StartAttackHit()、EndAttackHit()、EndAttack()を作成
	//攻撃判定の有効/無効化を行う
	void StartAttackHit(){
		//Debug.Log ("StartAttackHit");
		attackArea.OnAttack();
	}
	void EndAttackHit(){
		//Debug.Log ("EndAttackHit");
		attackArea.OnAttackTermination();
	}
	void EndAttack(){
		attacked = true;
	}

	void Died(){//倒れるアニメーションの最後で、この関数を呼び出す。
		Debug.Log ("Died");
		Destroy (gameObject);
	}

	void Start () {
		animator = GetComponent<Animator> ();
		status = GetComponent<EnemyStatus> ();
		attackArea = GetComponentInChildren<AttackArea> ();

		prePosition = transform.position;
	}

	void Update () {
		//前回のUpdateからの移動量を計算して、Speedパラメータの値を設定している
		Vector3 deltaPosition = transform.position - prePosition;

		animator.SetFloat ("Speed", deltaPosition.magnitude / Time.deltaTime); //走る速さを調整


		if (attacked && !status.attacking) {
			attacked = false;
		}

		animator.SetBool ("Attacking", (!attacked && status.attacking)); //アニメーションの分岐判定

		//Enemystatus.diedがtrueになったとき、PlayerAnimatorController内のDownパラメータがtrueになり、倒れるアニメーションが再生される
		if (!isDown && status.died) {
			isDown = true;
			animator.SetTrigger ("Down");
		}
		prePosition = transform.position;
	}
}
