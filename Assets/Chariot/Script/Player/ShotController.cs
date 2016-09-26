using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	public GameObject Bullet0;
	public GameObject Bullet1;
	public GameObject Bullet2;
	public int bulletNum = 0; //砲弾の種類
	public GameObject Cannon;
	public GameObject Cannon001;
	public GameObject Cannon002;
	public GameObject Cannon002end;
	public GameObject targetDirection;//発射する向きを決めるための空のオブジェクト

	public GameObject uiManagement;
	UIManager uiManager;

	CameraAndShotController cameraAndShotController;

	float shotInterval;
	float shotIntervalMax = 0.75f;

	void Start () {
		cameraAndShotController = GetComponent<CameraAndShotController> ();
		uiManager = uiManagement.GetComponent<UIManager> ();
	}

	void Update () {

		shotInterval += Time.deltaTime;

		// 砲弾の種類切り替え
		if(Input.GetButtonDown ("Fire2")){
			ChangeBullet ();
			uiManager.ChangeBulletUI ();
		}

		// 砲弾生成
		if (Input.GetButton ("Fire1")) {

			switch (bulletNum) {

			case 0:
				if (shotInterval > shotIntervalMax) {
					targetDirection.transform.LookAt (cameraAndShotController.Shot ());
					Vector3 targetAngle = targetDirection.transform.rotation.eulerAngles;

					Cannon.transform.rotation = Quaternion.Euler (0, targetAngle.y, 0);//大砲の向きを変える
					Cannon001.transform.rotation = Quaternion.Euler (targetAngle.x + 90f, targetAngle.y, targetAngle.z);//大砲の向きを変える
					Instantiate (Bullet0, Cannon002end.transform.position, Quaternion.Euler (targetAngle.x - 1f, targetAngle.y, targetAngle.z));//大砲の玉を生成
					shotInterval = 0f;
				}
				break;
			
			case 1:
				if (shotInterval > shotIntervalMax) {
					targetDirection.transform.LookAt (cameraAndShotController.Shot ());
					Vector3 targetAngle = targetDirection.transform.rotation.eulerAngles;

					Cannon.transform.rotation = Quaternion.Euler (0, targetAngle.y, 0);//大砲の向きを変える
					Cannon001.transform.rotation = Quaternion.Euler (targetAngle.x + 90f, targetAngle.y, targetAngle.z);//大砲の向きを変える
					Instantiate (Bullet1, Cannon002end.transform.position, Quaternion.Euler (targetAngle.x - 1f, targetAngle.y, targetAngle.z));//大砲の玉を生成
					shotInterval = 0f;
				}
				break;

			case 2:
				if (shotInterval > shotIntervalMax) {
					targetDirection.transform.LookAt (cameraAndShotController.Shot ());
					Vector3 targetAngle = targetDirection.transform.rotation.eulerAngles;

					Cannon.transform.rotation = Quaternion.Euler (0, targetAngle.y, 0);//大砲の向きを変える
					Cannon001.transform.rotation = Quaternion.Euler (targetAngle.x + 90f, targetAngle.y, targetAngle.z);//大砲の向きを変える
					Instantiate (Bullet2, Cannon002end.transform.position, Quaternion.Euler (targetAngle.x - 1f, targetAngle.y, targetAngle.z));//大砲の玉を生成
					shotInterval = 0f;
				}
				break;

			default:
				break;
			}
		}
	}

	void ChangeBullet(){
		if (this.bulletNum < 2) {
			this.bulletNum++;
		} else {
			this.bulletNum = 0;
		}
	}
}
