using Bogus;
using MongoDB.Driver;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;

namespace SmartHouseAPI.Contexts
{
    public class LoggerContext : SmartHouse.Infrastructure.Data.LoggerContext
    {
        public LoggerContext(MongoSettings configuration) : base(new MongoClient(configuration.Connection), configuration.DatabaseName)
        {
        }

        public override List<Logger> OnModelCreating()
        {
            var eventIdFaker = new Faker<EventId>()
             .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
             .RuleFor(o => o.Name, f => f.Random.String2(10));

            var loggerModelFaker = new Faker<Logger>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                 .RuleFor(o => o.CategoryName, f => "Category test")
                 .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                 .RuleFor(o => o.LogLevel, f => f.PickRandom<Microsoft.Extensions.Logging.LogLevel>())
                 .RuleFor(o => o.Message, f => f.Random.Words())
                 .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)));

            return loggerModelFaker.Generate(10);
        }
    }
}