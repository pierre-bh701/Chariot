using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	public GameObject Bullet;
	public GameObject Cannon;
	public GameObject Cannon001;
	public GameObject Cannon002;
	public GameObject Cannon002end;
	public GameObject targetDirection;//発射する向きを決めるための空のオブジェクト

	CameraAndShotController cameraAndShotController;

	float shotInterval;
	float shotIntervalMax = 0.75f;

	void Start () {
		cameraAndShotController = GetComponent<CameraAndShotController> ();
	}

	void Update () {

		shotInterval += Time.deltaTime;

		if (Input.GetButton ("Fire1")) {
			if (shotInterval > shotIntervalMax) {
				targetDirection.transform.LookAt (cameraAndShotController.Shot ());
				Vector3 targetAngle = targetDirection.transform.rotation.eulerAngles;

				Cannon.transform.rotation = Quaternion.Euler (0, targetAngle.y, 0);//大砲の向きを変える
				Cannon001.transform.rotation = Quaternion.Euler (targetAngle.x + 90f, targetAngle.y, targetAngle.z);//大砲の向きを変える
				Instantiate (Bullet, Cannon002end.transform.position, Quaternion.Euler (targetAngle.x - 1f, targetAngle.y, targetAngle.z));//大砲の玉を生成
				shotInterval = 0f;
			}
		}
	}
}
