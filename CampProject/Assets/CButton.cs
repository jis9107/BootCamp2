using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CButton : MonoBehaviour
{
    [SerializeField]private Button button;
    public int index;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Addlistener(UnityAction listener)
    {
        button.onClick.AddListener(listener);
    }

    public void Removelistener(UnityAction listener)
    {
        button.onClick.RemoveListener(listener);
    }
}
