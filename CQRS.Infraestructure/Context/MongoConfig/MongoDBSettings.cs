using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Infraestructure.Context.MongoConfig
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
