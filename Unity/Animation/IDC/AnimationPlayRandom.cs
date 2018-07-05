using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationPlayRandom : MonoBehaviour {

    public static AnimationPlayRandom instance;

    [System.Serializable]
    public class AnimationClipItem 
    {
        public string clipItemName;
        public List<AnimationClip> animationClips;
    }

    public List<AnimationClipItem> animationClipItemList;
   
    public bool isPlayLoop = false;
    public bool isPlayRandom = false;

    private bool isStartPlay = false;
    private Animation animation;
    private AnimationClip playClip;

    //指定的 AnimationClipItem 成员
    private List<AnimationClip> animationClipList=new List<AnimationClip>();
    private string animationClipName;

    //动画播放序号数组
    private List<int> playArray=new List<int>();
   
    void Awake() {
        instance = this;
        animation = this.GetComponent<Animation>();
    }
    public void Stop()
    {
        animation.Stop();
        instance.enabled = false;
    }
    void Start()
    {
        //AnimationPlayByRandom("WAIJING");
    }

    List<int> GetRandomList(int count) {
        List<int> randomList = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int j = Random.Range(0, count);
            while (randomList.Contains(j))
            {
                j = Random.Range(0, count);
            }
            randomList.Add(j);
        }
        return randomList;
    }

    public void AnimationPlayByRandom(string clipItemName) {

        instance.enabled = true;
        if (GetComponent<AerialCamera>().enabled) {
            GetComponent<AerialCamera>().enabled = false;
        }

        animationClipList.Clear();
        isStartPlay = false;

        if (!isStartPlay) {
            for (int i = 0; i < animationClipItemList.Count; i++)
            {
                if (animationClipItemList[i].clipItemName == clipItemName)
                {
                    for (int j = 0; j < animationClipItemList[i].animationClips.Count; j++)
                    {
                        animationClipList.Add(animationClipItemList[i].animationClips[j]);
                    }
                    animationClipName = animationClipItemList[i].clipItemName;
                }
            }

            if (isPlayRandom)
            {
                playArray = GetRandomList(animationClipList.Count);
            }
            else
            {
                for (int i = 0; i < animationClipList.Count; i++)
                {
                    animationClipList[i].wrapMode = WrapMode.Once;
                    playArray.Add(i);
                }
            }
        }

        Debug.Log("playArray.Count:"+ playArray.Count);

        isStartPlay = true;
        playClip = animationClipList[playArray[0]];
        animation.AddClip(playClip,playClip.name);
        animation.Play(playClip.name);
    }

    void Update()
    {
        if (isStartPlay&&playArray.Count  >0 && !animation.isPlaying)
        {
            playArray.Remove(playArray[0]);
            if (playArray.Count > 0)
            {
                AnimationPlayByRandom(animationClipName);
            }
            else {
                if (isPlayLoop)
                {
                    if (isPlayRandom)
                    {
                        playArray = GetRandomList(animationClipList.Count);
                    }
                    else
                    {
                        for (int i = 0; i < animationClipList.Count; i++)
                        {
                            playArray.Add(i);
                        }
                    }
                    AnimationPlayByRandom(animationClipName);
                }
                else {
                    isStartPlay = false;
                }    
            }
        }
    }
}
