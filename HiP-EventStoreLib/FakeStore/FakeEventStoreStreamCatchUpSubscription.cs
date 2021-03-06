﻿using PaderbornUniversity.SILab.Hip.EventSourcing.Migrations;
using System;
using System.Collections.Generic;

namespace PaderbornUniversity.SILab.Hip.EventSourcing.FakeStore
{
    public class FakeEventStoreStreamCatchUpSubscription : IEventStreamSubscription
    {
        private readonly IDisposable _futureEventSubscription;

        public event EventHandler<EventParsingFailedArgs> EventParsingFailed;

        /// <summary>
        /// Creates a new catch-up subscription.
        /// </summary>
        /// <param name="existingEvents">All events currently stored in the fake event store</param>
        /// <param name="futureEvents">An observable of events that are appended in the future</param>
        /// <param name="handler">The event handler</param>
        public FakeEventStoreStreamCatchUpSubscription(IEnumerable<IEvent> existingEvents, IObservable<IEvent> futureEvents, Action<IEvent> handler)
        {
            foreach (var ev in existingEvents)
                handler?.Invoke(ev.MigrateToLatestVersion());

            _futureEventSubscription = futureEvents.Subscribe(ev => handler?.Invoke(ev.MigrateToLatestVersion()));
        }
        
        public void Dispose()
        {
            _futureEventSubscription.Dispose();
        }
    }
}
