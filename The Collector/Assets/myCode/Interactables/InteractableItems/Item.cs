[System.Serializable]
public class Item 
{
    public string name;
    public int ID;
    public ItemObject itemObj;
    public int equipHandID;
    public bool isStackable;
    public Item(ItemObject itemObject)
    {
        itemObj = itemObject;
        name = itemObj.name;
        ID = itemObj.ID;
        isStackable = !itemObject.isEquipmentObject;
    }
    public void UpdateHandID() //not using 
    {
        equipHandID = itemObj.handID;
    }
}
