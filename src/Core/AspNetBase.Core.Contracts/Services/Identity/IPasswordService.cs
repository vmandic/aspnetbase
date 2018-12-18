using System.Threading.Tasks;

namespace AspNetBase.Core.Contracts.Services.Identity
{
    public interface IPasswordService
    {
      Task<bool> ResetPassword(string userEmail);
      Task<bool> ResetPasswordConfirmation(string userEmail, string resetToken);
    }
}
