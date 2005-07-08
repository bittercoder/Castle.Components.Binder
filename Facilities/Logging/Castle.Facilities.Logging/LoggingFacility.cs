using Castle.Model.Configuration;
using Castle.Services.Logging;
// Copyright 2004-2005 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Facilities.Logging
{
	using System;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Facilities;
    using Castle.Model;

    public enum LoggingFramework{None,log4net,NLog}
	/// <summary>
	/// 
	/// </summary>
	public class LoggingFacility : AbstractFacility
	{
        private LoggingFramework framework;
        private ILoggerFactory factory;
        private bool intercept = true;

		public LoggingFacility()
		{
            this.framework = LoggingFramework.None;
		}

		protected override void Init()
		{
            /////////////////////////////
            //Get some config information
		    GetConfigurationInformation();

		    ////////////////////////////////////
            //setup log4net/NLog and get rocking
            SetupLogManager();

            ////////////////////////
            //For base level logging
            this.Kernel.AddComponent("logging.intercepter", typeof(LoggingInterceptor));

            ////////////////////////
            //For Attributal Logging
            // Kernel.ComponentModelCreated += new ComponentModelDelegate(OnComponentModelCreated);
            // Kernel.ComponentRegistered += new ComponentDataDelegate(OnComponentRegistered);

            ///////////////////////////
            //For Constructor Injection
            // ???????
            // this.Kernel.AddComponentInstance("logging.factory", typeof(ILoggerFactory), this.factory);
            // How can I detect requests for ILogger and then supply them with a logger with the class full name?
            // can they request a ILoggerFactory?
		}

	    private void GetConfigurationInformation()
	    {
	        if(this.FacilityConfig != null)
	        {
	            IConfiguration frameworkConfig = FacilityConfig.Children["framework"];
                String fw = frameworkConfig.Value;
                this.framework = (LoggingFramework) Enum.Parse(typeof(LoggingFramework), fw, true);
                this.intercept = bool.Parse(FacilityConfig.Children["interception"].Value);
	        }
            else
	        {
	            this.framework = LoggingFramework.None;
                this.intercept = true;
	        }
	    }

        private void SetupLogManager()
        {
            if(this.FacilityConfig == null)
            {
                //setup NullLogger
            }
            else
            {
                if(this.framework == LoggingFramework.log4net)
                {
                    this.factory = null; /*log4netFactory*/
                }
                else if(this.framework == LoggingFramework.NLog)
                {
                    this.factory = null; /*NLogFactory*/
                }
                else
                {
                    this.factory = null; /*NullFactory*/
                }
            }
        }

	}
}
