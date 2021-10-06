using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.JPDS.WorkerService
{
    public class DailyEmailNotification : BackgroundService
    {
        private readonly ILogger<DailyEmailNotification> _logger;
        private readonly AppSettings _appSettings;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DailyEmailNotification(
            ILogger<DailyEmailNotification> logger,
            IOptions<AppSettings> appSettings,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyEmailNotification: Sync Task started at:{0}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested && _appSettings.SendDailyEmailNotificationsSetting.Enabled)
            {
                //Logic to execute it once a day at given hour
                DateTime nextStop = DateTime.Now.Date.Add(_appSettings.SendDailyEmailNotificationsSetting.Time);
                if (DateTime.Now.TimeOfDay > _appSettings.SendDailyEmailNotificationsSetting.Time)
                {
                    nextStop = nextStop.AddDays(1);
                }
                var timeToWait = nextStop - DateTime.Now;
                var millisToWait = timeToWait.TotalMilliseconds;

                await Task.Delay((int)millisToWait, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)//checking again if there is a cancellation during the Delay period.
                {
                    _logger.LogInformation("DailyEmailNotification - Worker running at: {time}", DateTime.Now);
                    try
                    {
                        SendNotificationsAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("DailyEmailNotification-TaskRoutine: {0}\n{1}", ex.Message, ex.StackTrace ?? string.Empty);
                    }
                }
                else
                {
                    _logger.LogInformation("DailyEmailNotification - Worker cancellation at: {time}", DateTime.Now);
                }
            }
        }

        private async void SendNotificationsAsync()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    using (var _repoSupervisor = scope.ServiceProvider.GetRequiredService<IRepoSupervisor>())
                    {
                        var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        var notifications = _repoSupervisor.Notifications.GetEmailNotifications();
                        var emailTemplate = _repoSupervisor.Config.GetStandardNotificationEmailTemplate();

                        foreach (var notification in notifications)
                        {
                            try
                            {
                                using (MailMessage mail = new MailMessage())
                                {
                                    _logger.LogDebug(
                                        "SendNoficationsAsync: Data - \nProject: {0}\nBuilding: {1}\nEmail: {2}\nNotifications:\n{3}\n\n",
                                        notification.ProjectNoAndName,
                                        notification.BuildingNoExtern,
                                        notification.Email,
                                        string.Join("\n", notification.Messages)
                                        );
                                    //mail.To.Add("abhi@aviminds.com");
                                    mail.To.Add(notification.Email);
                                    mail.Bcc.Add("jpdatabasesolutions@gmail.com");
                                    mail.Subject = emailTemplate.Subject
                                        .Replace("[werknaam]", notification.ProjectNoAndName, StringComparison.InvariantCulture)
                                        .Replace("[bouwnummer_extern]", notification.BuildingNoExtern, StringComparison.InvariantCulture);

                                    mail.Body = emailTemplate.TemplateHtml
                                        .Replace("[geachte]", notification.LetterSalutationFormal, StringComparison.InvariantCulture)
                                        .Replace("[geachte_informeel]", notification.LetterSalutationInformal, StringComparison.InvariantCulture)
                                        .Replace("[notificaties]", "<ul><li>" + string.Join("</li><li>", notification.Messages) + "</li></ul>", StringComparison.InvariantCulture)
                                        .Replace("[hoofdaannemer]", notification.MainContractorName, StringComparison.InvariantCulture);
                                    mail.IsBodyHtml = true;

                                    await _emailService.SendEmailAsync(mail).ConfigureAwait(false);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("SendNotificationsAsync: {0}\n{1}", ex.Message, ex.StackTrace ?? string.Empty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("DailyEmailNotificationConstructionQuality - SendNotificationsAsync: {0}\n{1}", ex.Message, ex.StackTrace ?? string.Empty);
            }
        }
    }
}
