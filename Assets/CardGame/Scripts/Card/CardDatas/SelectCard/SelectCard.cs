public class SelectCard<T>:CardModel
{
    public T select;
    public SelectCard(T select) : base(null,null,false)
    {
        this.select = select;
    }

    public T GetSelect()
    {
        return select;
    }
}