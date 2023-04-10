using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Topic : MonoBehaviour
{
    public ForumControl ForumControlParent;

    [SerializeField]
    public TMP_Text topicTitle;
    [SerializeField]
    public TMP_Text storeUid;
    

    public void setTopicTitle(string title)
    {
        topicTitle.text = title;
    }

    public void setUid(string uid)
    {
        storeUid.text = uid;
    }

    public string getTopicTitle()
    {
        return topicTitle.text;
    }

    public string getUid()
    {
        return storeUid.text;
    }

    public void topicOnClicked()
    {
        var ClickToView = FindObjectOfType<ForumControl>();
        ClickToView.viewTopic(storeUid.text);
    }
}
