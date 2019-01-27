
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JButton : MonoBehaviour
{
    private List<UnityAction> actions = new List<UnityAction>();
    private Button button;

    void Start()
    {
        this.button = GetComponent<Button>();
    }

    public bool Interactable
    {
        get
        {
            return this.button.interactable;
        }

        set
        {
            this.button.interactable = value;
        }
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void SetAction(UnityAction action)
    {
        this.RemoveActions();

        this.actions.Add(action);

        this.button.onClick.AddListener(action);
    }

    public void RemoveActions()
    {
        foreach (var action in this.actions)
        {
            this.button.onClick.RemoveListener(action);
        }

        this.actions.Clear();
    }

  
}
