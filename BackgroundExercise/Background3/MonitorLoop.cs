﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundExercise.Background3
{
    public class MonitorLoop
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<MonitorLoop> _logger;
        private readonly CancellationToken _cancellationToken;

        public MonitorLoop(IBackgroundTaskQueue taskQueue,
            ILogger<MonitorLoop> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void StartMonitorLoop()
        {
            _logger.LogInformation("Monitor Loop is starting.");

            // Run a console user input loop in a background thread
            Task.Run(() => Monitor());
        }

        public void Monitor()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var keyStroke = Console.ReadKey();

                if (keyStroke.Key == ConsoleKey.W)
                {
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        int delayLoop = 0;
                        var guid = Guid.NewGuid().ToString();

                        _logger.LogInformation(
                            "Queued Background Task {Guid} is starting.", guid);

                        while (!token.IsCancellationRequested && delayLoop < 3)
                        {
                            try
                            {
                                await Task.Delay(TimeSpan.FromSeconds(5), token);
                            }
                            catch (OperationCanceledException)
                            {
                                _logger.LogInformation("action canceled");
                            }

                            delayLoop++;

                            _logger.LogInformation(
                                "Queued Background Task {Guid} is running. " +
                                "{DelayLoop}/3", guid, delayLoop);
                        }

                        if (delayLoop == 3)
                        {
                            _logger.LogInformation(
                                "Queued Background Task {Guid} is complete.", guid);
                        }
                        else
                        {
                            _logger.LogInformation(
                                "Queued Background Task {Guid} was cancelled.", guid);
                        }
                    });
                }
            }
        }

    }
}
