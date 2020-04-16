using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Options
{
    public class ResillienceOption
    {
		public string DistributedName
		{
			get;
			set;
		}

		public string DefaultApiVersion
		{
			get;
			set;
		}

		public string LocalizationResourcesPath
		{
			get;
			set;
		}

		public string Resillience
		{
			get;
			set;
		}

		public bool ShowUnhandleException
		{
			get;
			set;
		}

		public bool RequireAuthenticatedUser
		{
			get;
			set;
		}
	}
}
