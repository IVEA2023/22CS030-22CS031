using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class ForumControl : MonoBehaviour
{
    public GameObject forumPanel;
    public GameObject newTopic;

    public TMP_InputField newTopicTitle;
    public TMP_InputField newTopicContent;

    public Button viewButton;
    public GameObject viewContent;
    public GameObject Content;
    public GameObject contentText;
    public GameObject ContentText;
    public GameObject storage;

    public TMP_InputField replyString;
    public string tUid;

    private DbManager dbaccess;
    private ArrayList forumData;
   
    
    private void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        forumData = dbaccess.GetForumData();
        
        Initialization(forumData);
    }

    public void Initialization(ArrayList datas)
    {
        GameObject g = Content;
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(g.transform.GetChild(i).gameObject);
        }
        foreach (string[] data in datas)
        {
            var TopicObject = storage.GetComponent<Topic>();
            TopicObject.setUid(data[0].ToString());
            var convertTopic = data[1].ToString();
            convertTopic = convertTopic.Replace("***IamComma***", ",");
            convertTopic = convertTopic.Replace("***IamleftBracket***", "[");
            convertTopic = convertTopic.Replace("***IamrightBracket***", "]");
            TopicObject.setTopicTitle(convertTopic);
            Instantiate(viewContent, Vector3.zero, Quaternion.identity, Content.transform);

        }
    }

    public void viewTopic(string uid)
    {
        GameObject g = ContentText;
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(g.transform.GetChild(i).gameObject);
        }
        string suid = uid;
        tUid = suid;
        forumData = dbaccess.GetForumData();
        foreach (string[] data in forumData)
        {
            if (data[0].ToString().Equals(suid))
            {
                string[] sContent = data[3].Split(',');
                string[] sReply = data[4].Split(",");
                for(int i = 0;i < sContent.Length; i++) 
                {
                    var content = contentText.GetComponent<Content>();
                    sReply[i] = sReply[i].Replace("[", "");
                    sReply[i] = sReply[i].Replace("]", "");
                    sContent[i] = sContent[i].Replace("[", "");
                    sContent[i] = sContent[i].Replace("]", "");
                    sContent[i] = sContent[i].Replace("***IamComma***", ",");
                    sContent[i] = sContent[i].Replace("***IamleftBracket***", "[");
                    sContent[i] = sContent[i].Replace("***IamrightBracket***", "]");
                    content.setContentText(sReply[i] + " : " + sContent[i]);
                    Instantiate(contentText, Vector3.zero, Quaternion.identity, ContentText.transform);
                }
                
            }
        }
    }

    public void reply()
    {
        var getName = PhotonNetwork.NickName;
        string replyUser = getName;
        Debug.Log(replyUser);
        string replyContent = replyString.text;
        if(replyContent.Trim().Equals("") || replyContent == null)
        {
            replyString.text = "";
            viewTopic(tUid);
        }
        else
        {
            replyContent = replyContent.Replace(",", "***IamComma***");
            replyContent = replyContent.Replace("[", "***IamleftBracket***");
            replyContent = replyContent.Replace("]", "***IamrightBracket***");
            dbaccess.updateUserCurrencyData(replyUser, 1);
            dbaccess.setReply(tUid, replyContent, replyUser);
            replyString.text = "";
            viewTopic(tUid);
        }
        
    }
    public void createTopic()
    {
        string title = newTopicTitle.text;
        string content = newTopicContent.text;
        var getName = PhotonNetwork.NickName;
        string replyUser = getName;
        if(title.Trim().Equals("") || content.Trim().Equals("") || title== null || content==null)
        {
            newTopicTitle.text = "";
            newTopicContent.text = "";
            topicDeactivate();
            forumActivate();
            forumData = dbaccess.GetForumData();
            Initialization(forumData);
        }
        else
        {
            title = title.Replace(",", "***IamComma***.");
            title = title.Replace("[", "***IamleftBracket***");
            title = title.Replace("]", "***IamrightBracket***");
            content = content.Replace(",", "***IamComma***");
            content = content.Replace("[", "***IamleftBracket***");
            content = content.Replace("]", "***IamrightBracket***");
            dbaccess.updateUserCurrencyData(replyUser, 5);
            dbaccess.newTopic(title, replyUser, content);
            newTopicTitle.text = "";
            newTopicContent.text = "";
            topicDeactivate();
            forumActivate();
            forumData = dbaccess.GetForumData();
            Initialization(forumData);
        }
        
    }
    public void forumActivate()
    {
        forumPanel.SetActive(true);
    }

    public void forumDeactivate()
    {
        forumPanel.SetActive(false);
    }

    public void topicActivate()
    {
        newTopic.SetActive(true);
    }

    public void topicDeactivate()
    {
        newTopic.SetActive(false);
    }

    public void exitOnclick()
    {
        forumDeactivate();
    }

    public void changetoTopic()
    {
        forumDeactivate();
        topicActivate();
    }

    public void changetoForum()
    {
        forumActivate();
        topicDeactivate();
    }
    

}
