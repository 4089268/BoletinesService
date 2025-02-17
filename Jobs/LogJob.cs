using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Quartz;
using log4net;

namespace BoletinesService.Jobs;

internal class LogJob : IJob
{

    private readonly ILog log;
    public LogJob()
    {
        log = LogManager.GetLogger(typeof(LogJob));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        log.Info("Hola mundo");
        await Task.CompletedTask;
    }
}