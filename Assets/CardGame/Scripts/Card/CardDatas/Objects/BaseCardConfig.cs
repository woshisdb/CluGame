using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



[CreateAssetMenu(fileName = "配置", menuName = "怪物/怪物配置")]
public abstract class BaseCardConfig:IRegisterID
{
    public IDModel UID;
    [LabelText("组件库")]
    public List<IComponentCreator> ComponentCreators=new List<IComponentCreator>();
    [Button]
    public void IsSat()
    {
        foreach (var x in ComponentCreators)
        {
            if (!x.NeedComponent(ComponentCreators))
            {
                Debug.Log(x.ToString());
            }
        }
    }

    public void AddComponentCreator(IComponentCreator componentCreator)
    {
        ComponentCreators.Add(componentCreator);
    }
    public virtual void ComponentNeed()
    {
        
    }

    public BaseCardConfig()
    {
        this.GetID();
        ComponentNeed();
    }

    public string SetID(Func<string> id)
    {
        if (UID == null)
        {
            UID = new IDModel();
            UID.id = id();
            GameFrameWork.Instance.data.saveFile.idMap[UID.id] = this;
        }
        return UID.id;
    }
    [Button]
    public void InitID()
    {
        this.GetID();
    }
    [Button]
    public void InitObjectComponent()
    {
        var model = FindCardModel();
        model.InitComponent();
    }
    public virtual CardModel FindCardModel()
    {
        foreach (var card in GameFrameWork.Instance.cardsManager.cardmodels)
        {
            if (card.cfg.Value == this)
            {
                return card;
            }
        }

        return null;
    }
}