using Edwards.CodeChallenge.Domain.Notifications;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Edwards.CodeChallenge.Domain.Interfaces.Notifications;

public interface IDomainNotification
{
    IReadOnlyCollection<NotificationMessage> Notifications { get; }
    bool HasNotifications { get; }
    void AddNotification(string key, string message);
    void AddNotifications(IEnumerable<NotificationMessage> notifications);
    void AddNotifications(ValidationResult validationResult);
}
