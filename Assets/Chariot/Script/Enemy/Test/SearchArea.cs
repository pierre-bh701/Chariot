using UnityEngine;
using System.Collections;

public class SearchArea : MonoBehaviour {
	EnemyCtrl enemyCtrl;

	// Use this for initialization
	void Start () {
		//EnemyCtrlをキャッシュする
		enemyCtrl = transform.root.GetComponent<EnemyCtrl>();
	}

	void OnTriggerStay(Collider other){
		//Debug.Log ("Look"+other.tag);
		if (other.tag == "Player") {
			enemyCtrl.SetAttackTarget (other.transform); //ターゲットをプレイヤーに指定
		}
	}
}
