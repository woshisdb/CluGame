using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandUI : MonoBehaviour
{
    public Transform root;
    public Transform topRoot;
    public List<CardViewUI> cardViews;
    public List<GameHandBar> PlayerHandBars;
    public List<PlayerDicHand> playerHands;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 获取一系列要选择的卡片
    /// </summary>
    /// <returns></returns>
    public void SetCards(List<CardModel> cards)
    {
        gameObject.SetActive(true);
        ClearCards();
        foreach (CardModel cardModel in cards)
        {
            var obj = cardModel.CreateViewUI() as CardViewUI;
            obj.BindModel(cardModel);
            cardViews.Add(obj);
            obj.transform.SetParent(root);
            obj.transform.localScale = new Vector3(1.7f,1.7f,1.7f);
        }
    }

    public void CreateBars(List<PlayerDicHand> dics)
    {
        foreach (var bar in PlayerHandBars)
        {
            GameObject.Destroy(bar);
        }
        PlayerHandBars.Clear();
        ClearCards();
        foreach (var dic in dics)
        {
            CreateBar(dic);
        }
    }
    public void CreateBar(PlayerDicHand dic)
    {
        var bar = GameFrameWork.Instance.gameConfig.GameHandBar;
        var barInst = GameObject.Instantiate(bar);
        barInst.transform.SetParent(topRoot);
        var comp = barInst.GetComponent<GameHandBar>();
        PlayerHandBars.Add(comp);
        comp.text.text = dic.id;
        comp.dicCards = dic.dics;
    }
    public void SetGamePlayerHand(List<PlayerDicHand> hands)
    {
        this.playerHands = hands;
    }
    public void CancealCards()
    {
        gameObject.SetActive(false);
    }
    public void ClearCards()
    {
        foreach (var cv in cardViews)
        {
            GameObject.DestroyImmediate(cv.gameObject);
        }
        cardViews.Clear();
    }
}
