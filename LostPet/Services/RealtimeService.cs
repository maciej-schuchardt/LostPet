using System.Diagnostics.CodeAnalysis;

namespace LostPet.Services
{
    [ExcludeFromCodeCoverage]
    public class RealtimeService
    {
        private int _a;
        public int a
        {
            get { return _a; }
            set
            {
                _a = value;
                UpdateEvent?.Invoke();
            }
        }
        private int _b;

        public int b
        {
            get { return _b; }
            set
            {
                _b = value;
                UpdateEvent?.Invoke();
            }
        }

        private string toastClasses = "toast";
        public string ToastClasses
        {
            get { return toastClasses; }
            set
            {
                toastClasses = value;
                UpdateEvent?.Invoke();
            }
        }
        public event Func<Task> UpdateEvent;
    }
}
