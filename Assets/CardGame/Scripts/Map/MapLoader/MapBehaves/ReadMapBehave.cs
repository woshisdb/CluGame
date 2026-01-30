public class ReadMapBehave:MapBehave
{
    private SeeComponent seer;
    public BookComponent BookComponent;
    public ReadMapBehave(MapLoader map,SeeComponent seer,BookComponent bookComponent) : base(map)
    {
        this.BookComponent = bookComponent;
        this.seer = seer;
    }

    public override void Run()
    {
        
    }
}