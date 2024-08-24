using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject optionsPanel;

    void Start()
    {
        optionsPanel.SetActive(false);
    }

    public void ShowOptionsPanel()
    {
        optionsPanel.SetActive(true); 
    }

    public void HideOptionsPanel()
    {
        optionsPanel.SetActive(false); 
    }
}