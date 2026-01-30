using System.Collections.Generic;

public class ChatInteractionControler:GetWorldMapInteraction
{
    public string GetKey()
    {
        return "交流";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var mec = me.GetComponent<ChatComponent>();
            var otherc =beObj.GetComponent<ChatComponent>();
            mec.StartChat(otherc);
            otherc.StartChat(mec);
            
        }));
        return ret;
    }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if (me.IsSatComponent<ChatComponent>()&&beObj.IsSatComponent<ChatComponent>())
        {
            if (me.GetComponent<ChatComponent>().CanChat()&&beObj.GetComponent<ChatComponent>().CanChat())
            {
                return true;
            }
        }

        return false;
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    public InteractionType GetInteractionType()
    {
        throw new System.NotImplementedException();
    }
}