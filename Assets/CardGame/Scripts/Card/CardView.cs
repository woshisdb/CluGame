using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Studio.OverOne.DragMe.Components;
using Studio.OverOne.DragMe.Data.Abstractions;
using Studio.OverOne.DragMe.Events;
using TMPro;
using UnityEngine;

public class CardView : SerializedMonoBehaviour,IView,IUISelector, ISendEvent
{
    public TextMeshPro title;
    public TextMeshPro description;
    [SerializeField]
    public CardModel cardModel;
    public TextMeshPro countDown;
    public Slot slot;
    
    public void BindModel(IModel model)
    {
        this.cardModel = (CardModel)model;
        Refresh();
        this.onBindView();
    }

    public IModel GetModel()
    {
        return cardModel;
    }
    public void onGrab()
    {
        if(slot != null)
        {
            slot.OnCardReleased(this);
            this.slot = null;
        }
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
    public void Placed(CardDragEventArgs placedEvent)
    {
        if(placedEvent.TargetSlot!=null)
        {
            placedEvent.TargetSlot.TryPlaceCard(placedEvent.Card);
        }
    }
    /// <summary>
    /// 卡片放置在Table上
    /// </summary>
    /// <returns></returns>
    public void PlaceToTable()
    {
    }
    public void OnDestroy()
    {
        this.OnDestroyView();
    }
    /// <summary>
    /// 点击卡片
    /// </summary>
    public virtual void OnCardClick()
    {
        this.SendEvent<RefreshViewEvent>(new RefreshViewEvent(this));
    }

    public virtual List<UIItemBinder> GetUI()
    {
        return new List<UIItemBinder>()
        {
            new KVItemBinder(()=>{return "ee1"; },()=>{return "ee2"; }),
            new KVItemBinder(()=>{return "ee1"; },()=>{return "ee2"; }),
            new ButtonBinder(()=>{return "t1"; },()=>{})
        };
    }

    public void Release()
    {

    }
}
