using CommonProcess.Error;

namespace DisConf.Web.Service.Core.Model
{
    internal static class ErrorConverter
    {
        public static DisConfProcessError ToDisConfProcessError(this ProcessError error)
        {
            if (error == null) return null;
            return new DisConfProcessError(error.Code, error.Message);
        }
    }
}
