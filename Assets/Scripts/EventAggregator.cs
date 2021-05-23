using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventAggregator
{
    public static void Subscribe<T>(Action<object,T> subscribeEvent){

        EventHolder<T>.Event += subscribeEvent;
    }
    public static void UnSubscribe<T>(Action<object, T> subscribeEvent)
    {
        EventHolder<T>.Event -= subscribeEvent;
    }

    public static void Post<T>(object sender, T eventData )
    {
        EventHolder<T>.Post(sender, eventData);
    }

    public static class EventHolder<T>
    {
        public static event Action<object, T> Event;
        public static void Post(object sender, T eventData)
        {
            Event?.Invoke(sender, eventData);
        }
    }

}
