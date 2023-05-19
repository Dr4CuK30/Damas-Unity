using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{

    public TMP_InputField userInput;
    public TMP_InputField passwordInput;

    public class LoginApiResponse
    {
        public string access_token { get; set; }
    }


    public void HandleLogin()
    {
        StartCoroutine(SignIn());
    }
    
    IEnumerator SignIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", userInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/login", form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            LoginApiResponse res= JsonConvert.DeserializeObject<LoginApiResponse>(jsonResponse);
            PlayerPrefs.SetString("Token", res.access_token);
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Error al hacer la petición: " + www.error);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}