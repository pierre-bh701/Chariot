using UnityEngine;
using System.Collections;

public class EnemyStatusBossWolf : MonoBehaviour {

	public int HP = 1000;
	public int maxHP = 1000;

	public int Power = 10;

	public bool attacking = false;

	public bool biting = false;
	public bool scratching = false;
	public bool chaseScratching = false;
	public bool crossing = false;
	public bool howling = false;
	public bool downing = false;
	public bool died = false;

	public int stageStep = 0;
	public bool [] howlingFrag = {false, true, true, true}; //遠吠えカウント

	void Update(){
		UpdateStageStep ();
	}

	// 遠吠え用、ボスの体力によってstageStepを変化
	void UpdateStageStep(){
		if ((float)HP / maxHP <= 0.25f) {
			stageStep = 3;
		} else if ((float)HP / maxHP <= 0.5f) {
			stageStep = 2;
		} else if ((float)HP / maxHP <= 0.75f) {
			stageStep = 1;
		}
	}
}
