using UnityEngine;

/// <summary>
/// Represent basic in game items. 
/// Note: This is not currently used. Class was created for an earlier version and may be re-used pending new featues.
/// </summary>
public class BasicItem : MonoBehaviour
{
    // Public vars set in Unity UI.

    /// <summary>
    /// Name of the item.
    /// </summary>
    public string ItemName;

    /// <summary>
    /// Struct containing information about the item.
    /// </summary>
    private Item itemInfo;

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
    {
        itemInfo = new Item(ItemName);
    }

    /// <summary>
    /// Return information about this item.
    /// </summary>
    /// <returns>Struct holding item information</returns>
    public Item GetItemInfo() {
        return itemInfo;
    }
}

/// <summary>
/// Placeholder for more robust item system. Currently only name is stored.
/// </summary>
public struct Item
{
    public string itemName;

    public Item(string name)
    {
        this.itemName = name;
    }
}
