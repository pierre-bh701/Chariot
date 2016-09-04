using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour {

	GameObject canvas;
	public GameObject EnemyHPGage; //敵の体力ゲージプレハブ、Inspecterで取得
	public GameObject enemyHPGage; //敵の体力ゲージ
	EnemyStatus enemyStatus;

	void Start () {
		enemyStatus = GetComponent<EnemyStatus> ();
		canvas = GameObject.Find ("CanvasOnWorldSpace");
		enemyHPGage = Instantiate (EnemyHPGage) as GameObject;
		enemyHPGage.transform.SetParent (canvas.transform, false);
	}

	void Update () {
		
		this.enemyHPGage.transform.position = this.transform.position + new Vector3(0, 2, 0);
		this.enemyHPGage.GetComponent<Image> ().fillAmount = (float)enemyStatus.HP / enemyStatus.MaxHP;
	}
}
