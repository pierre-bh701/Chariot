using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static int knockedOutEnemies;

	GameObject playerHPGage; //プレイヤーの体力ゲージ、Startで取得（以下ゲージは全てStartで取得）
	// float playerHP = 1; //プレイヤーの体力 Hit Point
	GameObject playerCPGage; //プレイヤーのチャージポイントゲージ、Startで取得
	// float playerCP = 1; //プレイヤーのチャージポイント Charge Point
	GameObject imagePressSpaceKey;
	float blinkInterval = 0.5f;
	float blinkNextTime = 0.5f;

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
		this.imagePressSpaceKey = GameObject.Find ("ImagePressSpaceKey");

	}

	void Update () {
		if(imagePressSpaceKey && Input.GetKey(KeyCode.Space)){
			Destroy (imagePressSpaceKey);
		}
		if(imagePressSpaceKey){
			if(Time.time > blinkNextTime){
				float alpha = imagePressSpaceKey.GetComponent<CanvasRenderer> ().GetAlpha ();
				if(alpha == 0.0f){
					imagePressSpaceKey.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
				}else{
					imagePressSpaceKey.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
				}
				blinkNextTime += blinkInterval;
			}
		}
	}

	public void UpdatePlayerHP(int hp, int maxhp){
		this.playerHPGage.GetComponent<Image> ().fillAmount = (float)hp / maxhp;
	}
	public void UpdatePlayerCP(float cp, float maxcp){
		this.playerCPGage.GetComponent<Image> ().fillAmount = cp / maxcp;
	}

}
