using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiControl : MonoBehaviour
{
    public static UiControl Instance;

  public GameObject fKeyPanel;
    public GameObject gKeyPanel;

    public void GetFkeyPanel(bool value)
    {
        fKeyPanel.SetActive(value);
    }
    public void GetGkeyPanel(bool value)
    {
        gKeyPanel.SetActive(value);
    }


}
