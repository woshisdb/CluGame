using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public TextMeshPro name;
    public CardView cardView;
    public TaskPanelView taskPanelView;
    public bool isInit;
    public bool HasCard
    {
        get { return cardView != null; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Pos(int x, int y)
    {
        var spaceX = 1.5f;
        var spaceY = 3.3f;
        transform.localPosition = new Vector3(x * spaceX, 0.5f, -y * spaceY);
    }
    /// <summary>
    /// 卡片是否可以放置
    /// </summary>
    /// <returns></returns>
    public bool IsCardCanPlaced(CardView cardView)
    {
        if (cardView == null ||cardView.cardModel==null)
        {
            return false;
        }
        return taskPanelView.CanAddCard(this, cardView.cardModel);
    }
    public void CardPlaceCheck(CardPlaceCheckArgs arg)
    {
        var c = arg.Card.GetComponent<CardView>();
        if (c!=null)
        {
            var res = IsCardCanPlaced(c);
            arg.IsAllowPlace = res;
        }
    }
    /// <summary>
    /// 卡片尝试放置
    /// </summary>
    /// <returns></returns>
    public void OnCardTryPlaced(OnCardPlaceArgs cardViewArgs)
    {
        var cardView = cardViewArgs.Card;
        var cardModel = cardView.GetModel() as CardModel;
        this.cardView = cardView;
        cardView.slot = this;
        cardView.transform.parent = this.transform;
        if (!isInit)
        {
            taskPanelView.OnAddCard(this, cardModel);
            if (taskPanelView.taskPanelModel.CanChangeCardSwitch())
            {
                GameFrameWork.Instance.viewModelManager.RefreshView(taskPanelView.taskPanelModel);
            }
        }
    }
    public void OnCardReleased(CardView cardView)
    {
        this.cardView = null;
        taskPanelView.OnRemoveCard(this, cardView.cardModel);
        if (taskPanelView.taskPanelModel.CanChangeCardSwitch())
        {
            //taskPanelView.StateTransition();
            GameFrameWork.Instance.viewModelManager.RefreshView(taskPanelView.taskPanelModel);
        }
    }
    /// <summary>
    /// 尝试添加卡片通过CardModel
    /// </summary>
    /// <param name="cardModel"></param>
    public void TryAddCardByCardModel(CardModel cardModel)
    {
        if (taskPanelView.CanAddCard(this, cardModel))
        {
            var card = GameFrameWork.Instance.AddCardByCardModel(cardModel,new Vector3(0,0,0), true);
            GetComponent<CardSlot>().TryPlaceCard(card.GetComponent<DraggableCard>());
            var evt = new OnCardPlaceArgs();
            evt.Card = card.GetComponent<CardView>();
            evt.Card.SetCustomGrabAction(() =>
            {
                GameObject.Destroy(card);
            });
            OnCardTryPlaced(evt);
        }
    }
    
    public void OnClickSlot()
    {
        taskPanelView.OnSlotTouch(this);
        Debug.Log("OnClickSlot");
    }
}
