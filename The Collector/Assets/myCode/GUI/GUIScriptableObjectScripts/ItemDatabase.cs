using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "New Item Database", menuName ="InventorySystem/Items/Database")]
public class ItemDatabase : ScriptableObject,ISerializationCallbackReceiver
{
    public ItemObject[] items;
    public Dictionary<int, ItemObject> GetItemObj = new Dictionary<int, ItemObject>();
    public void OnAfterDeserialize()
    {
          for(int i = 0;i< items.Length; i++)
          {
             items[i].ID = i;
             GetItemObj.Add(i, items[i]);
          }
    }
    public void OnBeforeSerialize()
    {
        GetItemObj = new Dictionary<int, ItemObject>();
    }
}
