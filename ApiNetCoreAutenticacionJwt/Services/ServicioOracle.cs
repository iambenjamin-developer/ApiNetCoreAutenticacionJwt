using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using System.Text;
using ApiNetCoreAutenticacionJwt.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace ApiNetCoreAutenticacionJwt.Services
{
    public class ServicioOracle
    {
        public bool ValidarLogin(LoginUser loginUser)
        {
            string conexionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=CYRE;Password=123456;";

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT COUNT(*) ");
            sqlQuery.Append("FROM LOGIN ");
            sqlQuery.Append("WHERE USUARIO = :usuarioParametro ");
            sqlQuery.Append("AND CLAVE = :claveParametro ");



            int entero = 0;
            using (var connection = new OracleConnection(conexionString))
            {
                var parameters = new { usuarioParametro = loginUser.Usuario, claveParametro = loginUser.Clave };

                entero = connection.ExecuteScalar<int>(sqlQuery.ToString(), parameters);
            }

            if (entero == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
