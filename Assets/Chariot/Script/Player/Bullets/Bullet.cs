using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject HitExplosion;//爆発のエフェクト
	public GameObject DamageArea;//攻撃の当たり判定用のemptyオブジェクト

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5.0f);//発射して５秒後に玉が消滅
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * 100;//玉の進行の仕方


	}

	//玉がぶつかった時にエフェクトと当たり判定オブジェクトを生成、玉を削除
	private void OnCollisionEnter(Collision collider){
		Instantiate (HitExplosion, gameObject.transform.position, gameObject.transform.rotation);
		Instantiate (DamageArea, gameObject.transform.position, gameObject.transform.rotation);
		Destroy (gameObject);
	}
}
