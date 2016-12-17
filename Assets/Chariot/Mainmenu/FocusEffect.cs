using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Collections;

public class FocusEffect : MonoBehaviour
{
    private Image MyImage;
    /// <summary>
    /// マウスカーソルが自分自身の上にない時の色
    /// </summary>
    public Color UnfocusedColor = Color.gray;
    /// <summary>
    /// マウスカーソルが自分自身の上にある時の色
    /// </summary>
    public Color FocusedColor = Color.white;
    /// <summary>
    /// 色を変える速さ
    /// </summary>
    public float ChangeTime = 0.5f;
    /// <summary>
    /// マウスクリック時にやりたいこと
    /// </summary>
    public UnityEvent OnMouseDownHandler;

    // Use this for initialization
    void Start()
    {
        MyImage = GetComponent<Image>();
        MyImage.CrossFadeColor(UnfocusedColor, 0.0f, false, false);

        // イベントトリガーを追加して、マウス関連のイベントを拾えるようにする。
        var Trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry OnMouseEnterCallback = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
        OnMouseEnterCallback.callback.AddListener((data) => { OnMouseEnter(); });
        Trigger.triggers.Add(OnMouseEnterCallback);

        EventTrigger.Entry OnMouseExitCallback = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit };
        OnMouseExitCallback.callback.AddListener((data) => { OnMouseExit(); });
        Trigger.triggers.Add(OnMouseExitCallback);

        EventTrigger.Entry OnMouseDownCallback = new EventTrigger.Entry() { eventID = EventTriggerType.PointerDown };
        OnMouseDownCallback.callback.AddListener((data) => { OnMouseDown(); });
        Trigger.triggers.Add(OnMouseDownCallback);
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    public void OnMouseEnter()
    {
        MyImage.CrossFadeColor(FocusedColor, ChangeTime, false, false);
    }

    public void OnMouseExit()
    {
        MyImage.CrossFadeColor(UnfocusedColor, ChangeTime, false, false);
    }

    public void OnMouseDown()
    {
        OnMouseDownHandler.Invoke();
    }
}
