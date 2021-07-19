using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageView : MonoBehaviour
{
    private object Props;

    public virtual void Awake()
    {

    }
    public virtual void Start()
    {
    }

    private void OnEnable()
    {
        OnDidOpen(Props);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PopUpManager.Instance.isPopUpShown)
            {
                PopUpManager.Instance.ClosePopUp();
            }
            else
            {
                OnSystemBackEvent();
            }
        }
    }
    public virtual void OnSystemBackEvent()
    {
    }

    public abstract void OnDidOpen(object props);

    public abstract void OnDidClose();

    public void Open(object props = null)
    {
        Props = props;
        gameObject.SetActive(true);
        //OnDidOpen(Props);
    }

    public void Close()
    {
        OnDidClose();
        gameObject.SetActive(false);
    }
}
