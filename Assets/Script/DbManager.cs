using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Unity.VisualScripting.FullSerializer;

public class DbManager: MonoBehaviour
{

    void Start()
    {
        //InsertData();
        //ViewData();
        //GetQuestionsFromDB();
    }

    private IMongoCollection<BsonDocument> DatabaseConn(string collName)
    {

        var client = new MongoClient("mongodb+srv://IVEA:FinishFYP@cluster0.kup5tz8.mongodb.net/?retryWrites=true&w=majority");
        var database = client.GetDatabase("IVEA");
        var collection = database.GetCollection<BsonDocument>(collName);

        return collection;
    }

    public void InsertData()
    {
        var document = new BsonDocument{
        {"username","peter"},
        {"password",123456}
    };
        Debug.Log("Worte");
        string collName = "UserData";
        var collection = DatabaseConn(collName);
        collection.InsertOne(document);
    }

    public void ViewData()
    {
        string collName = "UserData";
        var collection = DatabaseConn(collName);
        var filter = new BsonDocument();
        var list = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
        list.ForEach(p =>
        {
            Console.WriteLine("name�G" + p["username"].ToString() + ",pw:" + p["password"].ToString());
            Debug.Log("name�G" + p["username"].ToString() + ",pw:" + p["password"].ToString());
        });
        Debug.Log("Viewed");


    }

    public List<string> GetQuestionsFromDB()
    {
        string collName = "QuestionBank";
        var collection = DatabaseConn(collName);
        var filter = new BsonDocument();
        List<string> questionbank = new List<string>();
        var allquestions = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
        allquestions.ForEach(p =>
        {
            // Debug.Log("[InsertIntoDB] | "+"question�G" + p["Question"].ToString() + ",corr:" + p["CorrAns"].ToString() + ",w1:" + p["WrongAns1"].ToString() + ",w2:" + p["WrongAns2"].ToString());


            var line = p["Question"].ToString() + "," + p["CorrAns"].ToString() + "," + p["WrongAns1"].ToString() + "," + p["WrongAns2"].ToString();
            questionbank.Add(line);
        });
        return questionbank;
    }

    public ArrayList GetUserData()
    {
        string collName = "UserData";
        var collection = DatabaseConn(collName);
        var filter = new BsonDocument();
        ArrayList userDatas = new ArrayList();
        var allUserDatas = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
        allUserDatas.ForEach(p =>
        {
            string[] userData = new string[6];
            // Debug.Log("[InsertIntoDB] | "+"question�G" + p["Question"].ToString() + ",corr:" + p["CorrAns"].ToString() + ",w1:" + p["WrongAns1"].ToString() + ",w2:" + p["WrongAns2"].ToString());
            userData[0] = p["username"].ToString();
            userData[1] = p["password"].ToString();
            userData[2] = p["uid"].ToString();
            userData[3] = p["currency"].ToString();
            userData[4] = p["cType"].ToString();
            userData[5] = p["IsLogin"].ToString();
            //Debug.Log(userData[0] + "db");
            userDatas.Add(userData);
        });
        return userDatas;
    }


    public void newTopic(string topic, string username, string content)
    {
        string collName = "ForumData";
        var collection = DatabaseConn(collName);
        var filter = new BsonDocument();

        ArrayList tmp = GetForumData();
        Debug.Log(tmp.Count);
        int count = tmp.Count + 1;
        string uid = count.ToString();

        var document = new BsonDocument{
        {"uid",uid},
        {"topic",topic},
        {"post", username},
        {"content",new BsonArray{content } },
        {"reply",new BsonArray{username } } 
    }; 
        collection.InsertOne(document);
    }
    public ArrayList GetForumData()
    {
        string collName = "ForumData";
        var collection = DatabaseConn(collName);
        var filter = new BsonDocument();
        ArrayList forumDatas = new ArrayList();
        var allUserDatas = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
        allUserDatas.ForEach(p =>
        {
            string[] forumData = new string[5];
            // Debug.Log("[InsertIntoDB] | "+"question�G" + p["Question"].ToString() + ",corr:" + p["CorrAns"].ToString() + ",w1:" + p["WrongAns1"].ToString() + ",w2:" + p["WrongAns2"].ToString());
            forumData[0] = p["uid"].ToString();
            forumData[1] = p["topic"].ToString();
            forumData[2] = p["post"].ToString();
            forumData[3] = p["content"].ToString();
            forumData[4] = p["reply"].ToString();
            //Debug.Log(forumData[0] + "db");
            forumDatas.Add(forumData);
        });
        return forumDatas;
    }

    public void setReply(string uid,string content,string reply)
    {
        string collName = "ForumData";
        var collection = DatabaseConn(collName);
        var filter = Builders<BsonDocument>.Filter.Eq("uid", uid);
        var updateContent = Builders<BsonDocument>.Update.Push("content", content);
        var updateReply = Builders<BsonDocument>.Update.Push("reply", reply);
        collection.UpdateOne(filter, updateContent);
        collection.UpdateOne(filter, updateReply);
    }

    public void updateUserCurrencyData(string uid, int newmoney) 
    {
        string collName = "UserData";
        var collection = DatabaseConn(collName);
        var filter = Builders<BsonDocument>.Filter.Eq("username", uid);

        var userDatas = GetUserData();

        var new_money = 0;

        foreach (string[] userData in userDatas)
        {

            if (userData[0].Equals(uid))
            {
                new_money = newmoney + int.Parse(userData[3]);
            }
        }


        var newCurrency = Builders<BsonDocument>.Update.Set("currency", new_money);
        collection.UpdateOne(filter, newCurrency);
    }

    public int getPlayerCurrency(string name)
    {
        int currency = 0;

        var userDatas = GetUserData();

        foreach (string[] userData in userDatas)
        {

            if (userData[0].Equals(name))
            {
                currency = int.Parse(userData[3]);
            }
        }

        return currency;
    }

    public void updateUSerCharList(string chraname, string name)
    {
        string collName = "UserData";
        var collection = DatabaseConn(collName);
        var filter = Builders<BsonDocument>.Filter.Eq("username", name);

        var newchra = Builders<BsonDocument>.Update.Push("cType", chraname);
        collection.UpdateOne(filter, newchra);
    }

    public string[] getPlayerCtypeList(string name)
    {
        var userDatas = GetUserData();

        var cType = "";

        foreach (string[] userData in userDatas)
        {

            if (userData[0].Equals(name))
            {
                cType = userData[4];
            }
        }
        string[] cTypeList = cType.Split(',');

        for(int i = 0; i < cTypeList.Length; i++)
        {
            cTypeList[i] = cTypeList[i].Replace("[", "");
            cTypeList[i] = cTypeList[i].Replace("]", "");
            cTypeList[i] = cTypeList[i].Trim();
        }

        return cTypeList;
    }

    public void Registration(string name, string password)
    {
        string collName = "UserData";
        var collection = DatabaseConn(collName);

        var userDatas = GetUserData();

        var document = new BsonDocument{
        {"username", name},
        {"password", password},
        {"uid", (userDatas.Count+1).ToString()},
        {"name", name.ToUpper()},
        {"cType", new BsonArray { } },
        {"currency", 0},
        {"IsLogin", "1"} };

        collection.InsertOne(document);
    }

    public void updateLogin(string uName, string IsLogin)
    {
        string collName = "UserData";
        var collection = DatabaseConn(collName);
        var filter = Builders<BsonDocument>.Filter.Eq("username", uName);
        var updateLogin = Builders<BsonDocument>.Update.Set("IsLogin", IsLogin);
        collection.UpdateOne(filter, updateLogin);
    }
}