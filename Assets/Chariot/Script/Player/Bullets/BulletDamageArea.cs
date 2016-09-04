using UnityEngine;
using System.Collections;

//砲弾の攻撃力参照用クラス（要らないかも？）（BulletStatus -> this -> HitArea.Damage -> EnemyCtrl.Damage）
public class AttackInfo{
	public int bulletPower;//この弾の攻撃力
	public Transform hitBullet;//当たった弾
}

public class BulletDamageArea : MonoBehaviour {
	//弾の当たり判定用スクリプト

	//弾の攻撃力参照
	BulletStatus status;
	//攻撃判定のコライダ
	Collider damageArea;

	// Use this for initialization
	void Start () {
		status = GetComponent<BulletStatus> ();
		damageArea = GetComponent<Collider> ();
		Destroy (gameObject, 1.0f);
	}
		
	/* 砲弾の攻撃力参照用クラス（上のヤツ、もともとこっちにあった）
	public class AttackInfo{
		public int bulletPower;//この弾の攻撃力
		public Transform hitBullet;//当たった弾
	}
	*/

	AttackInfo GetAttackInfo(){
		AttackInfo attackInfo = new AttackInfo ();//攻撃力の計算
		attackInfo.bulletPower = status.Power;
		attackInfo.hitBullet = transform.root;

		return attackInfo;
	}

	//攻撃が当たった
	void OnTriggerEnter(Collider other){
		//攻撃が当たった相手にDamageメッセージを送る
		if (other.tag == "Enemy") { //ぶつかった対象のタグがEnemyだったら
			other.SendMessage ("Damage", GetAttackInfo ()); //otherのHitArea.Damage関数にGetAttackInfo()を与える
		}
		Destroy (gameObject); //消すかどうかは玉によるかも
	}

	//攻撃判定を有効化（玉によって変更する場合に使おう）
	void OnAttack(){
		damageArea.enabled = true;
	}

	//攻撃判定を無効化（玉によって変更する場合に使おう）
	void OnAttackTermination(){
		damageArea.enabled = false;
	}

}