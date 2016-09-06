using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGenerator : MonoBehaviour {

	//？自機に合わせて動かさないと重くなるかも？

	const int StageTipSize = 500;//ステージチップの大きさ

	public int currentTipIndex;
	public static int phaseCount = 0; //フェイズ進行度、？全ステージ通し番号？、敵消滅数にしたがってカウントアップ
	public static int destroyedEnemies; //敵消滅数、EnemyCtrl,AutoEnemyDestroyでカウントアップ

	public Transform character;//ターゲットキャラクターの指定
	public GameObject[] stageTips;//ステージチッププレハブ配列
	public int startTipIndex;//自動生成開始インデックス
	public int preInstantiate;//生成先読み個数
	int nextTipIndex; //次生成ステージチップ番号
	public List<GameObject> generatedStageList = new List<GameObject>();//生成済みステージチップ保持リスト

	public GameObject[] enemyGenerator;//エネミージェネレータの取得

	void Start () {
		currentTipIndex = startTipIndex - 1;
		UpdateStage (preInstantiate);
	}
	//初期化処理

	void Update () {

		if(phaseCount==0){
			if(Input.GetKey(KeyCode.Space)){
				phaseCount++;
			}
			//ステージチップの更新処理
			int charaPositionIndex = (int)(character.position.z / StageTipSize);
			if (charaPositionIndex + preInstantiate > currentTipIndex){
				UpdateStage (charaPositionIndex + preInstantiate);
			}
		}

		if(phaseCount >= 1 && phaseCount <= 3){
			//敵消滅数が定数以上ならnextStageTipを更新、？定数はステージやフェイズによって変更？
			if(destroyedEnemies >= 20 && phaseCount < 3){
				phaseCount++;
				destroyedEnemies = 0;
			}

			//キャラクターの位置から現在のステージチップのインデックスを計算
			int charaPositionIndex = (int)(character.position.z / StageTipSize);

			//次のステージチップに入ったらステージの更新処理を行う
			if (charaPositionIndex + preInstantiate > currentTipIndex){
				UpdateStage (charaPositionIndex + preInstantiate);
			}
		}
	}

	//指定のIndexまでのステージチップを生成して管理下に置く
	void UpdateStage (int toTipIndex){
		if (toTipIndex <= currentTipIndex) {
			return;
		}
		//指定のステージチップまで作成
		for (int i = currentTipIndex + 1; i <= toTipIndex; i++) {
			GameObject stageObject = GenerateStage (i);
			//生成したステージチップを管理リストに追加
			generatedStageList.Add(stageObject);
			if (phaseCount >= 1 && phaseCount <= 3) { // 戦闘フェイズならば
				//ステージチップ上にエネミージェネレータを配置
				GenerateEnemyGenerator (StageTipSize * i, StageTipSize);
			}
		}
		//ステージの保持上限内になるまで古いステージを削除
		while(generatedStageList.Count > preInstantiate + 2)DestroyOldestStage();

		currentTipIndex = toTipIndex;
	}

	//指定のインデックス位置にStageオブジェクトをランダムに生成/フェイズ数に管理させる
	GameObject GenerateStage(int tipIndex){
		GameObject stageObject = (GameObject)Instantiate (
			stageTips [PhaseCountToStageTipIndex(phaseCount)],
			                         new Vector3 (0, 0, tipIndex * StageTipSize),
			                         Quaternion.identity
		                         );
		return stageObject;
	}

	//一番古いステージの削除
	void DestroyOldestStage(){
		GameObject oldStage = generatedStageList [0];
		generatedStageList.RemoveAt (0);
		Destroy (oldStage);
	}

	//エネミージェネレータを生成（現在3×4で12個、ステージを長方形区間に分割して１つずつランダムに配置）
	void GenerateEnemyGenerator(int stagez, int stagesize){
		for(int i=0;i<3;i++){
			for(int j=0;j<6;j++){
				float randomx = -20f + 13.3f * i + 13.3f * Random.value;
				float randomz = stagez - 0.5f * stagesize + 83.3f * j + 83.3f * Random.value;
				float randomr = -120f + 60f * Random.value;
				Instantiate(enemyGenerator[PhaseCountToEnemyIndex(phaseCount)], new Vector3(randomx,0.73f,randomz), Quaternion.Euler(0,randomr,0));
			}
		}
	}

	// フェイズに応じてステージチップの種類番号を返す
	int PhaseCountToStageTipIndex (int phase){
		switch (phase) {
		case 0:
			return 0;
		case 1:
			return 0;
		case 2:
			return 1;
		case 3:
			return 2;
		}
		return 0;
	}

	// フェイズに応じてエネミーの種類番号を返す
	int PhaseCountToEnemyIndex(int phase){
		switch (phase) {
		case 1:
			return 0;
		case 2:
			return 1;
		case 3:
			return 2;
		}
		return 0;
	}
}
