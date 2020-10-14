using System.Threading.Tasks;

namespace Covario.AuditAdminApp.Integration.Authorisation
{
    public interface IIntrospectionValidator
    {
        Task<bool> ValidateToken(string token);
    }
}