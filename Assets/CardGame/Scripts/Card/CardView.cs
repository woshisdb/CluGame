using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour,IView
{
    public TextMeshPro title;
    public TextMeshPro description;
    public CardModel cardModel;

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
        this.title.text = cardModel.cardData.title;
        this.description.text = cardModel.cardData.description;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
