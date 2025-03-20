using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace qlsieuthi
{
    public class lib
    {
        private string connection = "server= 103.195.236.138;port= 3306;Database= qlsieuthi; Uid=team6;Pwd= Team6@1234!;CharSet=utf8;Allow User Variables=true;";
        public MySqlConnection sql()
        {
            return new MySqlConnection(connection);
        }
    }
}
