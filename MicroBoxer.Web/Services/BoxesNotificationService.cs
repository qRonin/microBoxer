namespace MicroBoxer.Web.Services
{
    public class BoxesNotificationService
    {
        private readonly object _subscriptionsLock = new();
        private readonly Dictionary<string, HashSet<Subscription>> _subscriptionsByUserId = new();
        public IDisposable SubscribeToBoxesChangesNotifications(string userId, Func<Task> callback)
        {
            var subscription = new Subscription(this, userId, callback);

            lock (_subscriptionsLock)
            {
                if (!_subscriptionsByUserId.TryGetValue(userId, out var subscriptions))
                {
                    subscriptions = [];
                    _subscriptionsByUserId.Add(userId, subscriptions);
                }

                subscriptions.Add(subscription);
            }

            return subscription;
        }

        public Task NotifyBoxesChangedAsync(string userId)
        {
            lock (_subscriptionsLock)
            {
                return _subscriptionsByUserId.TryGetValue(userId, out var subscriptions)
                    ? Task.WhenAll(subscriptions.Select(s => s.NotifyAsync()))
                    : Task.CompletedTask;
            }
        }


        private void Unsubscribe(string userId, Subscription subscription)
        {
            lock (_subscriptionsLock)
            {
                if (_subscriptionsByUserId.TryGetValue(userId, out var subscriptions))
                {
                    subscriptions.Remove(subscription);
                    if (subscriptions.Count == 0)
                    {
                        _subscriptionsByUserId.Remove(userId);
                    }
                }
            }
        }

        private class Subscription(BoxesNotificationService owner, string userId, Func<Task> callback) : IDisposable
        {
            public Task NotifyAsync()
            {
                return callback();
            }

            public void Dispose()
                => owner.Unsubscribe(userId, this);
        }

    }
}
