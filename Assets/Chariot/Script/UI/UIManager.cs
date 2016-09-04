using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static int knockedOutEnemies;

	GameObject playerHPGage; //プレイヤーの体力ゲージ、Startで取得（以下ゲージは全てStartで取得）
	// float playerHP = 1; //プレイヤーの体力 Hit Point
	GameObject playerCPGage; //プレイヤーのチャージポイントゲージ、Startで取得
	// float playerCP = 1; //プレイヤーのチャージポイント Charge Point

	/* 今後実装するもの
	 * Score
	 * playTime
	 * EnemyHP
	 * bossEnemyHP
	 * loadedBullet
	 * phase
	 * pauseButton
	 */

	void Start () {
		//各種ゲージのオブジェクトを取得
		this.playerHPGage = GameObject.Find ("PlayerHPGage");
		this.playerCPGage = GameObject.Find ("PlayerCPGage");
	}

	void Update () {
		
	}

	public void UpdatePlayerHP(int hp, int maxhp){
		this.playerHPGage.GetComponent<Image> ().fillAmount = (float)hp / maxhp;
	}
	public void UpdatePlayerCP(float cp, float maxcp){
		this.playerCPGage.GetComponent<Image> ().fillAmount = cp / maxcp;
	}

}
