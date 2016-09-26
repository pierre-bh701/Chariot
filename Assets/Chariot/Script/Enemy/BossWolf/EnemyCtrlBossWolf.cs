using UnityEngine;
using System.Collections;

public class EnemyCtrlBossWolf : MonoBehaviour {

	GameObject uiManagement;
	UIManager uiManager;

	Animator animator;
	EnemyStatusBossWolf statusBW;
	EnemyAnimationBossWolf animationBW;
	EnemyMoveBossWolf moveBW;
	SearchAreaBossWolfScratch scratchArea;
	GameObject chariot;
	Transform attackTarget;
	public BoxCollider attackAreaJaw;
	public BoxCollider attackAreaClaw;
	public BoxCollider hitAreaBody;
	public BoxCollider hitAreaHead;
	int downSwitchHpDif = 315;
	// 待機時間
	public float waitBaseTime = 10.0f; //3.0f
	// 残り待機時間
	float waitTime;
	// 移動範囲
	public float walkRange = 4.0f;
	// 初期位置を保存しておく変数
	//public Vector3 basePosition;
	int temporaryHP;

	Vector3 destination;

	//攻撃範囲
	public float searchScratchRange = 15f;

	int scratchStep = 0;
	int biteStep = 0;
	int chaseScratchStep = 0;
	int crossStep = 0;
	int howlingStep = 0;
	int downStep = 0;

	// ステートの種類.
	enum State {
		Walking,	// 探索
		Chasing,	// 追跡
		Attacking,	// 攻撃

		Running,         //ステージ脇を走る
		Biting,          //待ち伏せして噛みつき
		Scratching,      //ステージ脇から引っ掻き
		ChaseScratching, //追走後引っ掻き
		Crossing,        //ステージ逆サイドへジャンプ
		Howling,         //遠吠え
		Downing,         //攻撃を受けて怯む
		Died,            // 死亡
	};

	State state = State.Running;		// 現在のステート.
	State nextState = State.Running;	// 次のステート.


	void Start () {
		animator = GetComponent<Animator> ();
		statusBW = GetComponent<EnemyStatusBossWolf>();
		animationBW = GetComponent<EnemyAnimationBossWolf>();
		moveBW = GetComponent<EnemyMoveBossWolf>();
		scratchArea = GetComponent<SearchAreaBossWolfScratch> ();
		// 初期位置を保持
		//basePosition = transform.position;
		// 待機時間
		waitTime = waitBaseTime;
		uiManagement = GameObject.Find ("UIManagement");
		uiManager = uiManagement.GetComponent<UIManager> ();
		chariot = GameObject.Find ("Chariot");
		attackTarget = chariot.transform;
		uiManager.SetInBossHP ();
	}
		
	void Update () {

		switch (state) {

		case State.Running:
			Running();
			break;
		case State.Biting:
			Biting();
			break;
		case State.Scratching:
			Scratching();
			break;
		case State.ChaseScratching:
			ChaseScratching();
			break;
		case State.Crossing:
			Crossing();
			break;
		case State.Howling:
			Howling();
			break;
		case State.Downing:
			Downing();
			break;
		}
			
		if (state != nextState)
		{
			state = nextState;
			switch (state) {
			case State.Running:
				SetRunning();
				break;
			case State.Biting:
				SetBiting();
				break;
			case State.Scratching:
				SetScratching();
				break;
			case State.ChaseScratching:
				SetChaseScratching();
				break;
			case State.Crossing:
				SetCrossing();
				break;
			case State.Howling:
				SetHowling();
				break;
			case State.Downing:
				SetDowning();
				break;
			case State.Died:
				Died();
				break;
			}
		}
	}

	void Running(){
		float randomSeed = Random.value;
		if(randomSeed < 0.2){
			moveBW.runSpeed += 2;
		}else if(randomSeed > 0.8){
			moveBW.runSpeed -= 2;
		}
		if(moveBW.runSpeed >= moveBW.maxRunSpeed - 2 || transform.position.z - attackTarget.position.z > 45){
			moveBW.runSpeed -= 5;
		}else if(moveBW.runSpeed <= moveBW.minRunSpeed + 2 || transform.position.z - attackTarget.position.z < -45){
			moveBW.runSpeed += 5;
		}
		if(transform.position.z - attackTarget.position.z > 50){
			moveBW.runSpeed -= 5;
		}else if(transform.position.z - attackTarget.position.z < -50){
			moveBW.runSpeed += 5;
		}

		if (transform.position.x > 0) {
			transform.position = new Vector3 (20, transform.position.y, transform.position.z);
		} else {
			transform.position = new Vector3 (-20, transform.position.y, transform.position.z);;
		}

		if (waitTime > 0.0f) {
			waitTime -= Time.deltaTime;
			// destinationを決定、Moveスクリプトに送信
			Vector3 destinationPosition = transform.position + new Vector3 (0.0f, 0.0f, 100);
			SendMessage ("SetDestination", destinationPosition);
		} else {
			waitTime = Random.Range (waitBaseTime, waitBaseTime * 2.0f);

			//Vector3.Distance (attackTarget.position, transform.position) <= searchScratchRange

			if (statusBW.howlingFrag [statusBW.stageStep]) {
				ChangeState (State.Howling);
			} else if (transform.position.z - attackTarget.position.z < 5 && transform.position.z - attackTarget.position.z > -10) {
				if (transform.position.x * attackTarget.position.x >= 0) {
					ChangeState (State.Scratching);
				} else {
					ChangeState (State.Crossing);
				}
			} else if (randomSeed < 0.7) {
				if (transform.position.z - attackTarget.position.z > 5) {
					ChangeState (State.Biting);
				} else {
					ChangeState (State.ChaseScratching);
				}
			} else {
				ChangeState (State.Crossing);
			}
		}
	}

	void Biting(){
		switch(this.biteStep){
		case 0:
			// アニメーション切り替え、目的地と速さの初期化(Run)
			animator.SetBool ("Biting", true);
			destination = new Vector3 (transform.position.x, transform.position.y, attackTarget.position.z + 50f);
			SendMessage ("SetDestination", destination);
			moveBW.runSpeed = moveBW.maxRunSpeed * 1.2f;
			temporaryHP = statusBW.HP;
			this.biteStep++;
			break;
		case 1:
			// 移動(Run)、アニメーション切り替え、目的地の初期化(Jump)
			if(moveBW.arrived){
				animator.SetTrigger ("Jumping");
				destination = attackTarget.position + new Vector3 (0f, 0f, 80f);
				SendMessage ("SetDestination", destination);
				moveBW.runSpeed = moveBW.maxRunSpeed*1.5f;
				this.biteStep++;
			}
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.biteStep = 0;
			}
			break;
		case 2:
			// 移動(Jump、プレーヤーの方を向かせる)、アニメーション切り替え
			if(moveBW.arrived){
				animator.SetTrigger ("SetStay");
				this.biteStep++;
			}
			transform.LookAt(attackTarget);
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.biteStep = 0;
			}
			break;
		case 3:
			//待機
			if(transform.position.z - attackTarget.position.z < 20f){
				animator.SetBool("AttackBite",true);
				this.biteStep++;
			}
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				attackAreaJaw.enabled = false;
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.biteStep = 0;
			}
			break;
		case 4:
			animator.SetBool("AttackBite",false);
			// 噛みつき攻撃
			if(animationBW.bited == true){
				float randomSeed = Random.value;
				if (randomSeed < 0.5) {
					destination = new Vector3 (-20f, transform.position.y, transform.position.z + 40f);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (20f, transform.position.y, transform.position.z + 40f);
					SendMessage ("SetDestination", destination);
				}
				moveBW.runSpeed = moveBW.maxRunSpeed*0.8f;
				animationBW.bited = false;
				this.biteStep++;
			}
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				attackAreaJaw.enabled = false;
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.biteStep = 0;
			}
			break;
		case 5:
			// Jumpで戻る
			if(moveBW.arrived){
				animator.SetBool ("Biting", false);
				destination = transform.position + new Vector3 (0.0f, 0.0f, 100);
				SendMessage ("SetDestination", destination);
				this.waitTime = this.waitBaseTime;
				this.biteStep = 0;
				ChangeState (State.Running);
			}
			break;
		}
	}

	void Scratching(){
		switch(this.scratchStep){
		case 0:
			// アニメーション切り替え、目的地と速さの初期化
			animator.SetBool ("Scratching", true);
			if (transform.position.x < 0) {
				destination = attackTarget.position + new Vector3 (-3f, 0f, 29f);
				SendMessage ("SetDestination", destination);
			} else {
				destination = attackTarget.position + new Vector3 (3f, 0f, 29f);
				SendMessage ("SetDestination", destination);
			}
			moveBW.runSpeed = moveBW.maxRunSpeed;
			this.scratchStep++;
			break;
		case 1:
			// 移動
			if(moveBW.arrived){
				animator.SetBool ("AttackScratch", true);
				this.scratchStep++;
			}
			break;
		case 2:
			// Scratchアニメーション
			animator.SetBool ("AttackScratch", false);
			if(animationBW.scratchJumped == false){
				transform.position = new Vector3 (transform.position.x, transform.position.y, attackTarget.position.z - 7f);
				transform.LookAt (attackTarget);
			}

			if (animationBW.scratched == true) {
				if (transform.position.x < 0) {
					destination = new Vector3 (-20f, transform.position.y, attackTarget.position.z);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (20f, transform.position.y, attackTarget.position.z);
					SendMessage ("SetDestination", destination);
				}
				moveBW.runSpeed = moveBW.maxRunSpeed * 1.5f;
				animationBW.scratched = false;
				animationBW.scratchJumped = false;
				this.scratchStep++;
			}
			break;
		case 3:
			// Jumpで戻る
			if(moveBW.arrived){
				animator.SetBool ("Scratching", false);
				destination = transform.position + new Vector3 (0.0f, 0.0f, 100);
				SendMessage ("SetDestination", destination);
				moveBW.runSpeed = moveBW.maxRunSpeed * 0.8f;
				this.waitTime = this.waitBaseTime;
				this.scratchStep = 0;
				ChangeState (State.Running);
			}
			break;
		}
	}

	void ChaseScratching(){
		switch(this.chaseScratchStep){
		case 0:
			// アニメーション切り替え、目的地と速さの初期化
			animator.SetBool ("ChaseScratching", true);
			destination = attackTarget.position + new Vector3 (0f, 0f, -20f);
			SendMessage ("SetDestination", destination);
			moveBW.runSpeed = moveBW.maxRunSpeed * 1.2f;
			temporaryHP = statusBW.HP;
			this.chaseScratchStep++;
			break;
		case 1:
			// 移動(Jump)
			if(moveBW.arrived){
				animator.SetTrigger ("SetRun");
				destination = attackTarget.position;
				SendMessage ("SetDestination", destination);
				this.chaseScratchStep++;
				moveBW.runSpeed = moveBW.maxRunSpeed*1.4f;
			}
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.chaseScratchStep = 0;
			}
			break;
		case 2:
			// 追跡(Run)
			destination = attackTarget.position;
			SendMessage ("SetDestination", destination);
			if(transform.position.z - attackTarget.position.z > -7f){
				animator.SetBool ("AttackScratch", true);
				this.chaseScratchStep++;
			}
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				attackAreaClaw.enabled = false;
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.chaseScratchStep = 0;
			}
			break;
		case 3:
			// Scratchアニメーション
			animator.SetBool ("AttackScratch", false);
			if(animationBW.scratchJumped == false){
				transform.position = new Vector3 (transform.position.x, transform.position.y, attackTarget.position.z - 7f);
			}

			if (animationBW.scratched == true) {
				float randomSeed = Random.value;
				if (randomSeed < 0.5) {
					destination = new Vector3 (-20f, transform.position.y, attackTarget.position.z);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (20f, transform.position.y, attackTarget.position.z);
					SendMessage ("SetDestination", destination);
				}
				moveBW.runSpeed = moveBW.maxRunSpeed*1.5f;
				animationBW.scratched = false;
				this.chaseScratchStep++;
			}
			if(temporaryHP - statusBW.HP >= downSwitchHpDif){
				attackAreaClaw.enabled = false;
				ChangeState (State.Downing);
				animator.SetTrigger ("SetDown");
				this.chaseScratchStep = 0;
			}
			break;
		case 4:
			// Jumpで戻る
			if(moveBW.arrived){
				animator.SetBool ("ChaseScratching", false);
				animationBW.scratchJumped = false;
				destination = transform.position + new Vector3 (0.0f, 0.0f, 100);
				SendMessage ("SetDestination", destination);
				this.waitTime = this.waitBaseTime;
				this.chaseScratchStep = 0;
				moveBW.runSpeed = moveBW.maxRunSpeed*0.8f;
				ChangeState (State.Running);
			}
			break;
		}
	}

	void Crossing(){
		switch (this.crossStep) {
		case 0:
			// アニメーション切り替え、目的地と速さの初期化
			animator.SetBool ("Crossing", true);
			if (transform.position.x < 0) {
				if (transform.position.z - attackTarget.position.z > 20f) {
					destination = new Vector3 (19f, transform.position.y, transform.position.z + 50f);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (19f, transform.position.y, attackTarget.position.z + 20f);
					SendMessage ("SetDestination", destination);
				}
			} else {
				if (transform.position.z - attackTarget.position.z > 20f) {
					destination = new Vector3 (-19f, transform.position.y, transform.position.z + 50f);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (-19f, transform.position.y, attackTarget.position.z + 20f);
					SendMessage ("SetDestination", destination);
				}
			}
			moveBW.runSpeed = moveBW.maxRunSpeed * 2.0f;
			this.crossStep++;
			break;
		case 1:
			// 移動(Jump)
			transform.position = new Vector3 (transform.position.x, 0.68f+0.02f*(20 + transform.position.x)*(20 - transform.position.x) , transform.position.z);
			if (moveBW.arrived) {
				if(transform.position.x < 0){
					transform.position = new Vector3 (-20, transform.position.y, transform.position.z);
				}else{
					transform.position = new Vector3 (20, transform.position.y, transform.position.z);
				}
				if(Vector3.Distance(attackTarget.position, transform.position) > searchScratchRange){
					animator.SetBool ("Crossing", false);
					destination = transform.position + new Vector3 (0f, 0f, 100f);
					SendMessage ("SetDestination", destination);
					moveBW.runSpeed = moveBW.maxRunSpeed*0.8f;
					this.waitTime = this.waitBaseTime;
					this.crossStep = 0;
					ChangeState (State.Running);
				}else{
					animator.SetBool ("AttackScratch", true);
					/*
					if (transform.position.x < 0) {
						destination = attackTarget.position + new Vector3 (-3f, 0f, -7f);
						SendMessage ("SetDestination", destination);
					} else {
						destination = attackTarget.position + new Vector3 (3f, 0f, -7f);
						SendMessage ("SetDestination", destination);
					}
					moveBW.runSpeed = moveBW.maxRunSpeed*2.0f;
					*/
					this.crossStep++;
				}
			}
			break;
		case 2:
			animator.SetBool ("AttackScratch", false);
			if(animationBW.scratchJumped == false){
				transform.position = new Vector3 (transform.position.x, transform.position.y, attackTarget.position.z - 4f);
			}
			transform.LookAt (attackTarget);
			if (animationBW.scratched == true) {
				if (transform.position.x < 0) {
					destination = new Vector3 (-20f, transform.position.y, transform.position.z + 20f);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (20f, transform.position.y, transform.position.z + 20f);
					SendMessage ("SetDestination", destination);
				}
				moveBW.runSpeed = moveBW.maxRunSpeed * 0.8f;
				animationBW.scratched = false;
				animationBW.scratchJumped = false;
				this.crossStep++;
			}
			break;
		case 3:
			if(moveBW.arrived){
				animator.SetBool ("Crossing", false);
				destination = transform.position + new Vector3 (0.0f, 0.0f, 100);
				SendMessage ("SetDestination", destination);
				this.waitTime = this.waitBaseTime;
				this.crossStep = 0;
				ChangeState (State.Running);
			}
			break;
		}
	}

	void Howling(){
		switch (this.howlingStep) {
		case 0:
			// アニメーション切り替え、目的地と速さの初期化
			animator.SetBool ("Howling", true);
			destination = new Vector3 (0f, transform.position.y, attackTarget.position.z);
			SendMessage ("SetDestination", destination);
			moveBW.runSpeed = moveBW.maxRunSpeed * 1.5f;
			this.howlingStep++;
			break;
		case 1:
			//移動(Run)
			if(moveBW.arrived){
				animator.SetTrigger ("SetBark");
				transform.LookAt (attackTarget);
				this.howlingStep++;
			}
			break;
		case 2:
			//吠える
			if (animationBW.barked == true) {
				float randomSeed = Random.value;
				if (randomSeed < 0.5) {
					destination = new Vector3 (-17f, transform.position.y, attackTarget.position.z + 10f);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (17f, transform.position.y, attackTarget.position.z + 10f);
					SendMessage ("SetDestination", destination);
				}
				moveBW.runSpeed = moveBW.maxRunSpeed*3.0f;
				animationBW.barked = false;
				statusBW.howlingFrag [statusBW.stageStep] = false;
				this.howlingStep++;
			}
			break;
		case 3:
			// Jumpで戻る
			if (moveBW.arrived) {
				animator.SetBool ("Howling", false);
				destination = transform.position + new Vector3 (0.0f, 0.0f, 100);
				SendMessage ("SetDestination", destination);
				this.waitTime = this.waitBaseTime;
				this.howlingStep = 0;
				ChangeState (State.Running);
			}
			break;
		}
	}

	void Downing(){
		switch(downStep){
		case 0:
			transform.LookAt (attackTarget);
			if (animationBW.downed == true) {
				float randomSeed = Random.value;
				if (randomSeed < 0.5) {
					destination = new Vector3 (-20f, transform.position.y, attackTarget.position.z);
					SendMessage ("SetDestination", destination);
				} else {
					destination = new Vector3 (20f, transform.position.y, attackTarget.position.z);
					SendMessage ("SetDestination", destination);
				}
				moveBW.runSpeed = moveBW.maxRunSpeed*1.5f;
				animationBW.downed = false;
				this.downStep++;
			}
			break;
		case 1:
			if(moveBW.arrived){
				animator.SetBool ("ChaseScratching", false);
				animationBW.scratchJumped = false;
				animator.SetBool ("Biting", false);
				animationBW.bited = false;
				animationBW.scratched = false;
				destination = transform.position + new Vector3 (0.0f, 0.0f, 100);
				SendMessage ("SetDestination", destination);
				this.waitTime = this.waitBaseTime;
				this.downStep = 0;
				ChangeState (State.Running);
			}
			break;
		}
	}

	void Died(){
		destination = transform.position;
		SendMessage ("SetDestination", destination);
		statusBW.died = true;
	}


	void SetRunning(){
		StateStartCommon();
	}

	void SetBiting(){
		StateStartCommon();
		statusBW.biting = true;
	}

	void SetScratching(){
		StateStartCommon();
		statusBW.scratching = true;
	}

	void SetChaseScratching(){
		StateStartCommon();
		statusBW.chaseScratching = true;
	}

	void SetCrossing(){
		StateStartCommon();
		statusBW.crossing = true;
	}

	void SetHowling(){
		StateStartCommon();
		statusBW.howling = true;
	}

	void SetDowning(){
		StateStartCommon();
		statusBW.downing = true;
	}

	//BulletDamageAreaスクリプトから、HitArea経由でダメージを引っ張ってくる
	void Damage(int attackInfo)
	{
		statusBW.HP -= attackInfo;//当たった弾の攻撃力分HPを減らす
		uiManager.UpdateBossHP(statusBW.HP,statusBW.maxHP);
		if (statusBW.HP <= 0) {
			statusBW.HP = 0;
			ChangeState(State.Died);
		}
	}

	void StateStartCommon(){
		statusBW.attacking = false;
		statusBW.biting = false;
		statusBW.scratching = false;
		statusBW.chaseScratching = false;
		statusBW.crossing = false;
		statusBW.howling = false;
		statusBW.downing = false;
		statusBW.died = false;
	}

	void ChangeState(State nextState)
	{
		this.nextState = nextState;
	}




	void Walking(){
		if (waitTime > 0.0f)
		{
			waitTime -= Time.deltaTime;
			if (waitTime <= 0.0f)
			{
				// 範囲内の何処か
				Vector2 randomValue = Random.insideUnitCircle * walkRange;
				Vector3 destinationPosition = /* basePosition + */ new Vector3(randomValue.x, 0.0f, randomValue.y);
				SendMessage("SetDestination", destinationPosition);
			}
		}
		else
		{
			// 目的地へ到着
			if (moveBW.Arrived())
			{
				waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
			}
			if (attackTarget)
			{
				ChangeState(State.Chasing);
			}
		}
	}


	void Chasing()
	{
		SendMessage("SetDestination", attackTarget.position);
		if (Vector3.Distance( attackTarget.position, transform.position ) <= searchScratchRange)
		{
			ChangeState(State.Attacking);
		}
	}


	void Attacking()
	{
		if (animationBW.IsAttacked())
			ChangeState(State.Walking);
		waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
		attackTarget = null;
	}



	void WalkStart()
	{
		StateStartCommon();
	}
		
	void ChaseStart()
	{
		StateStartCommon();
	}
		
	void AttackStart()
	{
		StateStartCommon();
		statusBW.attacking = true;

		// 敵をこちらに向かせる.EnemyMoveスクリプト
		Vector3 targetDirection = (attackTarget.position-transform.position).normalized;
		moveBW.SetDiection (targetDirection);
		//SendMessage("SetDirection",targetDirection);

		// 移動を止める.EnemyMoveスクリプト（攻撃の瞬間だけ一瞬止まる）
		moveBW.StopMove();
		//SendMessage("StopMove");
	}
		

	// 攻撃対象を設定する
	public void SetAttackTarget(Transform target){
		attackTarget = target;
	}

	void HitAreaBodyOff(){
		hitAreaBody.enabled = false;
		Invoke("HitAreaBodyOn", 0.1f);
	}
	void HitAreaBodyOn(){
		hitAreaBody.enabled = true;
	}
	void HitAreaHeadOff(){
		hitAreaHead.enabled = false;
		Invoke("HitAreaHeadOn", 0.1f);
	}
	void HitAreaHeadOn(){
		hitAreaHead.enabled = true;
	}
}
