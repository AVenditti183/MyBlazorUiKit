namespace MyBlazorUiKit.Services
{
    public interface IServiceCounter
    {
        int Increment(int currentValue);
        int Decrement(int currentValue);
    }
}