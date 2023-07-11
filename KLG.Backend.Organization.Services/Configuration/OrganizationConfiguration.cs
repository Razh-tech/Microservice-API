using KLG.Library.Microservice.Configuration.Interface;

namespace KLG.Backend.Organization.Services.Configuration;

/// <summary>
/// Represents the configuration for the organization.
/// </summary>
public class OrganizationConfiguration : IKLGConfigurationSection
{
    /// <summary>
    /// Gets or sets the path of the configuration in the vault.
    /// </summary>
    public string Path => "OrganizationConfiguration";

    /// <summary>
    /// Gets or sets the minimum age required for the organization.
    /// </summary>
    public int MinimumAge { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationConfiguration"/> class with the specified minimum age.
    /// </summary>
    /// <param name="minimumAge">The minimum age required for the organization.</param>
    public OrganizationConfiguration(int minimumAge)
    {
        MinimumAge = minimumAge;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationConfiguration"/> class.
    /// </summary>
    public OrganizationConfiguration() { }
}
