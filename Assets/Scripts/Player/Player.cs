using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<string> inventory = new List<string>();


    public void AddItemToInventory(string item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
        }
        else
        {
        }
    }

    public bool HasItem(string item)
    {
        return inventory.Contains(item);
    }

 
}
