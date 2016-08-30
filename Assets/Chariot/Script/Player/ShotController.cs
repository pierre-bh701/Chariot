using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	public GameObject Bullet;
	public GameObject Cannon;
	public GameObject Cannon001;
	public GameObject Cannon002;
	public GameObject Cannon002end;
	public GameObject targetDirection;

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

				Cannon.transform.rotation = Quaternion.Euler (0, targetAngle.y, 0);
				Cannon001.transform.rotation = Quaternion.Euler (targetAngle.x + 90f, targetAngle.y, targetAngle.z);
				Instantiate (Bullet, Cannon002end.transform.position, Quaternion.Euler (targetAngle.x - 1f, targetAngle.y, targetAngle.z));
				shotInterval = 0f;
			}
		}
	}
}
