using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string key;
    public Sprite sprite;
    public virtual bool usable { get; set; }

    public virtual void Use() { }
}