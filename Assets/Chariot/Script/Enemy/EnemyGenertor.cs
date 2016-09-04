using UnityEngine;
using System.Collections;

public class EnemyGenertor : MonoBehaviour {

	//このスクリプトが置かれたオブジェクトから、敵プレハブが出てくる。


	//生まれてくる敵プレハブ
	public GameObject enemyPrefab;
	public GameObject player;

	//プレイヤー


	void Start () {
		player = GameObject.Find ("Chariot");
	}
	
	void Update(){
		//一定距離内に入ったら
		if(transform.position.z - player.transform.position.z < 100.0f){
			Generate ();//敵生成
			Destroy (gameObject);//生成後に削除
		}
	}

	//敵プレハブ生成
	void Generate(){
		Instantiate (enemyPrefab, transform.position, transform.rotation);
	}
}
