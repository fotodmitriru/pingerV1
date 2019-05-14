using Microsoft.EntityFrameworkCore;

namespace AppPinger.DB.ConfigureProviders
{
    public interface IConfigureDb
    {
        DbSet<LogModel> LogsModel { get; set; }
    }
}
