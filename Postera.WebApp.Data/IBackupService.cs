using System.Threading.Tasks;

namespace Postera.WebApp.Data
{
    public interface IBackupService
    {
        Task<byte[]> Backup(string token);
    }
}