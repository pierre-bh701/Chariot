using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static int knockedOutEnemies;

	public GameObject playerHPGage; //プレイヤーの体力ゲージ、Startで取得
	public GameObject playerCPGage; //プレイヤーのチャージポイントゲージ、Startで取得
	public GameObject bossEnemyHPGage; //ボスエネミーの体力ゲージ、Startで取得
	public GameObject bossEnemyHPGageMask;
	public GameObject imagePressSpaceKey;
	float blinkInterval = 0.5f;
	float blinkNextTime = 0.5f;
	public GameObject[] bulletViewer;
	int currentBulletNum = 0;
	int previousBulletNum = 2;
	private bool bulletChanging = false;

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
		
	}

	void Update () {

		//ゲーム開始用
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

		//砲弾種変更用
		bulletViewer [previousBulletNum].transform.position = Vector3.MoveTowards (bulletViewer [previousBulletNum].transform.position, new Vector3 (100, 150, 0), 8f);
		bulletViewer [3 - previousBulletNum - currentBulletNum].transform.position = Vector3.MoveTowards (bulletViewer [3 - previousBulletNum - currentBulletNum].transform.position, new Vector3 (120, 140, 0), 4f);
		bulletViewer [currentBulletNum].transform.position = Vector3.MoveTowards (bulletViewer [currentBulletNum].transform.position, new Vector3 (140, 130, 0), 4f);

	}

	public void UpdatePlayerHP(int hp, int maxhp){
		this.playerHPGage.GetComponent<Image> ().fillAmount = (float)hp / maxhp;
	}
	public void UpdatePlayerCP(float cp, float maxcp){
		this.playerCPGage.GetComponent<Image> ().fillAmount = cp / maxcp;
	}

	public void SetInBossHP(){
		this.bossEnemyHPGage.GetComponent<Image> ().fillAmount = 1f;
		this.bossEnemyHPGageMask.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
		this.bossEnemyHPGage.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
	}
	public void UpdateBossHP(float hp, float maxhp){
		this.bossEnemyHPGage.GetComponent<Image> ().fillAmount = (float)hp / maxhp;
	}
	public void SetOutBossHP(){
		this.bossEnemyHPGage.GetComponent<Image> ().color = new Color32 (255, 255, 255, 0);
		this.bossEnemyHPGageMask.GetComponent<Image> ().color = new Color32 (255, 255, 255, 0);
		this.bossEnemyHPGage.GetComponent<Image> ().fillAmount = 1f;
	}

	public void ChangeBulletUI(){
		this.previousBulletNum = this.currentBulletNum;
		this.bulletViewer [this.currentBulletNum].transform.SetSiblingIndex (0);
		if(this.currentBulletNum < 2){
			this.currentBulletNum++;
		}else{
			this.currentBulletNum = 0;
		}
		bulletViewer [previousBulletNum].GetComponent<Image> ().color = new Color32 (131, 131, 131, 255);
		bulletViewer [3 - currentBulletNum - previousBulletNum].GetComponent<Image> ().color = new Color32 (131, 131, 131, 255);
		bulletViewer [currentBulletNum].GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
	}

}
