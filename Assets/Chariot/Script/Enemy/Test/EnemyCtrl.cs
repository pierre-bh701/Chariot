using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
	EnemyStatus status;
	EnemyAnimation enemyAnimation;
	EnemyMove enemyMove;
	Transform attackTarget; //攻撃対象の座標
	// 待機時間
	public float waitBaseTime = 2.0f;
	// 残り待機時間
	float waitTime;
	// 移動範囲
	public float walkRange = 4.0f;
	// 初期位置を保存しておく変数
	public Vector3 basePosition;

	//攻撃範囲
	public float attackRange = 0.2f;

	// ステートの種類.
	enum State {
		Walking,	// 探索
		Chasing,	// 追跡
		Attacking,	// 攻撃
		Died,       // 死亡
	};

	State state = State.Walking;		// 現在のステート.
	State nextState = State.Walking;	// 次のステート.


	// Use this for initialization
	void Start () {
		status = GetComponent<EnemyStatus>();
		enemyAnimation = GetComponent<EnemyAnimation>();
		enemyMove = GetComponent<EnemyMove>(); 
		// 初期位置を保持
		basePosition = transform.position;
		// 待機時間
		waitTime = waitBaseTime;
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.Walking:
			Walking();
			break;
		case State.Chasing:
			Chasing();
			break;
		case State.Attacking:
			Attacking();
			break;
		}

		if (state != nextState)
		{
			state = nextState;
			switch (state) {
			case State.Walking:
				WalkStart();
				break;
			case State.Chasing:
				ChaseStart();
				break;
			case State.Attacking:
				AttackStart();
				break;
			case State.Died:
				Died();
				break;
			}
		}
	}


	// ステートを変更する.
	void ChangeState(State nextState)
	{
		this.nextState = nextState;
	}

	//探索、徘徊状態に入る前に実行
	void WalkStart()
	{
		StateStartCommon();
	}

	//探索、徘徊
	void Walking()
	{
		// 待機時間がまだあったら
		if (waitTime > 0.0f)
		{
			// 待機時間を減らす
			waitTime -= Time.deltaTime;
			// 待機時間が無くなったら
			if (waitTime <= 0.0f)
			{
				// 範囲内の何処か
				Vector2 randomValue = Random.insideUnitCircle * walkRange;
				// 移動先の設定
				Vector3 destinationPosition = basePosition + new Vector3(randomValue.x, 0.0f, randomValue.y);
				//　目的地の指定.
				SendMessage("SetDestination", destinationPosition);
			}
		}
		else
		{
			// 目的地へ到着
			if (enemyMove.Arrived())
			{
				// 待機状態へ
				waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
			}
			// ターゲットを発見したら追跡
			if (attackTarget)
			{
				ChangeState(State.Chasing);
			}
		}
	}

	// 追跡開始
	void ChaseStart()
	{
		StateStartCommon();//このスクリプトの下の方にあります
	}
	// 追跡中
	void Chasing()
	{
		// 移動先をプレイヤーに設定
		SendMessage("SetDestination", attackTarget.position);
		// 攻撃範囲に入ったら攻撃に移る
		if (Vector3.Distance( attackTarget.position, transform.position ) <= attackRange)
		{
			ChangeState(State.Attacking);
		}
	}

	// 攻撃ステートが始まる前に呼び出される.
	void AttackStart()
	{
		StateStartCommon();
		status.attacking = true;

		// 敵をこちらに向かせる.EnemyMoveスクリプト
		Vector3 targetDirection = (attackTarget.position-transform.position).normalized;
		enemyMove.SetDiection (targetDirection);
		//SendMessage("SetDirection",targetDirection);

		// 移動を止める.EnemyMoveスクリプト（攻撃の瞬間だけ一瞬止まる）
		enemyMove.StopMove();
		//SendMessage("StopMove");
	}

	// 攻撃後の処理.
	void Attacking()
	{
		if (enemyAnimation.IsAttacked())
			ChangeState(State.Walking);
		// 待機時間を再設定
		waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
		// ターゲットをリセットする
		attackTarget = null;
	}

	//体力0時に呼び出されるState
	void Died()
	{
		status.died = true;
		Destroy(gameObject);//倒れるアニメーションが無い為(ある場合は、Enemystatus経由でEnemyAnimationへ、倒れるアニメーション後にEnemy消失)
	}

	//BulletDamageAreaスクリプトから、HitArea経由でダメージを引っ張ってくる
	void Damage(BulletDamageArea.AttackInfo attackInfo)
	{
		status.HP -= attackInfo.bulletPower;//当たった弾の攻撃力分HPを減らす
		if (status.HP <= 0) {
			status.HP = 0;
			// 体力０なので死亡
			ChangeState(State.Died);
		}
	}

	// ステートが始まる前にステータスを初期化する.
	void StateStartCommon(){
		status.attacking = false;
		status.died = false;
	}

	// 攻撃対象を設定する
	public void SetAttackTarget(Transform target){
		attackTarget = target;
	}
}
