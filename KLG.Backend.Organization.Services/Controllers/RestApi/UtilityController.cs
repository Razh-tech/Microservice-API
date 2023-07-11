using DotNetCore.CAP;
using KLG.Backend.Organization.Services.Entities;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice;
using Microsoft.AspNetCore.Mvc;

namespace KLG.Backend.Organization.Services.Controllers.RestApi;

[Route("[controller]")]
public class UtilityController : KLGApiController<DefaultDbContext>
{
    public UtilityController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, IKLGRestApi restApi, Serilog.ILogger logger)
    : base(configuration, dbProvider, messageProvider, restApi, logger) { }

    /// <summary>
    /// Resets the employee data to its default state.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating whether the reset operation was successful.</returns>
    [HttpPost]
    [Route("reset")]
    public async Task<IActionResult> Reset([FromServices] IBootstrapper bootstrapper)
    {
        await bootstrapper.DisposeAsync();

        await DbProvider.ResetTablesAsync(typeof(Employee));

        await bootstrapper.BootstrapAsync();

        return Ok();
    }

    /// <summary>
    /// Resets all data in the database to its default state.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating whether the reset operation was successful.</returns>
    [HttpPost]
    [Route("resetall")]
    public async Task<IActionResult> ResetAll([FromServices] IBootstrapper bootstrapper)
    {
        await bootstrapper.DisposeAsync();

        await DbProvider.ResetTablesAsync();

        await bootstrapper.BootstrapAsync();

        return Ok();
    }
}
