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
using Microsoft.Extensions.Configuration;
using BoletinesService.Data;

namespace BoletinesService.Jobs;

internal class LogJob : IJob
{
    private readonly ILog log;
    private readonly SicemContext sicemContext;
    public LogJob(SicemContext context)
    {
        log = LogManager.GetLogger(typeof(LogJob));
        this.sicemContext = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var boletin = sicemContext.OprBoletins.ToList();
        foreach(var b in boletin)
        {
            log.Info($"Boletin: {b.Titulo}  {b.CreatedAt}");
        }
        await Task.CompletedTask;
    }
}