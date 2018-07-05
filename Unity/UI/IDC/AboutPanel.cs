using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AboutPanel : MonoBehaviour {

    [System.Serializable]
    public class BtnData
    {
        public GameObject btn;
        public Sprite selectedSprite;
        public Sprite defSprite;

    }
    public List<BtnData> btnDatas = new List<BtnData>();
    //public GameObject map, briefIntroduction, impetus, envir, back;
    public GameObject backGround;

    private GameObject selectBtn;
    public static AboutPanel instance;
    // public Sprite map_sprite, briefIntroduction_sprite, impetus_sprite, refrigeration_sprite;
    public Texture  briefIntroduction_spriteBG;
    public MovieTexture map_spriteBG, impetus_spriteBG,refrigeration_spriteBG, refrigeration_sprite2BG;
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        int c = btnDatas.Count;
        for (int i = 0; i < c; i++)
        {
            EventTriggerListener.Get(btnDatas[i].btn).onClick = OnClick;
            EventTriggerListener.Get(btnDatas[i].btn).onEnter = OnEnter;
            EventTriggerListener.Get(btnDatas[i].btn).onExit = OnExit;
            Color color = btnDatas[i].btn.GetComponent<Image>().color;
         //   btnDatas[i].btn.GetComponent<Image>().sprite = btnDatas[i].defSprite;
            btnDatas[i].btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.8f);
        }
    }
    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        backGround.GetComponent<RawImage>().texture = map_spriteBG;
        TexturePlay(map_spriteBG);
    }

    public void TabSwitchReset()
    {
        int c = btnDatas.Count;
        for (int i = 0; i < c; i++)
        {
            Color color = btnDatas[i].btn.GetComponent<Image>().color;
            btnDatas[i].btn.GetComponent<Image>().sprite = btnDatas[i].defSprite;
            btnDatas[i].btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.8f);
        }
    }
    public void ResetBTNSprite()
    {
        
        int c = btnDatas.Count;
        for (int i = 0; i < c; i++)
        {
            if (selectBtn)
            {
                if (btnDatas[i].btn.name != selectBtn.name)
                {
                    Color color = btnDatas[i].btn.GetComponent<Image>().color;
                    //btnDatas[i].btn.GetComponent<Image>().sprite = btnDatas[i].defSprite;
                    btnDatas[i].btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.8f);
                }
            }
            else
            {
                Color color = btnDatas[i].btn.GetComponent<Image>().color;
                //btnDatas[i].btn.GetComponent<Image>().sprite = btnDatas[i].defSprite;
                btnDatas[i].btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.8f);
            }
        }
    }
  
    public void OnEnter(GameObject go)
    {
        ResetBTNSprite();
        int c = btnDatas.Count;
        for (int i = 0; i < c; i++)
        {
            if (btnDatas[i].btn.name == go.name)
            {
                Color color = btnDatas[i].btn.GetComponent<Image>().color;
                btnDatas[i].btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
            }
        }
    }
    public void OnExit(GameObject go)
    {
        ResetBTNSprite();
    }

    public void OnClick(GameObject go)
    {
        TabSwitchReset();
        int c = btnDatas.Count;
        for (int i = 0; i < c; i++)
        {
            if (go.name == btnDatas[i].btn.name)
            {
                selectBtn = go;
                Color color = btnDatas[i].btn.GetComponent<Image>().color;
                btnDatas[i].btn.GetComponent<Image>().sprite = btnDatas[i].selectedSprite;
                btnDatas[i].btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
            }
        }
        if (go.name == "Map")
        {
            backGround.GetComponent<RawImage>().texture= map_spriteBG;
            TexturePlay(map_spriteBG);
        }
        else if (go.name == "BriefIntroduction")
        {
            backGround.GetComponent<RawImage>().texture= briefIntroduction_spriteBG;
        }
        else if (go.name == "Impetus")
        {
            backGround.GetComponent<RawImage>().texture = impetus_spriteBG;
            TexturePlay(impetus_spriteBG);
        }
        else if (go.name == "refrigeration")
        {
            backGround.GetComponent<RawImage>().texture = refrigeration_spriteBG;
            TexturePlay(refrigeration_spriteBG);
        }
        else if (go.name == "refrigeration2")
        {
            backGround.GetComponent<RawImage>().texture = refrigeration_sprite2BG;
            TexturePlay(refrigeration_sprite2BG);
        }
        else if (go.name == "Back")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    void TexturePlay(MovieTexture texture) {
        texture.loop = true;
        texture.Stop();
        texture.Play();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
