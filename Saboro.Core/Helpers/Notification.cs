using System.Collections.ObjectModel;
using Saboro.Core.Interfaces.Helpers;

namespace Saboro.Core.Helpers;

public class Notification : INotification
{
    private readonly Collection<NotificationItem> _notifications;

    public Notification()
    {
        _notifications = new Collection<NotificationItem>();
    }

    public void Add(string message) => _notifications.Add(new NotificationItem(message, NotificationType.None));
    public void Add(string message, NotificationType type) => _notifications.Add(new NotificationItem(message, type));

    public bool Any() => _notifications.Any();
    public bool Any(NotificationType type) => _notifications.Any(x => x.Type == type);

    public IEnumerable<string> Get() => _notifications.Select(x => x.Message);
    public string GetAsString() => string.Join(", ", _notifications.Select(x => x.Message));
    public IEnumerable<string> Get(NotificationType type) => _notifications.Where(x => x.Type == type).Select(x => x.Message);

    public string First() => _notifications.First().Message;
    public string First(NotificationType type) => _notifications.First(x => x.Type == type).Message;
}

internal class NotificationItem
{
    internal NotificationItem(string message, NotificationType type)
    {
        Message = message;
        Type = type;
    }

    public string Message { get; }
    public NotificationType Type { get; }
}

public enum NotificationType
{
    None,
    Error,
    Warning,
    Info,
    Success
}