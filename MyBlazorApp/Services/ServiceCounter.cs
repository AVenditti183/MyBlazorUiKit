using MyBlazorUiKit.Services;

namespace MyBlazorApp.Services
{
    public class ServiceCounter : IServiceCounter
    {
        public int Increment(int currentValue)
            => currentValue + 1;


        public int Decrement(int currentValue)
            => currentValue - 1;
    }
}