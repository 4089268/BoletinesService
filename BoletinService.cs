using System;
using BoletinesService.Jobs;
using Quartz;

namespace BoletinesService;

public class BoletinService
{
    private IScheduler Scheduler { get; }

    public BoletinService(IScheduler scheduler) {
        this.Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
    }
        
    public void OnStart()
    {
        // * Define the jobs
        IJobDetail job1 = JobBuilder.Create<LogJob>()
            .WithIdentity(typeof(LogJob).Name, SchedulerConstants.DefaultGroup)
            .Build();
        
        // * Create a trigger that fires on a cron schedule (every 30 minutes in this example)
        ITrigger trigger1 = TriggerBuilder.Create()
            .WithIdentity(typeof(LogJob).Name + "Trigger", SchedulerConstants.DefaultGroup)
            .WithCronSchedule("0/5 * * * * ?")
            .ForJob(job1)
            .Build();


        // *  Schedule the job with the trigger and start the scheduler
        Scheduler.ScheduleJob(job1, trigger1);
        Scheduler.Start();

        // Manually trigger the jobs to run immediately
        // Scheduler.TriggerJob(job2.Key);
    }

    public void OnStop() {
        Scheduler.Shutdown();
    }

    public void OnPaused() {
        Scheduler.PauseAll();
    }

    public void OnContinue() {
        Scheduler.ResumeAll();
    }

}
