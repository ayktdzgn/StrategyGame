using System;

public class Message<T> : EventArgs
{
    public T GenericMessage { get; set; }
    public Message(T message)
    {
        GenericMessage = message;
    }
}
