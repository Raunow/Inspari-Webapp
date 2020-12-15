using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace Inspari_API.Services
{
    public interface IDapper : IDisposable
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, object parms, CommandType commandType = CommandType.Text);
        List<T> GetAll<T>(string sp, object parms, CommandType commandType = CommandType.Text);
        int Execute(string sp, object parms, CommandType commandType = CommandType.Text);
        T Insert<T>(string sp, object parms, CommandType commandType = CommandType.Text);
        T Update<T>(string sp, object parms, CommandType commandType = CommandType.Text);
    }
}
