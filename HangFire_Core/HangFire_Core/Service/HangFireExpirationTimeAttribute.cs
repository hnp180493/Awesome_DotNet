using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire_Core.Service
{
    //Remove log data at specific time
    public class HangFireExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext filterContext, IWriteOnlyTransaction transaction)
        {
            filterContext.JobExpirationTimeout = TimeSpan.FromMinutes(1);//make 7 to number of days you want
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromMinutes(1);//make 7 to number of days you want
        }
    }
}
