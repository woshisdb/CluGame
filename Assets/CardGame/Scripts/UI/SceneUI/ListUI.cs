using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ListUI
{
    public Transform content;
    public List<GameObject> uiItems = new List<GameObject>();
    public void ClearUis()
    {
        foreach (GameObject item in uiItems)
        {
            GameObject.Destroy(item);
        }
        uiItems.Clear();
    }
    public void Add(GameObject obj)
    {
        obj.transform.SetParent(content);
        uiItems.Add(obj);
    }
}