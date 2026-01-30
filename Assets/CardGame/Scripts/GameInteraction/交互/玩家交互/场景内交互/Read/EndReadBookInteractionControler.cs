using System.Collections.Generic;

public class EndReadBookInteractionControler : GetInteractionControler, GetWorldMapInteraction
{
    string GetInteractionControler.GetKey()
    {
        throw new System.NotImplementedException();
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    bool GetWorldMapInteraction.IsSat(CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    InteractionType GetWorldMapInteraction.GetInteractionType()
    {
        throw new System.NotImplementedException();
    }

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    string GetWorldMapInteraction.GetKey()
    {
        throw new System.NotImplementedException();
    }

    bool GetInteractionControler.IsSat(CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    InteractionType GetInteractionControler.GetInteractionType()
    {
        throw new System.NotImplementedException();
    }
}