using SmartHouse.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouse.Services.Interfaces
{
    public interface ILoggerWork
    {
        Task<IEnumerable<Logger>> GetLoggerAsync();
    }
}