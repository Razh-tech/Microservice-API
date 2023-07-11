using System.Security.Claims;
using KLG.Backend.Organization.Models.Request;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KLG.Backend.Organization.Services.Controllers.RestApi;

[Route("[controller]")]
public class KeyController : Controller
{
    IKLGConfiguration ConfigurationProvider;

    public KeyController(IKLGConfiguration configuration)
    {
        ConfigurationProvider = configuration;
    }

    /// <summary>
    /// Generate private and public key to be used for creating and validating JWT.
    /// </summary>
    /// <returns>Returns an object containing the generated private and public keys.</returns>
    [HttpGet]
    [Route("key")]
    public IActionResult GenerateKeyPair()
    {
        var jwtManager = new KLGTokenManager();

        var (privateKey, publicKey) = jwtManager.GetKey();
        return new OkObjectResult(new { PrivateKey = privateKey, PublicKey = publicKey });
    }

    /// <summary>
    /// Creates a JWT token based on the provided token information.
    /// </summary>
    /// <param name="tokenInfo">The token information.</param>
    /// <returns>Returns the generated JWT token.</returns>
    [HttpPost]
    [Route("token")]
    public IActionResult CreateToken([FromBody] CreateTokenDTO tokenInfo)
    {
        var jwtManager = new KLGTokenManager();

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, tokenInfo.UserId),
            new Claim(ClaimTypes.Name, tokenInfo.UserName),
        };

        foreach (var role in tokenInfo.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = jwtManager.GenerateToken(
            ConfigurationProvider.Security.PrivateKey,
            ConfigurationProvider.Security.Audience,
            ConfigurationProvider.Security.Issuer,
            claims);

        return new OkObjectResult(token);
    }

    /// <summary>
    /// Retrieves the information stored in the JWT token of the authenticated user.
    /// </summary>
    /// <returns>Returns the parsed claims from the JWT token.</returns>
    [Authorize]
    [HttpGet]
    [Route("info")]
    public IActionResult GetTokenInfo()
    {
        return Ok(KLGTokenManager.ParseClaims(User.Claims));
    }
}
