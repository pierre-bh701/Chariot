using UnityEngine;
using System.Collections;

public class AutoEnemyDestroy : MonoBehaviour {

	public GameObject player;

	void Start(){
		player = GameObject.Find ("Chariot");
	}
	
	void Update () {
		if ((transform.position - player.transform.position).magnitude > 100.0f) {
			Destroy (this.gameObject);
		}
	}
}
