using System.Reflection;
using log4net;

namespace DisConf.Client.Manager
{
    public abstract class BaseManager
    {
        protected ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);//MethodBase.GetCurrentMethod().DeclaringType
    }
}
