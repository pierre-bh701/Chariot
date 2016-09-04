using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	PlayerStatus status;
	//PlayerAnimation playerAnimation;

	GameObject uimanagement; //UI更新用
	UIManager uimanager; //UI更新用

	void Start(){
		status = GetComponent<PlayerStatus> ();
		//playerAnimation = GetComponent<PlayerAnimation>();
		uimanagement = GameObject.Find ("UIManagement");
		uimanager = uimanagement.GetComponent<UIManager> (); //UI更新用スクリプトを取得
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
			//このフラグが立つ事で、被弾時のアニメーションが再生される。
			//その後、そのアニメーションイベントの最後で、このフラグを元に戻す
		}
		uimanager.UpdatePlayerHP (status.HP, status.MaxHP);
	}
}
