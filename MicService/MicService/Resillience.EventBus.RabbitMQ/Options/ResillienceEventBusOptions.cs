using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.EventBus.RabbitMQ.Options
{
	public class ResillienceEventBusOptions
	{
		public string EventBusConnection
		{
			get;
			set;
		}

		public string EventBusUserName
		{
			get;
			set;
		}

		public string EventBusPassword
		{
			get;
			set;
		}

		public int EventBusRetryCount
		{
			get;
			set;
		}

		public string SubscriptionClientName
		{
			get;
			set;
		}

		public string ExchangeName
		{
			get;
			set;
		}
	}
}
