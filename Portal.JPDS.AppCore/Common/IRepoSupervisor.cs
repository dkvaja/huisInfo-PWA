using Portal.JPDS.AppCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Common
{
    /// <summary>
    /// Interface for Repository
    /// </summary>
    public interface IRepoSupervisor : IDisposable
    {
        IActionRepository Actions { get; }
        IAttachmentsRepository Attachments { get; }
        IBuildingOptionsRepository BuildingOptions { get; }
        IChatRepository Chats { get; }
        IConfigRepository Config { get; }
        IDossierRepository Dossiers { get; }
        IEmployeeRepository Employees { get; }
        IFAQRepository FAQs { get; }
        ILoginRepository Logins { get; }
        INewsRepository News { get; }
        INotificationRepository Notifications { get; }
        IOrganisationRepository Organisations { get; }
        IUserObjectRepository UserObjects { get; }
        IPersonRepository Persons { get; }
        IPlanningRepository Plannings { get; }
        IProjectRepository Project { get; }
        IRepairRequestRepository RepairRequest { get; }
        ISurveyRepository Survey { get; }
        int Complete(string username = null);
        int CentralComplete();
    }
}
