using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using NLog;

namespace SQLConnection
{
    /// <summary>
    /// Method for the SQL Connection and its relation to the Query and Alter database methods
    /// </summary>
    public class SQL : IQueryDatabase, IAlterDatabase
    {
        #region Member Variables

        //Declare the variables
        // Initialize variable for SqlConnection
        SqlConnection _sqlConnection = null;
        // Initialize variable for SqlCommand
        SqlCommand _sqlCommand = null;
        // Initialize variable logger
        private Logger _log;

        #endregion

        #region Constructor

        /// <summary>
        ///  SQL Connection method
        /// </summary>
        public SQL()
        {
            // NLog to be added to all try catch statements
            LogManager.LoadConfiguration("NLog.config");
            _log = LogManager.GetCurrentClassLogger();
            // Get the connection string from app.config
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            // Initialize and create a new SqlConnection object that is needed to connect to a SQl server
            _sqlConnection = new SqlConnection(connectionString);
        }
        #endregion

        #region AlterDatabase
        /// <summary>
        /// Alter DatabaseTable method
        /// </summary>
        public void AlterDatabaseTable(string tableName, string tableStructure)
        {
            try
            {
                string sqlQuery = $"ALTER TABLE {tableName} ({tableStructure})";
                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed) _sqlConnection.Open();
                    _sqlCommand.ExecuteNonQuery();
                    _sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
        }

        /// <summary>
        /// Create Database Method
        /// </summary>
        public void CreateDatabase()
        {
            // Create a serrver connectionstring which will only have the data source specified to create database
            string serverConnectionString = $"Data Source = {_sqlConnection.DataSource};" + $"Integrated Security=True";
            // Declare and initialize a string to store our SQL script to create a database
            string sqlQuery =
                $"IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name ='{_sqlConnection.Database}') " +
                $"CREATE DATABASE {_sqlConnection.Database}";
            //Create another SqlConnection object using the connection string without the Database name to elimate connection error
            SqlConnection serverConnection = new SqlConnection(serverConnectionString);
            //Create a SqlCommand object that is needed to execute SQl script on a specified SQL Database server
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, serverConnection))
            {
                //Check if SqlConnection ovject is closed before opneing otherwise an error will occur
                if (serverConnection.State == ConnectionState.Closed)
                {

                    serverConnection.Open();
                }
                //Run the SQL script using the SQL command object
                sqlCommand.ExecuteNonQuery();

                //Close the SqlConnection as soon as we are done with it
                serverConnection.Close();

            }
        }
        /// <summary>
        /// This will create a database on a specified server
        /// </summary>
        /// <param name="tableName"></param>" The table name to be created
        /// <param name="tableStructure"></param> The table structure or schema
        /// 
        public void CreateDatabaseTable(string tableName, string tableStructure)
        {

            try
            {
                // TODO: Modify the query when profilling.
                // This query will create a database table based on the specified table structure.
                string sqlQuery = $"IF NOT EXISTS (SELECT name FROM sysobjects " +
                    $"WHERE name = '{tableName}') " +
                    $"CREATE TABLE {tableName} ({tableStructure})";

                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    _sqlCommand.ExecuteNonQuery();
                    _sqlConnection.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
        }

        /// <summary>
        /// This method will delete a record in the database
        /// </summary>
        /// <param name="tableName">Table Name</param>
        /// <param name="pkName">pkName</param>
        /// <param name="pkId">pkId</param>
        public void DeleteRecord(string tableName, string pkName, string pkID)
        {
            // Create and assign a new SQL Query
            string sqlQuery =
                $"DELETE FROM {tableName} WHERE {pkName}={pkID}" +
                "SELECT SCOPE_IDENTITY()";

            // Try to execute the query
            // Catch any errors
            // Finally close the SQL Connection
            try
            {
                // Assign a new SQL Command to _sqlCmd
                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    // Check if the SQL Connection is closed
                    // Open the SQL Connection
                    if (_sqlConnection.State == ConnectionState.Closed)
                    {
                        _sqlConnection.Open();
                    }

                    // Execute the query
                    _sqlCommand.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                // Log the error to the console
                // log the error to the logger
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            finally
            {
                // Close the SQL Connection
                _sqlConnection.Close();
            }
        }

        /// <summary>
        /// This method will insert a record in the database. 
        /// </summary>
        /// <param name="tableName">Destination table</param>
        /// <param name="columnNames">Column Names</param>
        /// <param name="columnValues">Column Values</param>
        /// <returns>int NewId</returns>
        public int InsertParentRecord(string tableName, string columnNames, string columnValues)
        {
            int Id = 0;

            try
            {
                string sqlQuery = $"INSERT INTO {tableName} ({columnNames}) " +
                    $"VALUES ({columnValues}) " +
                    $"SELECT SCOPE_IDENTITY()";

                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed) _sqlConnection.Open();
                    var output = _sqlCommand.ExecuteScalar();
                    if (!(output is DBNull))
                    {
                        Id = (int)(decimal)output;
                    }
                    _sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            return Id;
        }

        /// <summary>
        /// This method will insert a record in the table.
        /// </summary>
        /// <param name="tableName">Table to insert record into</param> 
        /// <param name="columnNames">Column names of the table</param>
        /// <param name="columnValues">Values of each column of the table</param>
        /// <returns></returns>
        public int InsertRecord(string tableName, string columnNames, string columnValues)
        {
            int id = 0;
            string sqlQuery =
                $"SET IDENTITY_INSERT {tableName} ON "
                + $"INSERT INTO {tableName} ({columnNames}) "
                + $"VALUES ({columnValues}) "
                + $"SET IDENTITY_INSERT {tableName} OFF "
                + $"SELECT SCOPE_IDENTITY() ";
            try
            {
                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    id = (int)(decimal)_sqlCommand.ExecuteScalar();
                    _sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            return id;
        }

        /// <summary>
        /// Save DatabaseTable method
        /// </summary>
        public void SaveDatabaseTable(DataTable table)
        {
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {table.TableName}", _sqlConnection))
                {
                    //Using the SqlCommand Builder to create the insert, update, and delete command automatically based on the query 
                    //above when initializing a SqlAdapter
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.InsertCommand = commandBuilder.GetInsertCommand();
                    adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                    adapter.DeleteCommand = commandBuilder.GetDeleteCommand();

                    if (_sqlConnection.State == ConnectionState.Closed) _sqlConnection.Open();
                    adapter.Update(table);
                    _sqlConnection.Close();
                    table.AcceptChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
        }

        /// <summary>
        /// Updated DatabaseTable Method
        /// </summary>
        public bool UpdatedRecord(string tableName, string columnNamesAndValues, string criteria)
        {
            bool isOk = false;
            string sqlQuery = $"UPDATE {tableName} SET {columnNamesAndValues} WHERE {criteria}";
            try
            {
                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed) _sqlConnection.Open();
                    _sqlCommand.ExecuteNonQuery();
                    isOk = true;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                isOk = false;
                _log.Error(e.ToString());
            }
            return isOk;
        }


        #endregion
        #region Query Database
        /// <summary>
        ///  This method will get an updateable table from the database 
        /// </summary>
        /// <param name="tableName">The source table</param>
        /// <returns></returns>
        public DataTable GetDataTable(string tableName)
        {
            DataTable table = new DataTable(tableName);

            try
            {
                // Using a SqlDataAdapater allows us to make a DataTable updateable as it represents a set of data commands
                // and connection that are used to update a SQL database. 
                using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    // based on the SELECT query above, the SqlAdapter built-in command object will send the Sql query request to SQL.
                    // SQL will then return the corresponding data set and populate our DataTable called 'table'. 
                    adapter.Fill(table);
                    _sqlConnection.Close();

                    // Configure our DataTable and specify the Primary Key, which is in column 0 (or the first column)
                    table.PrimaryKey = new DataColumn[] { table.Columns[0] };

                    // Specify tha the primary key in column 0 is auto-increment
                    table.Columns[0].AutoIncrement = true;

                    // Seed the primary key value by using the last PKID value. Seeding the primary key value is to simply set the
                    // starting value of the auto-increment.
                    if (table.Rows.Count > 0)
                        table.Columns[0].AutoIncrementSeed = long.Parse(table.Rows[table.Rows.Count - 1][0].ToString());

                    // set the auto-increment step to 1
                    table.Columns[0].AutoIncrementStep = 1;

                    // Setting the colkumns to not read only
                    foreach (DataColumn col in table.Columns)
                    {
                        col.ReadOnly = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            return table;
        }

        /// <summary>
        /// This method will get a Read-Only table from the Database. 
        /// </summary>
        /// <param name="tableName">Source Table</param>
        /// <param name="isReadOnly">Specify if table is Read-Only</param>
        /// <returns></returns>
        public DataTable GetDataTable(string tableName, bool isReadOnly)
        {
            if (!isReadOnly) return GetDataTable(tableName);
            DataTable table = new DataTable(tableName);

            try
            {
                using (_sqlCommand = new SqlCommand($"SELECT * FROM {tableName}", _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed) _sqlConnection.Open();
                    using (SqlDataReader reader = _sqlCommand.ExecuteReader())
                    {
                        table.Load(reader);
                        _sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            return table;
        }

        /// <summary>
        ///  Get DataTable according to an Sql query and the tableName
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlQuery, string tableName)
        {
            DataTable table = new DataTable(tableName);

            try
            {
                // Using a SqlDataAdapater allows us to make a DataTable updateable as it represents a set of data commands
                // and connection that are used to update a SQL database. 
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    // based on the SELECT query above, the SqlAdapter built-in command object will send the Sql query request to SQL.
                    // SQL will then return the corresponding data set and populate our DataTable called 'table'. 
                    adapter.Fill(table);
                    _sqlConnection.Close();

                    // Configure our DataTable and specify the Primary Key, which is in column 0 (or the first column)
                    table.PrimaryKey = new DataColumn[] { table.Columns[0] };

                    // Specify tha the primary key in column 0 is auto-increment
                    table.Columns[0].AutoIncrement = true;

                    // Seed the primary key value by using the last PKID value. Seeding the primary key value is to simply set the
                    // starting value of the auto-increment.
                    if (table.Rows.Count > 0)
                        table.Columns[0].AutoIncrementSeed = long.Parse(table.Rows[table.Rows.Count - 1][0].ToString());

                    // set the auto-increment step to 1
                    table.Columns[0].AutoIncrementStep = 1;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            return table;
        }

        /// <summary>
        ///  Get DataTable according to Sql query, tableName and readonly
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlQuery, string tableName, bool isReadOnly)
        {
            if (!isReadOnly) return GetDataTable(tableName);
            DataTable table = new DataTable(tableName);

            try
            {
                using (_sqlCommand = new SqlCommand(sqlQuery, _sqlConnection))
                {
                    if (_sqlConnection.State == ConnectionState.Closed) _sqlConnection.Open();
                    using (SqlDataReader reader = _sqlCommand.ExecuteReader())
                    {
                        table.Load(reader);
                        _sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
            }
            return table;
        }

        #endregion

    }
}
