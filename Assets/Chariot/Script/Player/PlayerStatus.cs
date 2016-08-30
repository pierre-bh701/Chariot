using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	//体力
	public int HP = 100;
	public int MaxHP = 100;

	//最後に弾を当てた敵
	public GameObject lastHitTarget = null;

	//プレイヤーの状態
	//突進時
	public bool charge = false;

	//被弾時
	public bool stun = false;

	//敗北時
	public bool defeated = false;
}
