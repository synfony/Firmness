using System.Threading.Tasks;

namespace Firmness.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
