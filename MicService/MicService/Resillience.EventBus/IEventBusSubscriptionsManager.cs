﻿using Resillience.EventBus.Abstractions;
using Resillience.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.EventBus
{
	public interface IEventBusSubscriptionsManager
	{
		bool IsEmpty
		{
			get;
		}

		event EventHandler<string> OnEventRemoved;

		void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

		void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

		void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

		void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

		bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

		bool HasSubscriptionsForEvent(string eventName);

		Type GetEventTypeByName(string eventName);

		void Clear();

		IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

		IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent(string eventName);

		string GetEventKey<T>();
	}
}
