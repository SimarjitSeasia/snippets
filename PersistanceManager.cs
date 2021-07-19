using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistanceManager
{
    public static void SaveToPrefs<T>(string keypath, T Data)
    {
        string saveValue = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(keypath, saveValue);
    }

    public static T GetSavedData<T>(string keypath)
    {
        string json = PlayerPrefs.GetString(keypath);
        return JsonUtility.FromJson<T>(json);
    }
}

public static class DementiaUrls
{
    const string baseUrl = "http://apptest.omniacare.com/WSCartellaClinica/Rehab-Dem/TestDementia/api/";
    public static readonly string LoginUrl = baseUrl + "Login";
    public static readonly string ListofTestUrl = baseUrl + "getUserTestList";
    public static readonly string TestDetailsUrl = baseUrl + "getTestDetails";
    public static readonly string SendResultUrl = baseUrl + "putUserTestResponse";
    public static readonly string GetSummaryTest = baseUrl + "getSummaryTests";
}

public static class DementiaPaths
{
    const string SavedPath = "";
    public const string UserDetailsSaveString = "User_Details_Saved_Key";
}

#region Custom Classes
public class TestResult
{
    public int idQuestion;
    public string answer;
    public int numberAttempts;
    public int numberHelp;
    public int numberReinforcement;
    public string timeResponse;
    public int numberErrorAnswer;
}

public class UserDetails
{
    public string Name;
    public string Password;
}

public class Test
{
    public string name;
    public TestCategories idCategoryTest;
    public int idTest;
    public System.DateTime date;
    public Templates idTemplate;
    public bool mandatory;
}

public enum TestCategories
{
    MemoryTest = 1,
    ShortMemory =2,
    Labyrinth =3
}

public enum Templates
{
    QuestionAnswers = 1,
    Falana = 2,
    Dhimkana = 3
}
#endregion