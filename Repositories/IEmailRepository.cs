using EmployMe.Models.DTO.EmailDto;

namespace EmployMe.Repositories
{
    public interface IEmailRepository
    {
        void SendEmail(EmailDto request);
    }
}
