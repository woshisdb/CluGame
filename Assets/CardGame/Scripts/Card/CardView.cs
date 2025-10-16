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
    public CardLineMgr lineMgr;
    public bool justCkeck { get { return cardModel.atLeastOne; } }
    public Vector3 startPos;
    /// <summary>
    /// 纯SlotCard
    /// </summary>
    public bool pureSlotCard;
    public void BindModel(IModel model)
    {
        this.cardModel = (CardModel)model;
        Refresh();
        this.onBindView();
        if(!pureSlotCard)
        lineMgr = new CardLineMgr(cardModel);
    }

    public IModel GetModel()
    {
        return cardModel;
    }
    public void onGrab()
    {
        startPos = this.transform.localPosition;
        if(pureSlotCard)
        {
            if (slot != null)
            {
                slot.OnCardReleased(this);
                this.slot = null;
            }
        }
        else
        {
            if (slot != null)
            {
                slot.OnCardReleased(this);
                this.slot = null;
            }
        }
    }
    public virtual void Refresh()
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
        if(!pureSlotCard)
        UpdateLine();
        if (cardModel!=null && cardModel.cardData.needRefresh)
        {
            Refresh();
        }
    }
    public virtual void WhenCanSuccPlace(CardDragEventArgs placedEvent)
    {
        var obj = GameFrameWork.instance.AddCardByCardModel(cardModel, Vector3.zero, true);
        placedEvent.TargetSlot.TryPlaceCard(obj.GetComponent<DraggableCard>());
    }
    public void Placed(CardDragEventArgs placedEvent)
    {
        if (pureSlotCard&&justCkeck)
        {
            if (placedEvent.TargetSlot != null)
            {
                bool succPut = placedEvent.TargetSlot.TryPlaceCard(placedEvent.Card, justCkeck);
                transform.localPosition = startPos;
                if (!succPut)
                {
                    transform.localPosition = startPos;
                    GameObject.Destroy(gameObject);
                }
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            if (placedEvent.TargetSlot != null)
            {
                if (justCkeck)//不会放到插槽上,而是进行Check判断
                {
                    bool succPut = placedEvent.TargetSlot.TryPlaceCard(placedEvent.Card, justCkeck);
                    if(succPut)
                    {
                        transform.localPosition = startPos;
                        WhenCanSuccPlace(placedEvent);
                    }
                }
                else
                {
                    bool succPut = placedEvent.TargetSlot.TryPlaceCard(placedEvent.Card, justCkeck);
                    if (!succPut)
                    {
                        transform.localPosition = startPos;
                    }
                }
            }
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
        return cardModel.GetUI();
    }

    public void Release()
    {

    }
    public void UpdateLine()
    {
        foreach(var item in lineMgr.cardLines)
        {
            if (lineMgr.connectors.ContainsKey(item)&& lineMgr.connectors[item]==null)
            {
                lineMgr.connectors.Remove(item);
            }
            if(!lineMgr.connectors.ContainsKey(item))
            {
                CreateLine(item);
            }
        }
    }
    public void CreateLine(CardLineData data)
    {
        var fromObj = GameFrameWork.instance.cardsManager.FindCardByEnum(data.from);
        var toObj = GameFrameWork.instance.cardsManager.FindCardByEnum(data.to);
        if(fromObj && toObj)
        {
            var lineTempate = GameFrameWork.Instance.gameConfig.lineTemplate;
            var obj = GameObject.Instantiate(lineTempate);
            var cmp = obj.GetComponent<ObjectConnector>();
            cmp.Bind(fromObj.transform, toObj.transform, data.action);
            cmp.transform.SetParent(transform);
            lineMgr.connectors[data] = cmp;
        }
    }
}
