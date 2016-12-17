using UnityEngine;
using System.Collections;

/// <summary>
/// UIオブジェクトの透明度を一定時間ごとに切り替える
/// </summary>
public class UIBlinker : MonoBehaviour {
    // /// <summary>
    // /// 透明度を一定時間ごとに切り替えるUIオブジェクト
    // /// </summary>
    // public GameObject UIObject;

    private float NextBlinkTime;
    private bool m_IsEnabled = true;
   
    /// <summary>
    /// 点滅間隔
    /// </summary>
    public float BlinkInterval = 1.0f;

    /// <summary>
    /// オブジェクトが見えるかどうか
    /// </summary>
    public bool Visible
    {
        get
        {
            var Renderer = GetComponent<CanvasRenderer>();
            return Renderer.GetAlpha() != 0.0f;
        }
        set
        {
            var Renderer = GetComponent<CanvasRenderer>();
            Renderer.SetAlpha(value ? 1.0f : 0.0f);
        }
    }

    /// <summary>
    /// 点滅が有効かどうか
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return m_IsEnabled;
        }
        set
        {
            // もし点滅を無効にするときは見えるようにしておく
            if (!value)
            {
                Visible = true;
            }

            m_IsEnabled = value;
        }
    }

	// Use this for initialization
	void Start () {
        // 設定をチェック
       // Debug.Assert(UIObject != null, new System.ArgumentNullException("UIObject は null であってはなりません。何らかのオブジェクトを設定してください。"));
        Debug.Assert(GetComponent(typeof(CanvasRenderer)) != null, new System.ArgumentException("設定されているオブジェクトが正しくありません。CanvasRenderer コンポーネントを持つオブジェクトである必要があります。"));

        NextBlinkTime = BlinkInterval;
	}
	
	// Update is called once per frame
	void Update () {
        if (IsEnabled)
        {
	        if (Time.time > NextBlinkTime)
            {
                // 描画コンポーネントを取得
                // var Renderer = UIObject.GetComponent<CanvasRenderer>();
                var Renderer = GetComponent<CanvasRenderer>();

                // もし透明度が 0 なら 1 にし、そうでなければ 0 にする。
                // Renderer.SetAlpha(Renderer.GetAlpha() == 0.0f ? 1.0f : 0.0f);
                Visible = Renderer.GetAlpha() == 0.0f;

                // 次の点滅時刻を設定
                NextBlinkTime = Time.time + BlinkInterval;
            }
        }
	}
}
