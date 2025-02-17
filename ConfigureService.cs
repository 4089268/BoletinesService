using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Configuration;
using Autofac.Extras.Quartz;
using BoletinesService.Data;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Topshelf;
using Topshelf.Autofac;

namespace BoletinesService;

public class ConfigureService
{
    internal static void Configure()
    {
        // Build configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set base path
            .AddJsonFile("appsettings.json")
            .Build(); // Load JSON file

        Console.WriteLine(">> " + configuration.GetConnectionString("SICEM"));
            
        // Get log settings
        string logFilePath = "logs/app.log";
        string maxFileSize = "10MB";
        int maxRollBackups = 5;
        string logLevel = "ALL";
        ConfigureLog4Net(logFilePath, maxFileSize, maxRollBackups, logLevel);

        // * configure services for dependancy injection
        ContainerBuilder cb = new ContainerBuilder();
        cb.RegisterModule(new ConfigurationModule(configuration));
        cb.RegisterModule(new QuartzAutofacFactoryModule());
        cb.RegisterModule(new QuartzAutofacJobsModule( typeof (BoletinService).Assembly));
        cb.RegisterType<BoletinService>().AsSelf().InstancePerLifetimeScope();
        cb.RegisterType<SicemContext>().AsSelf().InstancePerLifetimeScope();
        
        // * configure the windows service
        HostFactory.Run(conf => {
            conf.RunAsLocalService();
            conf.SetServiceName("Boletin Service");
            conf.SetDisplayName("BoletinService");
            conf.SetDescription("Servicio para enviar boletines generados desde SICEM");
            conf.UseLog4Net();
            conf.UseAutofacContainer(cb.Build());
            conf.StartAutomatically();

            conf.Service<BoletinService>(sv => {
                sv.ConstructUsingAutofacContainer();
                sv.WhenStarted(s=> s.OnStart());
                sv.WhenStopped(s=>s.OnStop());
                sv.WhenPaused(s=>s.OnPaused());
                sv.WhenContinued(s=>s.OnContinue());
            });
        });
    }

    static void ConfigureLog4Net(string logFile, string maxFileSize, int maxRollBackups, string logLevel)
    {
        var layout = new PatternLayout
        {
            ConversionPattern = "[%date{yyyy-MM-dd HH:mm:ss}] %-5level %logger - %message%newline"
        };
        layout.ActivateOptions();

        // Console Appender
        var consoleAppender = new ConsoleAppender { Layout = layout };

        // File Appender
        var fileAppender = new RollingFileAppender
        {
            File = logFile,
            AppendToFile = true,
            RollingStyle = RollingFileAppender.RollingMode.Size,
            MaxSizeRollBackups = maxRollBackups,
            MaximumFileSize = maxFileSize,
            StaticLogFileName = true,
            Layout = layout
        };
        fileAppender.ActivateOptions();

        // Set log configuration
        var repo = LogManager.GetRepository(Assembly.GetExecutingAssembly());
        BasicConfigurator.Configure(repo, consoleAppender, fileAppender);

        // Set log level
        var hierarchy = (log4net.Repository.Hierarchy.Hierarchy)repo;
        hierarchy.Root.Level = log4net.Core.Level.All;
        hierarchy.Configured = true;
    }

}
