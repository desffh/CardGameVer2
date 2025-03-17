using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPopUp : MonoBehaviour
{
    [SerializeField] GameObject Popup;

    private void Start()
    {
        Popup.SetActive(false);
    }

    public void OnEnter()
    {
        Popup.SetActive(true);
    }

    public void OnExit()
    {
        Popup.SetActive(false);
    }
}
