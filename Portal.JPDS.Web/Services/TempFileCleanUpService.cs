using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.JPDS.Web.Services
{
    public class TempFileCleanUpService : IHostedService
    {
        private readonly ILogger _logger;

        public TempFileCleanUpService(ILogger<TempFileCleanUpService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => TaskFileCleanUp(cancellationToken), cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<Task> TaskFileCleanUp(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(10 * 60 * 1000, cancellationToken);//10 mins wait
                if (!cancellationToken.IsCancellationRequested)
                    DeleteFilesAsync();

                while (!cancellationToken.IsCancellationRequested)
                {
                    DateTime nextStop = DateTime.Now.Add(new TimeSpan(24, 0, 0));
                    await Task.Delay((int)(nextStop - DateTime.Now).TotalMilliseconds, cancellationToken);

                    if (!cancellationToken.IsCancellationRequested)
                        DeleteFilesAsync();
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError("Error in TaskFileCleanUp: ", e.Message, e.StackTrace);
            }
            return Task.CompletedTask;
        }

        private async void DeleteFilesAsync()
        {
            try
            {
                if (Directory.Exists("Temp"))
                {
                    foreach (var file in Directory.EnumerateFiles("Temp"))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if ((DateTime.Now - fileInfo.CreationTime).TotalHours >= 24)
                        {
                            try
                            {
                                fileInfo.Delete();
                            }
                            catch (Exception e)
                            {
                                _logger.LogError("Failed to delete file: " + fileInfo.FullName, e.Message, e.StackTrace);
                            }
                        }
                    }
                }
                else
                {
                    //directory does not exists
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in DeleteFilesAsync: ", ex.Message, ex.StackTrace);
            }
        }
    }
}
