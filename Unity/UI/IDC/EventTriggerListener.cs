using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;
	public VoidDelegate onDoubleClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onEndDrag;
    public VoidDelegate onDrag;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onSubmit;

    float lastTime ;

	static public EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}
	public override void OnPointerClick(PointerEventData eventData)
	{
		if(onClick != null) 
		{
			onClick(gameObject);
		}
			
		if( onDoubleClick != null )
		{
			float currentTime = Time.time ;
			if( ( currentTime - lastTime ) < 1f )
			{
				onDoubleClick(gameObject);
			}
			lastTime = currentTime;
		}
	}


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onDown != null) onSubmit(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject);
    }
    public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}
	public override void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject);
        
	}
	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}


}

/*
//// 使用案例
/// 
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class UIMain : MonoBehaviour {
	Button	button;
	Image image;
	void Start () 
	{
		button = transform.Find("Button").GetComponent<Button>();
		image = transform.Find("Image").GetComponent<Image>();
		EventTriggerListener.Get(button.gameObject).onClick =OnButtonClick;
		EventTriggerListener.Get(image.gameObject).onClick =OnButtonClick;
	}
	
	private void OnButtonClick(GameObject go){
		//在这里监听按钮的点击事件
		if(go == button.gameObject){
			Debug.Log ("DoSomeThings");
		}
	}
}

*/