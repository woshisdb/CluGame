using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardViewUI : MonoBehaviour,IView
{
    public CardModel cardModel;
    public TextMeshProUGUI name;

    public DraggableCard card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BindModel(IModel model)
    {
        cardModel = model as CardModel;
        Refresh();
    }

    public IModel GetModel()
    {
        return cardModel;
    }

    public void Refresh()
    {
        this.name.text = cardModel.GetTitle();
    }

    public void Release()
    {
        
    }
    
    
    
    public void TestClick()
    {
        cardModel.TryOnClick();
    }
}
