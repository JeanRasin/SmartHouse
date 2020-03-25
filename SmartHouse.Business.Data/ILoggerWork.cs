using SmartHouse.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public interface ILoggerWork
    {
        Task<IEnumerable<LoggerModel>> GetLoggerAsync();
    }
}