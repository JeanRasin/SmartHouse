using SmartHouse.Infrastructure.Data;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace RepositoryTest
{
    [Collection("Logger context")]
    public class LoggerContextTest
    {
        [Fact(Skip = "Возможно и не надо.")] // todo:!!!
        public void GetTableName_AttributeName()
        {
            //var loggerContext = new LoggerContext("","");
            Type type = typeof(LoggerContext);
            var hello = Activator.CreateInstance(type, "mongodb://localhost", "dbname");
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "GetTableName" && x.IsPrivate)
                .First();

            var getTableName = (string)method.Invoke(hello, new object[] { });

            //getTableName.
        }
    }
}