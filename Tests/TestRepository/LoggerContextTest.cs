using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SmartHouse.Infrastructure.Data;
using System.Reflection;
using System.Linq;

namespace RepositoryTest
{
    [CollectionDefinition("Logger context")]
    public class LoggerContextTest
    {

        [Fact(Skip ="!!!")]
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
