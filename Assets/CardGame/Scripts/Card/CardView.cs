using System.Collections;
using System.Collections.Generic;
using Studio.OverOne.DragMe.Data.Abstractions;
using Studio.OverOne.DragMe.Events;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour,IView
{
    public TextMeshPro title;
    public TextMeshPro description;
    public CardModel cardModel;
    public TextMeshPro countDown;
    
    public void BindModel(IModel model)
    {
        this.cardModel = (CardModel)model;
        Refresh();
    }

    public IModel GetModel()
    {
        return cardModel;
    }

    public void Refresh()
    {
        if (cardModel!=null)
        {
            this.title.text = cardModel.cardData.title;
            this.description.text = cardModel.cardData.description;
            this.countDown.text = "倒计时";
        }
    }

    public void Release()
    {
        Debug.Log("shuissssss");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cardModel!=null && cardModel.cardData.needRefresh)
        {
            Refresh();
        }
    }
    public void Placed(IPlacedEventData placedEvent)
    {
        var placement = placedEvent.PlacementComponent;
        placement.GetComponent<Slot>().OnCardTryPlaced(this);
    }
}
