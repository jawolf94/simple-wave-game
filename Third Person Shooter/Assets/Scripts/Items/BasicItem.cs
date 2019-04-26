using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : MonoBehaviour
{
    public string ItemName;

    private Item itemInfo;

    // Start is called before the first frame update
    void Start()
    {
        itemInfo = new Item(ItemName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item GetItemInfo() {
        return itemInfo;
    }
}

//Placeholder for more robust item system
public struct Item
{
    public string itemName;

    public Item(string name)
    {
        this.itemName = name;
    }
}
