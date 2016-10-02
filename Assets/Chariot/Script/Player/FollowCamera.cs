using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	float distance = 10.0f; //カメラと注視対象との距離
	public float horizontalAngle = 0.0f; //カメラのアングル
	public float rotAngle = 180.0f; //カメラのアングル

	public float verticalAngle = 10.0f;
	public Transform lookTarget; //デフォルトではChariot
	public Transform chariot; //lookTarget保存用
	Vector3 offset = new Vector3 (0, 3f, 0); //カメラの高さ
	public Camera TPcamera;
	public Transform cameraTarget;

	public bool lookAtSomething;

	CameraAndShotController cameraAndShotController;

	void Start () {
		cameraAndShotController = FindObjectOfType<CameraAndShotController> ();
	}

	void Update () {

		if (lookAtSomething == false) {

			//movedを取得してカメラを動かす
			if (cameraAndShotController.Moved ()) {
				//カメラの位置と、AxisDirectionの向きを更新する
				float anglePerPixel = rotAngle / (float)Screen.width;
				Vector2 delta = cameraAndShotController.GetDeltaPosition ();
				horizontalAngle += delta.x * anglePerPixel;
				horizontalAngle = Mathf.Repeat (horizontalAngle, 360.0f);
				verticalAngle -= delta.y * anglePerPixel;
				verticalAngle = Mathf.Clamp (verticalAngle, -30.0f, 30.0f);
			}

			//カメラを位置と回転を更新する
			if (lookTarget != null) {

				Vector3 lookPosition = lookTarget.position + offset;//カメラの注視点

				//自機からの相対距離
				Vector3 relativePos = Quaternion.Euler (verticalAngle, horizontalAngle, 0) * new Vector3 (0, 0, -distance);

				//自機の位置にオフセット加算した位置に移動させる
				transform.position = lookPosition + relativePos;//カメラの位置と回転

				//このオブジェクトが、Layer"Ground"のオブジェクトを避ける
				//カメラが床より下にいかないように調整（レイヤー使用）
				RaycastHit hitInfo;
				if (Physics.Linecast (lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer ("Ground"))) {
					transform.position = hitInfo.point + new Vector3 (0, 0.01f, 0);
				}
				//自機にこのオブジェクトを向ける
				transform.LookAt (lookPosition);
			}
		} else {
			if (lookTarget != null) {
				Vector3 lookPosition = lookTarget.position + offset;//カメラの注視点
				transform.LookAt (lookPosition);
			}
		}
	}

	public void CameraLookAt(Transform target){
		this.lookAtSomething = true;
		this.lookTarget = target;
		cameraTarget.position = new Vector3 (lookTarget.position.x, lookTarget.position.y, lookTarget.position.z);
		cameraTarget.eulerAngles = new Vector3 (lookTarget.eulerAngles.x, lookTarget.eulerAngles.y, lookTarget.eulerAngles.z);
		transform.position = lookTarget.position + lookTarget.forward * 20f + new Vector3 (0, 10f, 0);
	}

	public void CameraNoLookAt(float delay){
		Invoke ("LookAtReset", delay);
	}

	public void LookAtReset(){
		this.lookAtSomething = false;
		this.lookTarget = this.chariot;
	}
}
