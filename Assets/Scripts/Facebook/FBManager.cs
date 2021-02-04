using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBManager : MonoBehaviour
{
    #region Singleton
    public static FBManager instance;

    private void SingletonStart()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public Text textDisplay;

    private void Awake()
    {
        SingletonStart();
        if (!FB.IsInitialized)
        {
            FB.Init(OnFBInitialize, OnHideFB);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnFBInitialize()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("FB Done Init");
        }
        else
        {
            Debug.Log("Failed to Init FB");
        }
    }

    private void OnHideFB(bool shown)
    {
        if (shown)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    private void FBLoginDone(ILoginResult res)
    {
        if (FB.IsLoggedIn)
        {
            GetUserData();
            Debug.Log("Success User Login");
        }
        else
        {
            Debug.LogError("User Failed to Login");
        }
    }

    public void LoginFB()
    {
        if (FB.IsInitialized)
        {
            if (!FB.IsLoggedIn)
            {
                List<string> permissions = new List<string>() { "public_profile", "email" };
                FB.LogInWithReadPermissions(permissions, FBLoginDone);
            }
            else
            {
                Debug.Log("User already Logged in!");
                GetUserData();
            }
        }
        else
        {
            Debug.LogError("FB not initialized!");
        }
    }

    private void GetUserDataDone(IGraphResult res)
    {
        if (string.IsNullOrEmpty(res.Error))
        {
            string user_name = res.ResultDictionary["name"].ToString();
            string email = res.ResultDictionary["email"].ToString();

            textDisplay.text = $"Name: {user_name}\nEmail: {email}";
        }
        else
        {
            Debug.LogError("Error getting user data");
        }
    }

    private void GetUserData()
    {
        string emailQuery = "/me?fields=name,email";
        FB.API(emailQuery, HttpMethod.GET, GetUserDataDone);
    }

    private void UploadPhotoDone(IGraphResult res)
    {
        if (string.IsNullOrEmpty(res.Error))
        {
            Debug.Log("Uploaded Photo with id: " + res.ResultDictionary["id"].ToString());
        }
        else
        {
            Debug.Log("Error Uploading Photo" + res.Error);
        }
    }

    IEnumerator ScreenshotAndUpload()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screen = ScreenCapture.CaptureScreenshotAsTexture();

        byte[] screenBytes = screen.EncodeToPNG();
        WWWForm form = new WWWForm();

        form.AddBinaryData("image", screenBytes, "screenshot.png");
        form.AddField("caption", "Space Guy Shooting");

        FB.API("me/photos", HttpMethod.POST, UploadPhotoDone, form);

        Debug.Log("Uploading Image");
    }

    public void UploadScreenshot()
    {
        if (FB.IsLoggedIn)
        {
            StartCoroutine(ScreenshotAndUpload());
        }
        else
        {
            Debug.Log("Not Logged In");
        }
    }
}
