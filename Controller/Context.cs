using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLConnection;

namespace Controller
{
    public static class Context
    {
        #region Member Variables
        // Declare the SQL variable
        static SQL _sql = new SQL();

        #endregion
        #region Accessors
        // Getting information from the database

        /// <summary>
        /// This method will return all records from the specified database table. 
        /// </summary>
        /// <param name="sqlQuery"> The SELECT query that will be used to filter the records. </param>
        /// <param name="tableName">The source table. </param>
        /// <returns></returns>

        public static DataTable GetDataTable(string tableName)
        {
            return _sql.GetDataTable(tableName);

        }
        /// <summary>
        /// This method will return all records based on the specified SQL query and table name. 
        /// </summary>
        /// <param name="sqlQuery"> The SELECT query that will be used to filter the records. </param>
        /// <param name="tableName">The source table. </param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sqlQuery, string tableName)
        {
            return _sql.GetDataTable(sqlQuery, tableName);

        }
        /// <summary>
        /// This method will return the record based on the specified SQL query. 
        /// </summary>
        /// <param name="sqlQuery">The SELECT query that will be used to filter the records.</param>
        /// <param name="tableName">The source table</param>
        /// <param name="isReadOnly">To indicate whether the returned DataTable is updateable or not</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sqlQuery, string tableName, bool isReadOnly)
        {
            return _sql.GetDataTable(sqlQuery, tableName, isReadOnly);
        }

        #endregion
        #region Mutators

        // Change the state of the database

        /// <summary>
        /// This method will save the record based on the table
        /// </summary>
        public static void SaveDatabaseTable(DataTable table)
        {
            _sql.SaveDatabaseTable(table);
        }
        /// <summary>
        /// This method will insert the parent record based on the table, columnNames and Column Vales
        /// </summary>
        public static int InsertParentTable(string tableName, string columnNames, string columnValues)
        {
            return _sql.InsertParentRecord(tableName, columnNames, columnValues);
        }

        /// <summary>
        /// This method will delete the record based on the table, pkName and pkId
        /// </summary>
        public static void DeleteRecord(string tableName, string pkName, string pkId)
        {
            _sql.DeleteRecord(tableName, pkName, pkId);
        }
        #endregion
    }
}
