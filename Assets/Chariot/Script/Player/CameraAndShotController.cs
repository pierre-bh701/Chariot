using UnityEngine;
using System.Collections;

public class CameraAndShotController : MonoBehaviour {

	Vector2 delta = Vector2.zero;
	bool moved = false;

	int left;
	int right;
	int down;
	int up;

	public Camera TPcamera;
	private LayerMask layerMask;
	Vector3 LookAtPos;
	Ray ray;

	private float xMin;
	private float yMin;
	public Texture2D crosshairImage;

	void Start(){
		left = (int)(Screen.width * 0.2f);
		right = (int)(Screen.width * 0.8f);
		down = (int)(Screen.height * 0.2f);
		up = (int)(Screen.height * 0.8f);

		//旧版。ステージに反応したりするので、改造。敵か、ポーズボタン、ポーズメニューのみに反応する。
		/*
		layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask =~ layerMask; //反転
		*/

		layerMask = LayerMask.GetMask(new string[] {"EnemyHit","Button"});

		Cursor.lockState = CursorLockMode.Confined; //はみ出さないモード
		Cursor.visible = false; //OSカーソル非表示


	}


	void Update () {
			
		Vector2 mouse = Input.mousePosition;
		delta = Vector2.zero;

		if (mouse.y > up) {
			moved = true;
			Vector2 diff = mouse - new Vector2 (mouse.x, up);
			delta += diff * 0.1f;
		}

		if (mouse.y < down) {
			moved = true;
			Vector2 diff = mouse - new Vector2 (mouse.x, down);
			delta += diff * 0.1f;
		}

		if (mouse.x < left) {
			moved = true;
			Vector2 diff = mouse - new Vector2 (left, mouse.y);
			delta += diff * 0.1f;
		}
		
		if (mouse.x > right) {
			moved = true;
			Vector2 diff = mouse - new Vector2 (right, mouse.y);
			delta += diff * 0.1f;
		}

		if (mouse.y < up && mouse.y > down && mouse.x > left && mouse.x < right) {
			delta = Vector2.zero;
			moved = false;
		}

		/*
}

void LateUpdate (){
*/
		RaycastHit hit;
		ray = TPcamera.ScreenPointToRay (Input.mousePosition); //カメラからマウスカーソルの向きにRay
		if (Physics.Raycast (ray, out hit, 300.0f, layerMask)) {
			LookAtPos = hit.point;      //Player以外のObjectを見つけた時
		} else {
			LookAtPos = ray.GetPoint (300f);     //何もないとき
		}
	}

	public Vector3 Shot(){
		return LookAtPos;
	}

	//クリックされたかどうかの判定
	public bool Clicked(){
		if (Input.GetButton ("Fire1")) {
			return true;
		} else {
			return false;
		}
	}


	//カーソルと領域の距離
	public Vector2 GetDeltaPosition(){
		return delta;
	}

	public bool Moved() {
		return moved;
	}

	void OnGUI() //カーソルに画像を貼る
	{
		xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (crosshairImage.width / 4);
		yMin = (Screen.height - Input.mousePosition.y) - (crosshairImage.height / 4);
		GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width / 2, crosshairImage.height / 2), crosshairImage);

	}
}
