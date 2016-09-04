using UnityEngine;
using System.Collections;

public class AutoEnemyDestroy : MonoBehaviour {

	public GameObject player;

	void Start(){
		player = GameObject.Find ("Chariot");
	}
		
	void Update () {
		//敵がプレイヤーより後方100fより遠ざかったら自動的に消滅、その際に敵消滅数カウントアップ
		if (player.transform.position.z - transform.position.z > 100.0f) {
			StageGenerator.destroyedEnemies++;
			Destroy (this.gameObject);
		}
	}
}
