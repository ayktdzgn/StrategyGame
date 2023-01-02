using System;

namespace Core.PublishSubscribe
{
    public class Publisher<T> : IPublisher<T>
    {
        public event EventHandler<Message<T>> MessagePublisher;

        public void OnEventHandler(Message<T> args)
        {
            var handler = MessagePublisher;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Publish event
        /// </summary>
        /// <param name="Set type of data"></param>
        public void Publish(T data)
        {
            Message<T> message = (Message<T>)System.Activator.CreateInstance(typeof(Message<T>), new object[] { data });
            OnEventHandler(message);
        }
    }
}
