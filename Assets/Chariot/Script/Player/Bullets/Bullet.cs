using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject HitExplosion;
	public GameObject DamageArea;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * 100;


	}

	private void OnCollisionEnter(Collision collider){
		Instantiate (HitExplosion, gameObject.transform.position, gameObject.transform.rotation);
		Instantiate (DamageArea, gameObject.transform.position, gameObject.transform.rotation);
		Destroy (gameObject);
	}
}
