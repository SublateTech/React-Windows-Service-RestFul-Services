using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiolWinSvc
{
    public abstract class AbstractServiceProvider : ICloneable
    {
        /// <SUMMARY>
        /// Provides a new instance of the object.
        /// </SUMMARY>
        public virtual object Clone()
        {
            throw new Exception("Derived clases must override Clone method.");
        }

        /// <SUMMARY>
        /// Start Service
        /// </SUMMARY>
        public abstract void StartService();

        /// <SUMMARY>
        /// Stop Service
        /// </SUMMARY>
        public abstract void StopService();


    }
}
