using UnityEngine;
using System.Collections;

public class SearchAreaBossWolfScratch : MonoBehaviour {

	EnemyCtrlBossWolf enemyCtrl;
	public bool isInScratchArea = false;

	void Start () {
		enemyCtrl = transform.root.GetComponent<EnemyCtrlBossWolf>();
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			this.isInScratchArea = true;
		}
	}
}
