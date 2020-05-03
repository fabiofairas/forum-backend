using MediatR;

namespace Project.Model.Notifications
{
    public class DomainNotification : INotification
    {
        public string Value { get; private set; }

        public DomainNotification(string value)
        {
            Value = value;
        }
    }
}