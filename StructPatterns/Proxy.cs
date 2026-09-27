using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructPatterns
{
    public interface Database
    {
        void Query(string sql);
    }
    public class RealDatabase : Database
    {
        public void Query(string sql)
        {
            Console.WriteLine("Executing query: " + sql);
        }
    }
    public class DatabaseProxy : Database
    {
        private readonly RealDatabase realDatabase;
        private readonly bool hasAcc;

        public DatabaseProxy(bool hasAccess)
        {
            realDatabase = new RealDatabase();
            hasAcc = hasAccess;
        }

        public void Query(string sql)
        {
            if (hasAcc)
            {
                realDatabase.Query(sql);
            }
            else
            {
                Console.WriteLine("Access denied. Query cannot be executed.");
            }
        }
    }
}
