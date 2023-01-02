using System;

namespace Core.PublishSubscribe
{
    public interface IPublisher<T>
    {
        public event EventHandler<Message<T>> MessagePublisher;
        public void OnEventHandler(Message<T> args);
        public void Publish(T data);
    }
}
