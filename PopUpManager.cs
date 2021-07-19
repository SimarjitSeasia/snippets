using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour {
    public static PopUpManager Instance;
    void Awake () {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
	}

    [SerializeField]
    GameObject PopupWindow;

    [SerializeField]
    Text HeaderText;

    [SerializeField]
    Text MessageText;

    [SerializeField]
    Button PositiveResponse;

    [SerializeField]
    Button NegativeResponse;

    public void ShowPopUp(string Title, string Message)
    {
        ShowPopUp(Title, Message);
        PositiveResponse.GetComponentInChildren<Text>().text = "Ok";
        NegativeResponse.gameObject.SetActive(false);
    }
    public void ShowPopUp(string Title, string Message, UnityAction onClickOk)
    {
        ShowPopUp(Title, Message, onClickOk, null);
        PositiveResponse.GetComponentInChildren<Text>().text = "Ok";
    }

    public void ShowPopUp(string Title, string Message, UnityAction onClickYes, UnityAction onClickNo)
    {
        PopupWindow.SetActive(true);
        HeaderText.text = Title;
        MessageText.text = Message;

        NegativeResponse.gameObject.SetActive(false);

        PositiveResponse.onClick.RemoveAllListeners();
        NegativeResponse.onClick.RemoveAllListeners();

        if (onClickYes != null)
        {
            PositiveResponse.onClick.AddListener(() =>
            {
                ClosePopUp();
                onClickYes();
            });
        }
        else
            PositiveResponse.onClick.AddListener(ClosePopUp);

        if (onClickNo != null)
        {
            NegativeResponse.gameObject.SetActive(true);
            NegativeResponse.onClick.AddListener(() =>
            {
                ClosePopUp();
                onClickNo();
            });
        }else
            NegativeResponse.gameObject.SetActive(false);
    }

    void ClosePopUp()
    {
        PopupWindow.SetActive(false);
    }
}
