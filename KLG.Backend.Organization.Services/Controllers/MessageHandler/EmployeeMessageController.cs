using DotNetCore.CAP;
using KLG.Backend.Organization.Models.Message;
using KLG.Backend.Organization.Services.Entities;
using KLG.Backend.Organization.Services.Business;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice;
using Microsoft.AspNetCore.Mvc;
using KLG.Backend.Organization.Services.Business.Employees;
using KLG.Backend.Organization.Services.Business.Companies;

namespace KLG.Backend.Organization.Services.Controllers.MessageHandler;

[Route("[controller]")]
public class EmployeeMessageController : KLGApiController<DefaultDbContext>
{
    private EmployeeManager _employeeManager;

    public EmployeeMessageController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, IKLGRestApi restApi, Serilog.ILogger logger)
        : base(configuration, dbProvider, messageProvider, restApi, logger)
    {
        _employeeManager = new EmployeeManager(DbProvider, MessageProvider, ConfigurationProvider);
    }

    /// <summary>
    /// Subscriber method to handle EmployeeCreated messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee created DTO payload.</param>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeCreated))]
    public async Task EmployeeCreatedSubscriber(KLGMessage p)
    {
        var emp = KLGMessagingProvider.GetPayloadAsync<EmployeeCreated>(p)
            ?? throw new InvalidOperationException("Payload is null");

        await new HeadcountManager(DbProvider).RecruitNewEmployee();
    }

    /// <summary>
    /// Subscriber method to handle "employee.deleted" messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee deleted DTO payload.</param>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleted))]
    public async Task EmployeeDeletedSubscriber(KLGMessage p)
    {
        var emp = KLGMessagingProvider.GetPayloadAsync<EmployeeDeleted>(p)
            ?? throw new InvalidOperationException("Payload is null");

        await new HeadcountManager(DbProvider).EmployeeResignation();
    }

    /// <summary>
    /// Subscriber method to handle EmployeeDeleteRequest messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee delete request DTO payload.</param>
    /// <returns>The employee delete approval DTO.</returns>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleteRequest))]
    public EmployeeDeleteApproval EmployeeDeleteRequestSubscriber(KLGMessage p)
    {
        var message = KLGMessagingProvider.GetPayloadAsync<EmployeeDeleteRequest>(p)
            ?? throw new InvalidOperationException("Invalid message");

        return new EmployeeDeleteApproval()
        {
            Id = message.Id,
            Approved = _employeeManager.DeleteApproval(message.Name)
        };
    }

    /// <summary>
    /// Subscriber method to handle EmployeeDeleteApproval messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee delete approval DTO payload.</param>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleteApproval))]
    public async Task EmployeeDeleteApprovalSubscriber(KLGMessage p)
    {
        var message = KLGMessagingProvider.GetPayloadAsync<EmployeeDeleteApproval>(p)
            ?? throw new InvalidOperationException("Invalid message");

        if (message.Approved)
        {
            await _employeeManager.Deactivate(message.Id);
        }
        else
        {
            Logger.Information($"Deletion of employee {message.Id} was rejected");
        }
    }
}
