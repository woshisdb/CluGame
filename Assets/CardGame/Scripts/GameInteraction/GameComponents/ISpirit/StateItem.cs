public class StateItem<T>
{
    public T state;
    public int value;
    public int maxValue;

    public StateItem(T state, int value, int maxValue)
    {
        this.state = state;
        this.maxValue = maxValue;
        this.value = value;
    }
}