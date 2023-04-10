using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class Login : MonoBehaviour
{
    private DbManager dbaccess;
    TextMeshProUGUI userText;
    TextMeshProUGUI passwordText;

    [SerializeField]
    private GameObject usernameInput_text;

    [SerializeField]
    private GameObject passwordInput_text;

    [SerializeField]
    private TMP_InputField usernameInput;

    [SerializeField]
    private TMP_InputField passwordInput;

    [SerializeField] 
    private Button loginButton;

    [SerializeField]
    private GameObject Prom;

    [SerializeField]
    private TMP_Text Prom_text;

    [SerializeField]
    private GameObject registration_interface;

    [SerializeField]
    private GameObject login_interafce;

    public static string login_playername;
    public int InputSelected;

    void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputSelected++;
            if (InputSelected > 1) InputSelected = 0;
            SelectInputField();
        }

        void SelectInputField()
        {
            switch (InputSelected)
            { 
                case 0: usernameInput.Select(); break;
                case 1: passwordInput.Select(); break;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            loginButton.onClick.Invoke();

        }
    }

    public void ShowInput()
    {
        loginButton.interactable = false;

        userText = usernameInput_text.GetComponent<TextMeshProUGUI>();
        passwordText = passwordInput_text.GetComponent<TextMeshProUGUI>();

        string username = userText.text.ToString();
        string password = passwordText.text.ToString();

        bool loginStatus = Authentication(username, password);



        if (loginStatus == true)
        {

            bool accountStatus = checkthePlayerLogined(username);

            if (accountStatus)
            {
                Prom.SetActive(true);
                Prom_text.text = "This Account Already Login";
                Invoke("setPromFalse", 2.0f);
                loginButton.interactable = true;
            }
            else
            {
                dbaccess.updateLogin(username.Trim((char)8203), "0");
                moveToLobby();
            }
        }
        else
        {
            Prom.SetActive(true);
            Prom_text.text = "Your Username/Password Incorrect";
            Invoke("setPromFalse", 2.0f);
            loginButton.interactable = true;
        }


        
    }
    public void setPromFalse()
    {
        Prom.SetActive(false);
    }

    public void moveToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }



    public bool Authentication(string userName, string password)
    {
        var userDatas = dbaccess.GetUserData();

        foreach (string[] userData in userDatas)
        {
            string a = userName.Trim((char)8203);
            string b = password.Trim((char)8203);
            if (userData[0].Equals(a) & userData[1].Equals(b))
            {
                login_playername = a;
                return true;
            }
        }
        return false;
    }

    public bool checkthePlayerLogined(string userName) 
    {
        var userDatas = dbaccess.GetUserData();
        string a = userName.Trim((char)8203);

        foreach (string[] userData in userDatas)
        {
            
            if (userData[0].Equals(a))
            {
                if (userData[5] == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    public static string GetUserName()
    {
        return login_playername;
    }

    public void SetUserName(string name)
    {
        login_playername = name;
    }

    public void OnlickRegistration()
    {
        login_interafce.SetActive(false);
        registration_interface.SetActive(true);
    }

}
