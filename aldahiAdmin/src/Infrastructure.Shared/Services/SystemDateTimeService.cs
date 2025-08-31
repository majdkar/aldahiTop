using FirstCall.Application.Interfaces.Services;
using System;

namespace FirstCall.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}