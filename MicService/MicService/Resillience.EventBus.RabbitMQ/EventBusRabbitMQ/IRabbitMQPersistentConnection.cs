using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.EventBus.RabbitMQ.EventBusRabbitMQ
{
	public interface IRabbitMQPersistentConnection : IDisposable
	{
		bool IsConnected
		{
			get;
		}

		bool TryConnect();

		IModel CreateModel();
	}
}
