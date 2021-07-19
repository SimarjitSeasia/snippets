using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LoginPage : PageView
{

    public InputField Email, Password;
    private MainMenu mainMenu;
    public Sprite Background;
    public UserDetails user = new UserDetails();


    public override void OnDidClose()
    {
        //StopAllCoroutines();
    }

    public override void OnDidOpen(object props)
    {
        Email.text = Password.text = "";
        mainMenu = GetComponentInParent<MainMenu>();
        if (Background != null)
        {
            mainMenu.ChangeBackground(Background);
        }
    }


    public override void Awake()
    {
        base.Awake();
        mainMenu = GetComponentInParent<MainMenu>();
    }



    public void OnClickForgot()
    {
        mainMenu.OpenPage(mainMenu.ForgotPage);
    }

    public void OnClickCreateAccount()
    {
        mainMenu.OpenPage(mainMenu.SignupPage);
    }

    public void AttemptAutoLogin(string email, string password)
    {
        print("login attempt " + email + password);
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        PopUpManager.Instance.LoadingStatus(true);

        var request = new WebServices(TravXUrls.LoginUrl, form);
        request.OnResponse += OnLoginResponse;
    }

    public void OnClickLogin()
    {
        var validator = Validate();
        if (validator == string.Empty)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", Email.text.Trim());
            form.AddField("password", Password.text.Trim());
            PopUpManager.Instance.LoadingStatus(true);
            PlayerPrefs.SetString(TravXConstants.PasswordSavedKey, Password.text.Trim());
            new WebServices(TravXUrls.LoginUrl, form).OnResponse += OnLoginResponse;
        }
        else
        {
            print("Validation, " + validator);
            PopUpManager.Instance.ShowToast(validator);
        }
    }

    private void OnLoginResponse(JSONNode response, string error)
    {
        if (error == null)
        {
            print(" " + response.ToString());
            //PopUpManager.Instance.ShowToast("Logged in sccuessfully");
            string token = response["access_token"];
            PlayerPrefs.SetString(TravXConstants.AccessTokenSavedKey, token);
            user.Id = response["user"]["id"];
            user.Email = response["user"]["email"];
            user.FirstName = response["user"]["firstname"];
            user.LastName = response["user"]["lastname"];

            user.DateofBirth = response["user"]["dob"];
            user.Height = response["user"]["height"];
            user.Weight = response["user"]["weight"];
            user.Gender = response["user"]["gender"];
            //user.Country = response["user"]["weight"];
            //user.State = response["user"]["weight"];


            if (!string.IsNullOrEmpty(response["user"]["social_id"]))
            {
                user.IsSocial = true;
                user.SocialId = response["user"]["social_id"];
                user.Platform = response["user"]["social_platform"];
            }

            PersistanceManager.SaveToPrefs(TravXConstants.UserDetailsKey, user);
            if (PopUpManager.Instance.GotoListing)
                mainMenu.OpenPage(mainMenu.TracksPage);
            else
                mainMenu.OpenPage(mainMenu.HomePage);

            TimeCalculator.Instance.StartTimer();

            AchievementManager.Instance.GetAchiements(() =>
       {
           string saved = PlayerPrefs.GetString(TravXConstants.LoggedInTime, "");
           if (!string.IsNullOrEmpty(saved))
           {
               var previousTime = Convert.ToDateTime(saved);
               var todayTime = DateTime.Now;
               var difference = todayTime - previousTime;
               if (difference.Days == 1)
                   AchievementManager.Instance.IncreaseAchivement(AchievementType.LogIn, 1);
               else if (difference.Days > 1)
                   AchievementManager.Instance.IncreaseAchivement(AchievementType.LogIn, 1, true);
               else
               {

               }
           }
           else
               AchievementManager.Instance.IncreaseAchivement(AchievementType.LogIn, 1, true);

           PlayerPrefs.SetString(TravXConstants.LoggedInTime, DateTime.Now.ToString());
           AchievementManager.Instance.SaveAllAchievements();
       });

        }
        else
        {
            var _user = PersistanceManager.GetSavedData<UserDetails>(TravXConstants.UserDetailsKey);

            print("Login Error" + error);
            if (error.Contains("Wrong credentials"))
                PopUpManager.Instance.ShowToast("You have entered an invalid email or password");
            else if (error.Contains("User Account Not Verified"))
            {
                PopUpManager.Instance.ShowToast(error);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                    mainMenu.OpenPage(mainMenu.VerifyPage, user.Email);
                else
                    mainMenu.OpenPage(mainMenu.VerifyPage, Email.text.Trim());
            }
            else
                PopUpManager.Instance.ShowToast(error);
        }
        mainMenu.Splash.CloseSplash();
        PopUpManager.Instance.LoadingStatus(false);

    }

    private string Validate()
    {

        if (Email.text.Length < 1)
            return "Please enter email address";
        if (!Regex.IsMatch(Email.text, TravXConstants.MatchEmailPattern))
            return "Please enter a valid email address";
        if (Password.text.Length < 1)
            return "Please enter password";
        if (!Regex.IsMatch(Password.text, TravXConstants.PasswordPattern))
            return "Password must contain uppercase/lowercase and numbers. Min 6 and max 30 characters.";
        return string.Empty;
    }

    public override void OnSystemBackEvent()
    {
        base.OnSystemBackEvent();
        mainMenu.OpenPage(mainMenu.EnterPage);
    }

    public void OnClickBackButton()
    {
        mainMenu.OpenPage(mainMenu.EnterPage);
    }
}
