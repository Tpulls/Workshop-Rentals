using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQLConnection
{
    /// <summary>
    /// Method to access and query the Database
    /// </summary>
    interface IQueryDatabase
    {
       /// <summary>
       ///  Get DataTable according to tableName
       /// </summary>
       /// <param name="tableName"></param>
       /// <returns></returns>
        DataTable GetDataTable(string tableName);
        /// <summary>
        ///  Get DataTable according to tableName and readonly
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataTable GetDataTable(string tableName, bool isReadOnly);
        /// <summary>
        ///  Get DataTable according to an Sql query and the tableName
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataTable GetDataTable(string sqlQuery, string tableName);
        /// <summary>
        ///  Get DataTable according to Sql query, tableName and readonly
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataTable GetDataTable(string sqlQuery, string tableName, bool isReadOnly);

    }
}
