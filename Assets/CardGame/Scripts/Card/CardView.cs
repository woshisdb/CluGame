using System.Collections;
using System.Collections.Generic;
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
}
