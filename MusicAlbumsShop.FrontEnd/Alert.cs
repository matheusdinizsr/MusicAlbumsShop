using static System.Net.Mime.MediaTypeNames;

namespace MusicAlbumsShop.FrontEnd
{
    public interface IAlertService
    {
        public event Action? OnAlertsChanged;
        public void NewAlert(string text, bool isDismissible, int dismissingTime, EMessageType type);
        public void NewAlert(string text, bool isDismissible, EMessageType type);
        public Alert[] GetAlerts();
        public void DismissByButton(Guid id);
    }
    public class Alert
    {
        public Guid Id { get; private set; }
        public string Text { get; private set; }
        public EMessageType Type { get; private set; }
        public bool IsDismissible { get; private set; }
        public int DismissingTime { get; private set; }

        public Alert(string text, bool isDismissible, int dismissingTime, EMessageType type)
        {
            Id = Guid.NewGuid();
            Text = text;
            IsDismissible = isDismissible;
            DismissingTime = dismissingTime;
            Type = type;
        }

        public Alert(string text, bool isDismissible, EMessageType messageType) : this(text, isDismissible, 0, messageType) { }
    }

    public class AlertService : IAlertService
    {
        private List<Alert> _alerts = new();
        public event Action? OnAlertsChanged;

        public void NewAlert(string text, bool isDismissible, int dismissingTime, EMessageType type)
        {
            var alert = new Alert(text, isDismissible, dismissingTime, type);

            _alerts.Add(alert);
            OnAlertsChanged?.Invoke();

            _ = Task.Delay(TimeSpan.FromSeconds(alert.DismissingTime)).ContinueWith(a =>
            {
                _alerts.Remove(alert);
                OnAlertsChanged?.Invoke();
            });


        }

        public void NewAlert(string text, bool isDismissible, EMessageType type)
        {
            var alert = new Alert(text, isDismissible, type);

            _alerts.Add(alert);
            OnAlertsChanged?.Invoke();
        }

        public void DismissByButton(Guid id)
        {
            var alert = _alerts.Where(a => a.Id == id).FirstOrDefault();

            if (alert != null)
            {
                _alerts.Remove(alert);
                OnAlertsChanged?.Invoke();
            }
        }

        public Alert[] GetAlerts()
        {
            return _alerts.ToArray();
        }
    }

    public enum EMessageType
    {
        Success,
        Warning,
        Error,
    }

}
