/**
 * 2015-02-24 M.Horigome
 * Service sample
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

// add
using System.ServiceModel.Web;

namespace ShiolWinSvc
{
    
    /// <summary>
    /// Service Class
    /// </summary>
    public partial class MainService : ServiceBase
    {
        private WebServerServiceProvider  serviceHost;
        

        // initialize
        public MainService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start Service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            
            try {
                serviceHost = new WebServerServiceProvider();
                serviceHost.StartService();
                EventLog.WriteEntry("Web Service Started", EventLogEntryType.Information);
            } 
            catch(Exception e) 
            {
                EventLog.WriteEntry("Web Service Start ERR. " + e.Message, EventLogEntryType.Error);
            }
            
        }

        /// <summary>
        /// Stop Service
        /// </summary>
        protected override void OnStop()
        {
            
            try
            {
                if (serviceHost != null)
                {
                    serviceHost.StopService();
                }
                EventLog.WriteEntry("Web Service Stopped", EventLogEntryType.Information);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Web Service Stopped ERR. " + e.Message, EventLogEntryType.Error);
            }

        }
    }
}
