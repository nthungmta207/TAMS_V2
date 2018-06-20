using TAMS_V2.EF;
using System;

namespace TAMS_V2.Services.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TAMDbContext Init();
    }
}