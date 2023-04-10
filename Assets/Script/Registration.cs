using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MongoDB.Bson;
using MongoDB.Driver;

public class Registration : MonoBehaviour
{

    private DbManager dbaccess;

    [SerializeField]
    private GameObject login_interafce;

    [SerializeField]
    private GameObject registation_interafce;

    [SerializeField]
    private TMP_Text reg_username;

    [SerializeField]
    private TMP_Text reg_password;

    [SerializeField]
    private TMP_Text reg_comfirmpassword;

    [SerializeField]
    private Button regis_Button;

    [SerializeField]
    private Button cancel_Button;

    [SerializeField]
    private GameObject returntoLogin_Button;

    [SerializeField]
    private GameObject Prom;

    [SerializeField]
    private TMP_Text Prom_text;

    void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
    }

    public bool check_password(string pass_reg, string conpass_reg)
    {
        if (pass_reg.Equals(conpass_reg))
        {
            return true;
        }
        Prom.SetActive(true);
        Prom_text.text = "The Password Input Different";
        Invoke("setPromFalse", 2.0f);
        reg_password.text = "";
        reg_comfirmpassword.text = "";
        return false;
    }

    public bool check_usename_exist(string name_reg)
    {
        var userDatas = dbaccess.GetUserData();
        foreach (string[] userData in userDatas)
        {
            if (userData[0].Equals(name_reg))
            {
                Prom.SetActive(true);
                Prom_text.text = "Username exist! Please Change A New One!";
                Invoke("setPromFalse", 2.0f);
                reg_username.text = "";

                return true;
            }
        }
        return false;
    }

    public void setPromFalse()
    {
        Prom.SetActive(false);
    }

    public void OnClickRegistration()
    {
        var name_reg = reg_username.text.Trim((char)8203);
        var pass_reg = reg_password.text.Trim((char)8203);
        var conpass_reg = reg_comfirmpassword.text.Trim((char)8203);

        bool pass_check = check_password(pass_reg, conpass_reg);
        bool name_check = check_usename_exist(name_reg);

        if (!name_check)
        {
            if (pass_check)
            {

                dbaccess.Registration(name_reg, pass_reg);

                Prom.SetActive(true);
                Prom_text.text = "Registration Successfully!!";
                Invoke("setPromFalse", 2.0f);

                regis_Button.interactable = false;
                cancel_Button.interactable = false;
                returntoLogin_Button.SetActive(true);


            }
        }
    }

    public void OnClickCancel()
    {
        reg_username.text = " ";
        reg_password.text = " ";
        reg_comfirmpassword.text = " ";
    }

    public void OnClickBackToLogin()
    {
        registation_interafce.SetActive(false);
        login_interafce.SetActive(true);
    }

}
