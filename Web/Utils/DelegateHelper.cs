namespace Web.Utils
{
    public class DelegateHelper
    {
        public static Func<TRet> WrapT1<T, TRet>(Func<T, TRet> fn, T v)
        {
            
            return () => fn(v);
        }
        public static Action Wrap(Delegate fn,  params object?[]? values) 
        {
            return () => fn.DynamicInvoke(values);
        }
        public static Func<Task> WrapAsync<T>(T fn, params object?[]? values) where T : Delegate
        {
            return () => (Task)fn.DynamicInvoke(values);
        }
        public static Action WrapT1<T>(Action<T> fn, T v)
        {
            return () => fn(v);
        }
    }
}
