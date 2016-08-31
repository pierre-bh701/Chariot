using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	float distance = 10.0f; //カメラと自機との距離
	public float horizontalAngle = 0.0f; //カメラのアングル
	public float rotAngle = 180.0f; //カメラのアングル

	public float verticalAngle = 10.0f;
	public Transform lookTarget;
	Vector3 offset = new Vector3 (0, 3f, 0); //カメラの高さ
	public Camera TPcamera;

	CameraAndShotController cameraAndShotController;

	void Start () {
		cameraAndShotController = FindObjectOfType<CameraAndShotController> ();
	}

	void Update () {

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
	}
	public void SetTarget(Transform target){
		lookTarget = target;
	}
}
