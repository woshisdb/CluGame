using Sirenix.OdinInspector;

public class ChatComponent : BaseComponent, ISupplyTask
{
    public ChatComponent other;
    public ChatComponent(CardModel cardModel, IComponentCreator creator) 
        : base(cardModel, creator)
    {
        var c = creator as ChatComponentCreator;
    }

    public bool CanChat()
    {
        return other == null;
    }

    public void StartChat(ChatComponent chat)
    {
        this.other = chat;
    }
    public SupplyTaskType TaskType => SupplyTaskType.Chat;
}

public class ChatComponentCreator : BaseComponentCreator<ChatComponent>
{

    public override ComponentType ComponentName()
    {
        return ComponentType.ChatComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new ChatComponent(cardModel, this);
    }
}
