using UnityEngine;
using System.Collections;

public class EnemyAnimationBossWolf : MonoBehaviour {

	//AnimatorコンポーネントクラスのSetFloat、SetBool、SetTrigger関数を利用し、PlayerAnimatorControllerが持つパラメータの値を設定しています
	Animator animator;
	EnemyStatusBossWolf statusBW;
	Vector3 prePosition;
	AttackArea attackArea;
	AttackScratchArea attackScratchArea;
	AttackBiteArea attackBiteArea;
	bool isDead = false;//倒れるフラグ
	bool attacked = false;//攻撃終了フラグ
	public bool scratched = false;
	public bool bited = false;

	public bool IsAttacked(){
		return this.attacked;
	}

	//アニメーションイベントで呼び出す関数、StartAttackHit()、EndAttackHit()、EndAttack()を作成
	//攻撃判定の有効/無効化を行う
	void StartAttackHit(){
		attackArea.OnAttack();
	}
	void EndAttackHit(){
		attackArea.OnAttackTermination();
	}
	void EndAttack(){
		attacked = true;
	}
	void StartScratchHit(){
		attackScratchArea.OnAttack();
	}
	void EndScratchHit(){
		attackBiteArea.OnAttackTermination();
	}
	void StartBiteHit(){
		attackScratchArea.OnAttack();
	}
	void EndBiteHit(){
		attackBiteArea.OnAttackTermination();
	}
	void EndScratch(){
		this.scratched = true;
		animator.SetTrigger("Jumping");
	}
	void EndBite(){
		this.bited = true;
		animator.SetTrigger("Jumping");
	}

	void Died(){//倒れるアニメーションの最後で、この関数を呼び出す。
		Destroy (gameObject);
	}

	void Start () {
		animator = GetComponent<Animator> ();
		statusBW = GetComponent<EnemyStatusBossWolf> ();
		attackArea = GetComponentInChildren<AttackArea> ();
		attackScratchArea = GetComponentInChildren<AttackScratchArea> ();
		attackBiteArea = GetComponentInChildren<AttackBiteArea> ();

		prePosition = transform.position;
	}

	void Update () {
		//前回のUpdateからの移動量を計算して、Speedパラメータの値を設定している
		Vector3 deltaPosition = transform.position - prePosition;

		animator.SetFloat ("Speed", deltaPosition.magnitude / Time.deltaTime); //走る速さを調整


		if (attacked && !statusBW.attacking) {
			attacked = false;
		}

		animator.SetBool ("Attacking", (!attacked && statusBW.attacking)); //アニメーションの分岐判定

		if (!isDead && statusBW.died) {
			isDead = true;
			animator.SetTrigger ("Died");
		}
		prePosition = transform.position;
	}
}
