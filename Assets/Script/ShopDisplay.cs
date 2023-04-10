using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDisplay : MonoBehaviour
{

    [SerializeField]
    private int Price_first_product;

    [SerializeField]
    private int Price_second_product;

    [SerializeField]
    private int Price_third_product;

    public Button Fisrt_sell;
    public Button Second_sell;
    public Button Third_sell;

    public GameObject Prom;
    public TMP_Text Current_text;
    public TMP_Text Prom_text;

    private DbManager dbaccess;
    private int current_currency;


    private void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        getCurrency();
        getcharlist();

    }

    public void getcharlist()
    {
        var player_name = PhotonNetwork.NickName;
        string[] player_ctype = dbaccess.getPlayerCtypeList(player_name);
        foreach (string ctype in player_ctype)
        {
            if (ctype == "C01")
            {
                Fisrt_sell.interactable = false;
            }
            if (ctype == "C02")
            {
                Second_sell.interactable = false;
            }
            if (ctype == "C03")
            {
                Third_sell.interactable = false;
            }
        }
    }

    public void getCurrency()
    {
        var player_name = PhotonNetwork.NickName;
        current_currency = dbaccess.getPlayerCurrency(player_name);
        Current_text.text = ("Your Point: " + current_currency.ToString());
    }

    public void purchase(string charname, int price, string name)
    {
        if (-price > current_currency)
        {
            Prom.SetActive(true);
            Prom_text.text = "You Dont Have Enough Point!!!";
            Invoke("setPromFalse", 2.0f);
        }
        else
        {
            dbaccess.updateUSerCharList(charname, name);
            dbaccess.updateUserCurrencyData(name, price);
            Prom.SetActive(true);
            Prom_text.text = "Success!!!";
            getcharlist();
            getCurrency();
            Invoke("setPromFalse", 2.0f);
        }
    }

    public void setPromFalse()
    {
        Prom.SetActive(false);
    }

    public void OnClickProduct1()
    {
        var player_name = PhotonNetwork.NickName;
        var charname = "C01";
        purchase(charname, -Price_first_product, player_name);
    }
    public void OnClickProduct2()
    {
        var player_name = PhotonNetwork.NickName;
        var charname = "C02";
        purchase(charname, -Price_second_product, player_name);

    }
    public void OnClickProduct3()
    {
        var player_name = PhotonNetwork.NickName;
        var charname = "C03";
        purchase(charname, -Price_third_product, player_name);
    }







}
