using TAMS_V2.EF;

namespace TAMS_V2.Services.Infrastructure
{
    public class DbFactory : IDbFactory
    {
        private TAMDbContext dbContext;

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        public TAMDbContext Init()
        {
            return dbContext ?? (dbContext = new TAMDbContext());
        }
    }
}