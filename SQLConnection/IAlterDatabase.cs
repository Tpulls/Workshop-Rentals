using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLConnection
{
    /// <summary>
    /// Method to alter the Database
    /// </summary>
    interface IAlterDatabase
    {
        /// <summary>
        /// Create Database Method
        /// </summary>
        void CreateDatabase();
        /// <summary>
        /// Create Database Table Method
        /// </summary>
        void CreateDatabaseTable(string tableName, string tableStructure);
        /// <summary>
        /// Alter DatabaseTable method
        /// </summary>
        void AlterDatabaseTable(string tableName, string tableStructure);
        /// <summary>
        /// Save DatabaseTable method
        /// </summary>
        void SaveDatabaseTable(DataTable table);
        /// <summary>
        /// Insert DatabaseTable Method
        /// </summary>
        int InsertRecord(string tableName, string tableStructure, string columnValues);
        /// <summary>
        /// Insert DatabaseTable parent recor Method
        /// </summary>
        int InsertParentRecord(string tableName, string tableStructure, string columnValues);
        /// <summary>
        /// Updated DatabaseTable Method
        /// </summary>
        bool UpdatedRecord(string tableName, string columnNamesAndValues, string criteria);
        /// <summary>
        /// Delete DatabaseTable record Method
        /// </summary>
        void DeleteRecord(string tableName, string pkName, string pkID);
    }
}
