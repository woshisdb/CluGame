using System.Collections;
using System.Collections.Generic;
using Studio.OverOne.DragMe.Components;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public DragMe dragMe;
    public TextMeshPro name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pos(int x,int y)
    {
        var spaceX =1.5f;
        var spaceY =10;
        dragMe.SetPos((new Vector3(x * spaceX, 0.5f, -y * spaceY)));
        Debug.Log(dragMe.DesiredPosition);
    }
    public void Set()
    {
        Debug.Log("set");
    }
}
