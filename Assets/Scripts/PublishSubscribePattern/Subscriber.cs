using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subscriber<T>
{
    public IPublisher<T> Publisher { get; private set; }
    public Subscriber(IPublisher<T> publisher)
    {
        Publisher = publisher;
    }
}
