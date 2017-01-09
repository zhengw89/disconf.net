using System;
using System.Reflection;
using log4net;

namespace DisConf.Client.Manager
{
    public abstract class BaseManager
    {
        protected readonly string ManagerId;
        protected ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);//MethodBase.GetCurrentMethod().DeclaringType

        protected BaseManager()
        {
            this.ManagerId = Guid.NewGuid().ToString();
        }
    }
}
