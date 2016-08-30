using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	//速度、方向関連
	public float speed = 4.0f;//速度倍率
	public float chargeSpeed = 10.0f;//突進時のスピード
	public float chargeCount = 10.0f;//突進時間
	public bool charge = false;//突進スイッチ
	public float gravity = 20.0f;//重力
	private Vector3 moveDirection = Vector3.zero; //左右移動
	//private bool DirectionRotationFlag = false;//円周進行
	public float DirectionAxis = 0; //強制進行方向制御

	//機体の変形関連(ボーン)
	public GameObject body;//機体全体(車輪軸を軸に回転)
	public GameObject wheelF;//前車輪(回転速度3倍)
	public GameObject wheelL;//左車輪
	public GameObject wheelR;//右車輪
	private Vector3 wheelRotation;//車輪の回転用

	private Animator animator;

	//AnimatorコンポーネントクラスのSetFloat、SetBool、SetTrigger関数を利用し、PlayerAnimatorControllerが持つパラメータの値を設定しています
	PlayerStatus status;
	Vector3 prePosition;

	//アニメーションイベントで呼び出す関数、StartAttackHit()、EndAttackHit()、EndAttack()を作成
	//突進攻撃判定の有効/無効化を行う
	void StartChargeHitHit(){
		Debug.Log ("StartAttackHit");
	}
	void EndChargeHitHit(){
		Debug.Log ("EndAttackHit");
	}

	void Defeated(){//倒れるアニメーションで、この関数を呼び出す。
		Debug.Log ("Defeated");
	}

	void Start () {
		animator = GetComponent<Animator> ();
		status = GetComponent<PlayerStatus> ();
	}

	float Angle180(float a)
	{
		while (a >= 180) a -= 360;
		while (a < -180) a += 360;
		return a;
	}
	//値を-180~180でループさせる。

	float AngleDiff(float a, float b)
	{
		float left = Angle180(a - b);
		float right = Angle180(b - a);

		if (Mathf.Abs(left) > Mathf.Abs(right)) return left;
		return right;
	}
	//第２引数から見た、第１引数までの差をとる。(100,-30)なら-130、(-30,100)なら130を返す。

	void Update () {
		CharacterController controller = GetComponent<CharacterController> ();

		float angle = Angle180 (transform.rotation.eulerAngles.y);//機体の向き
		DirectionAxis = Angle180 (DirectionAxis);//機体の強制進行方向(道に沿わせる)


		wheelRotation = new Vector3 (5f, 0, 0) * speed; 
		wheelL.transform.Rotate (wheelRotation);//左輪
		wheelR.transform.Rotate (wheelRotation);//右輪
		wheelF.transform.Rotate (wheelRotation * 3f);//前輪

		//地面に付いている場合のみ、操作を受け付ける
		if (controller.isGrounded) {

			moveDirection = new Vector3 (0, 0, 5f) * speed;//機体の向き、速度

			if (Input.GetKey ("d")) {
				if (AngleDiff (angle - 45, DirectionAxis) > 0) {
					float diff = Mathf.Min (5f, AngleDiff (angle - 45f, DirectionAxis));
					transform.Rotate (new Vector3 (0, diff, 0));

					wheelL.transform.Rotate (wheelRotation + new Vector3 (diff, 0, 0));
					wheelR.transform.Rotate (wheelRotation + new Vector3 (-diff, 0, 0));
				}
			}
			//押している間だけ右に45度向く

			if (Input.GetKey ("a")) {
				if (AngleDiff (angle + 45f, DirectionAxis) < 0) {
					float diff = Mathf.Max (-5f, AngleDiff (angle + 45f, DirectionAxis));
					transform.Rotate (new Vector3 (0, diff, 0));

					wheelL.transform.Rotate (wheelRotation + new Vector3 (diff, 0, 0));
					wheelR.transform.Rotate (wheelRotation + new Vector3 (-diff, 0, 0));
				}
			}
			//押している間だけ左に45度向く

			if (!Input.GetKey ("d") && !Input.GetKey ("a")) {
				if (AngleDiff (angle, DirectionAxis) > 0) {
					float diff = Mathf.Min (5f, AngleDiff (angle, DirectionAxis));
					transform.Rotate (new Vector3 (0, diff, 0));
					wheelL.transform.Rotate (new Vector3 (diff, 0, 0));
					wheelR.transform.Rotate (new Vector3 (-diff, 0, 0));
				} else if (AngleDiff (angle, DirectionAxis) < 0) {
					float diff = Mathf.Max (-5f, AngleDiff (angle, DirectionAxis));
					transform.Rotate (new Vector3 (0, diff, 0));
					wheelL.transform.Rotate (new Vector3 (diff, 0, 0));
					wheelR.transform.Rotate (new Vector3 (-diff, 0, 0));
				}
			}
			//a、dを押していないとき、DirectionAxisの向きに戻る。

			if (Input.GetKey ("w")) {
				if (speed < 10.0f) {
					speed += 4.0f * Time.deltaTime;
				}
			}

			//押している間加速

			if (Input.GetKey ("s")) {
				if (speed > 0.0f) {
					speed -= 4.0f * Time.deltaTime;
				} else if (speed < 0.0f) {
					speed = 0.0f;
				}
			}
			//ブレーキ

			if (!Input.GetKey ("w") && !Input.GetKey ("s")) {
				if (speed > 4.0f) {
					speed -= 4.0f * Time.deltaTime;
				} else if (speed < 4.0f) {
					speed += 4.0f * Time.deltaTime;
				}

			}

			//w、kを押していないとき、徐々に元の速度に戻る

			moveDirection = transform.rotation * moveDirection;

			//突進関連
			if (Input.GetButtonDown ("Charge")) {
				if (chargeCount > 9.9f) {
					charge = true;
				}
			}
			if (charge == true) {
				speed = chargeSpeed;
				animator.SetBool ("LanceCharge", true);
				chargeCount -= 2.0f * Time.deltaTime;
			}
			if (chargeCount < 0.1f) {
				charge = false;
				animator.SetBool ("LanceCharge", false);
			}
			if (charge == false && chargeCount < 10.0f) {
				chargeCount += 0.5f * Time.deltaTime;
			}


			//前回のUpdateからの移動量を計算して、Speedパラメータの値を設定している
			Vector3 deltaPosition = transform.position - prePosition;

			animator.SetFloat ("Speed", deltaPosition.magnitude / Time.deltaTime);


			//Playerstatus.defeatedがtrueになったとき、PlayerAnimatorController内のdefeatedパラメータがtrueになり、倒れるアニメーションが再生される
			if (status.defeated == true) {
				animator.SetTrigger ("Defeated");
			}
			prePosition = transform.position;



			//普段は使わない。強制進行方向のデバッグ
			/*
			if (Input.GetKeyDown ("h")) {
				DirectionAxis += 90f;
			}

			if (Input.GetKeyDown ("f")) {
				DirectionAxis -= 90f;
			}

			if (Input.GetKeyDown ("g")) {
				DirectionAxis = 0f;
			}
			//DirectionAxisを元に戻す
			if (Input.GetKeyDown("t")) {
				if (DirectionRotationFlag == false) {
					DirectionRotationFlag = true;
				} else {
					DirectionRotationFlag = false;
				}
			}
			//円周進行のフラグ管理
			if (DirectionRotationFlag == true) {
				DirectionAxis += 10f * Time.deltaTime;
			}
			//円周進行する。値は要調整

			//尚、実際はDirectionAxisは状況に応じて変化する。>if()の中身は、ステージの仕様に応じて変える予定
			*/


		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
	}
}
