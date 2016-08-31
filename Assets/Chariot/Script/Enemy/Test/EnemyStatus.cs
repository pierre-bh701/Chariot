using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {

	public int HP = 100;
	public int MaxHP = 100; //現状は使ってない

	public int Power = 10;

	public bool attacking = false;
	public bool died = false;
}
