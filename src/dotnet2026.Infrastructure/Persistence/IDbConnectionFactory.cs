using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace dotnet2026.Infrastructure.Persistence
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
