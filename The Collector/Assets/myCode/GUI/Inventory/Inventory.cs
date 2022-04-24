using System.Collections.Generic;
[System.Serializable]
public class Inventory //this is just for visualization I might combine with inventory object in future. 
{
    public InventorySlot[] inventoryItemSlots = new InventorySlot[9]; //I chose array over list structure because I could can prebuild an inventory before its being used more easily with array
    public Item leftHandItem;
    public Item rightHandItem;
}
