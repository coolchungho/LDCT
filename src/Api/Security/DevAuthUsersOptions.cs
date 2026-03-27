namespace LDCT.Api.Security;

public class DevAuthUsersOptions
{
    public const string SectionName = "DevAuthUsers";
    public List<DevUserRecord> Users { get; set; } = new();
}
