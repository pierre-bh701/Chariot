using UnityEngine;
using System.Collections;

public class MaimMenuSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// シーンを切り替えます
    /// </summary>
    /// <param name="SceneName">切り替え先のシーン名</param>
    /// <remarks>
    /// この関数の呼び出し元は、AttackChanceButton の FocusEffect の中の OnMouseDownHander です。
    /// </remarks>
    public void ChangeScene(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
