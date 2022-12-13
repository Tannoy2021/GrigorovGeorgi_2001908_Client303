using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public static UI instance;
    public GameObject startMenu;
    private void Awake()
    {
        if (instance == null)         instance = this;
        else if (instance != this)    Destroy(this);
    }
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        GameClient.instance.ConnectToServer();
    }
}
