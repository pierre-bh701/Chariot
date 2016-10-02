using UnityEngine;
using System.Collections;

public class PlayerHitArea : MonoBehaviour {

	 public Collider playerHitCollider;

	/*
	void Start(){
		playerHitCollider = GetComponent<Collider>();
	}
	*/

	void Damage(int power){
		playerHitCollider.enabled = false;
		transform.root.SendMessage ("Damage", power);
		Invoke ("colliderEnable", 1.0f);
	}

	void colliderEnable(){
		playerHitCollider.enabled = true;
		gameObject.layer = LayerMask.NameToLayer("PlayerHit");
	}
}

