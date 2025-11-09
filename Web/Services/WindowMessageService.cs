using Microsoft.JSInterop;

namespace Web.Services
{
    public class WindowMessageService
    {
        public Action<string,object?>? OnPlay { get; set; }
        
        public void Play(string mode, object? message)
        {

            OnPlay?.Invoke(mode, message);
        }
        public void AddListener(Action<string, object?> listener)
        {
            OnPlay = (Action<string, object?>?)Delegate.Combine(OnPlay, listener);
        }
        public void RemoveListener(Action<string,object?> listener)
        {
            OnPlay = (Action<string, object?>?)Delegate.Remove( OnPlay, listener);
        }
    }
}
