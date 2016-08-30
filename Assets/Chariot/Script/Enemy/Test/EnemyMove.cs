﻿using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

	const float GravityPower = 9.8f;//重力値
	const float StoppingDistance = 0.6f;//目的地に着いたとみなす停止距離

	Vector3 velocity = Vector3.zero;//現在の移動速度
	CharacterController characterController;//キャラクターコントローラーのキャッシュ
	public bool arrived = false;//到着したか(到着した = true / 到着していない = false)

	bool forceRotate = false;//向きを強制的に指示するか
	Vector3 forceRotateDirection;//強制的に向かせたい方向

	public Vector3 destination;//目的地
	public float walkSpeed = 6.0f;//移動速度
	public float rotationSpeed = 360.0f;//回転速度

	void Start () {
		characterController = GetComponent<CharacterController> ();
		destination = transform.position;
	}

	void Update () {

		//移動速度Velocityを更新する
		if (characterController.isGrounded) {
			//水平面での移動を考えるのでXZのみ扱う
			Vector3 destinationXZ = destination;
			destinationXZ.y = transform.position.y;//高さを目的地と現在地を同じにしておく

			//********ここからXZのみで考える********
			//目的地までの距離と方角を求める

			Vector3 direction = (destinationXZ - transform.position).normalized;
			float distance = Vector3.Distance (transform.position, destinationXZ);

			//現在の速度を退避
			Vector3 currentVelocity = velocity;

			//目的地に近づいたら到着
			if (arrived || distance < StoppingDistance) {
				arrived = true;
			}

			//移動速度を求める
			if (arrived) {
				velocity = Vector3.zero;
			} else {
				velocity = direction * walkSpeed;
			}

			//スムーズに補間
			velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f,1.0f));
			velocity.y = 0;

			if (!forceRotate) {
				//向きを行きたい方向に向ける
				if (velocity.magnitude > 0.1f && !arrived) { //移動していない場合、向きは更新しない
					Quaternion characterTargetRotation = Quaternion.LookRotation (direction);
					transform.rotation = Quaternion.RotateTowards (transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
				}
			} else {
					//強制向き指定
				Quaternion characterTargetRotation = Quaternion.LookRotation (forceRotateDirection);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
				}
			}
		//重力
		velocity += Vector3.down * GravityPower * Time.deltaTime;

		//接地していたら、強く地面に押し付ける(接地判定を強める)(浮いている敵も同様)
		//(UnityのCharacterControllerの特性のため)
		Vector3 snapGround = Vector3.zero;
		if (characterController.isGrounded) {
			snapGround = Vector3.down;
		}

			//CharacterControllerを使って動かす
		characterController.Move(velocity * Time.deltaTime+snapGround);//計算怪しい？

			//強制的に向きを変えるのを解除
		if (forceRotate && Vector3.Dot (transform.forward, forceRotateDirection) > 0.99f) {
			forceRotate = false;
		}
	}

	//壁にぶつかったら止めて、目的地を再配置
	void OnTriggerStay(Collider other){
		if (other.tag == "Wall") {
			arrived = true;
			arrived = false;
		}
	}

	//目的地を設定する。引数destinationは目的地
	public void SetDestination(Vector3 destination){
		arrived = false;
		this.destination = destination;
	}

	//指定した向きを向かせる
	public void SetDiection(Vector3 direction){
		forceRotateDirection = direction;
		forceRotateDirection.y = 0;
		forceRotateDirection.Normalize ();
		forceRotate = true;
	}

	//移動を止める
	public void StopMove(){
		destination = transform.position; //現在地を目的地にする。
	}

	//目的地に到着したかを調べる。true = 到着した / false = 到着していない
	public bool Arrived(){
		return arrived;
	}

}
