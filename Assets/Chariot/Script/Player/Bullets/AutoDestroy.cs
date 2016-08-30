using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	void Start () {
		Destroy (gameObject, 2.0f);
	}
}
