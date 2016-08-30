using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	PlayerStatus status;
	//PlayerAnimation playerAnimation;

	void Start(){
		status = GetComponent<PlayerStatus> ();
		//playerAnimation = GetComponent<PlayerAnimation>();
	}


	//AttackAreaスクリプトから、PlayerHitArea経由でダメージを引っ張ってくる
	void Damage(AttackArea.AttackInfo attackInfo)
	{
		status.HP -= attackInfo.attackPower;//当たった攻撃の攻撃力分HPを減らす
		if (status.HP <= 0) {
			status.HP = 0;
			// 体力０なので敗北
			status.defeated = true;
		} else {
			status.stun = true;
			//このフラグが立つ事で、被弾時のアニメーションが再生される。その後、そのアニメーションイベントの最後で、このフラグを元に戻す
		}
	}
}
