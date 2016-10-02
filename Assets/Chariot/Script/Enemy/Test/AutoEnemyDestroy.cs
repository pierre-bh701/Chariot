using UnityEngine;
using System.Collections;

public class AutoEnemyDestroy : MonoBehaviour {

	GameObject stageManagement;
	StageGenerator stageGenerator;
	EnemyUIManager enemyUIManager;
	public GameObject player;

	void Start(){
		player = GameObject.Find ("Chariot");
		enemyUIManager = GetComponent<EnemyUIManager> ();
		stageManagement = GameObject.Find ("StageManagement");
		stageGenerator = stageManagement.GetComponent<StageGenerator> ();
	}
		
	void Update () {
		//敵がプレイヤーより後方100fより遠ざかったら自動的に消滅、その際に敵消滅数カウントアップ
		if (player.transform.position.z - transform.position.z > 100.0f) {
			stageGenerator.phaseDestroyedEnemies++;
			stageGenerator.destroyedEnemies++;
			Destroy(enemyUIManager.enemyHPGage);
			Destroy (this.gameObject);
		}
	}
}
