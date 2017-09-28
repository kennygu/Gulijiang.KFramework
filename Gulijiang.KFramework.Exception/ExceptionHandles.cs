using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp;
using PostSharp.Aspects;
using Gulijiang.KFramework.WebExtensions;
using Gulijiang.KFramework.Log;

namespace Gulijiang.KFramework.Exception
{
    [Serializable]
    public class ExceptionHandlesAttribute : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            SysLog.WriteErrorLog(args.Exception.Message);
            args.FlowBehavior = FlowBehavior.Return;
            DataResult res = new DataResult();
            res.Message = args.Exception.Message;
            res.Data = "";
            res.Code = -1;
            args.ReturnValue = res;
        }
    }
}
