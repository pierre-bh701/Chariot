using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	Vector3 centerPosition;
	float angle = 0.0f;
	float radius = 1.0f;
	void Start( ){
		
		centerPosition = transform.position;
	}

	void Update( ){
		transform.position = centerPosition + radius * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) ,0);
		angle += 0.1f;
	} 

}
