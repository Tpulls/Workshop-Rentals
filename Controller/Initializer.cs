using SQLConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Initializer
    {
        #region Member Variables
        // Declare the SQl variable
        static SQL _sql = new SQL();

        #endregion

        #region Initialize Database
        /// <summary>
        /// This method will initialize the Database
        /// </summary>
        public static void InitializeDatabase()
        {
        // Call the CreateDatabase method from our SQL Class to build the database in SQL server
        _sql.CreateDatabase();
        CreateDatabaseTable();
        SeedDatabaseTable();
        }

        #endregion

        #region Create Database Schema

        /// <summary> 
        /// This method will create the database tables.
        /// </summary>
        public static void CreateDatabaseTable()
        {
            CreateToolTable();
            CreateRentalTable();
            CreateWorkspaceTable();
            CreateCustomersTable();
            CreateRentalItems();
        }

        /// <summary>
        /// This method will create the Tool Table
        /// </summary>
        public static void CreateToolTable()
        {
            // Create the Tool table schema
            string schema = "ToolID int IDENTITY(1,1) PRIMARY KEY, " + "ToolDescription VARCHAR(70), " +
                "Brand VARCHAR(70), " + "AssetNumber VARCHAR(70), "  + "Status VARCHAR(70), " + "Availability VARCHAR(30), " + "RentalID VARCHAR(40), " 
                + "Comments VARCHAR(70)";
            // Call the create DatabaseTable method of the SQL clas to create the Tool Table
            _sql.CreateDatabaseTable("Tool", schema);
        }
        /// <summary>
        /// This method will create the Rental Table
        /// </summary>
        /// 
        public static void CreateRentalTable()
        {
            // Create the Tool Rental schema
            string schema = "RentalID int IDENTITY(1,1) PRIMARY KEY, " + "CustomerID VARCHAR(40), " + "WorkspaceID VARCHAR(40), " + 
                "DateRented DATETIME NOT NULL, " + "DateReturned DATETIME NULL";
            // Call the Create DatabaseTable method of the SQl class to create the customer table
            _sql.CreateDatabaseTable("Rental", schema);

        }
        /// <summary>
        /// This method will create the Workspace Table
        /// </summary>
        /// 
        public static void CreateWorkspaceTable()
        {
            // Create the Tool Rental schema
            string schema = "WorkspaceID int IDENTITY(1,1) PRIMARY KEY, " + "WorkspaceName VARCHAR(70)";
            // Call the Create DatabaseTable method of the SQl class to create the customer table
            _sql.CreateDatabaseTable("Workspace", schema);

        }

        /// <summary>
        /// This method will create the Customers Table
        /// </summary>
        /// 
        public static void CreateCustomersTable()
        {
            // Create the Tool Rental schema
            string schema = "CustomerID int IDENTITY(1,1) PRIMARY KEY, " + "CustomerName VARCHAR(70), " + "Phone VARCHAR(70), " + 
                "Email VARCHAR(70)";
            // Call the Create DatabaseTable method of the SQl class to create the customer table
            _sql.CreateDatabaseTable("Customer", schema);

        }

        /// <summary>
        /// This method will create the RentalItems bridging table
        /// </summary>
        private static void CreateRentalItems()
        {
            string schema = "RentalItemID int IDENTITY(1,1) PRIMARY KEY, " + "RentalID int NOT NULL, " + "ToolID int NOT NULL";
            // Call the Create Database method of the SQL class to create RentalItems table
            _sql.CreateDatabaseTable("RentalItems", schema);
        }


        #endregion

        #region Seed Database Tables
        /// <summary>
        /// This method will seed the coded information to the tables
        /// </summary>
        public static void SeedDatabaseTable()
        {
            SeedToolTable();
            SeedRentalTable();
            SeedCustomerTable();
            SeedWorkSpaceTable();
            seedRentalItemsTable();
        }

        /// <summary>
        /// This method will seed the informtion to the Tool Table
        /// </summary>
        public static void SeedToolTable()
        {
            List<string> tools = new List<string>
            {
                // ToolID, ToolDescription, Brand, AssetNumber, Status, Availability, RentalID, Comments
                "1, 'Hammer', 'Toshiba', '21512', 'Operational', 'Available', 3, 'Hammer is in good condition'",
                "2, 'Drill', 'Samsung', '53436', 'In-use', 'Rented', 1, 'Drill in in fine condition'",
                "3, 'Saw', 'Apple', '59693', 'Retired', 'Available', 2, 'Blade worn, item no longer fit for operation.'",
            };
            // ColumnNames must watch the order of the initialize data above
            string columnNames = "ToolID, ToolDescription, Brand, AssetNumber, Status, Availability, RentalID, Comments";
            // Loop through the list and push the data to the database table
            foreach (var tool in tools)
            {
                _sql.InsertRecord("Tool", columnNames, tool);
            }
        }

        /// <summary>
        /// This method will seed the informtion to the Rental Table
        /// </summary>
        public static void SeedRentalTable()
        {
            List<string> rentals = new List<string>
            {
                // RentalID, CustomerID, WorkspaceID, DateRented, Date Returned
                "1, 2, 1, '05/04/2019', null",
                "2, 1, 3, '12/04/2019', null",
                "3, 3, 2, '11/05/2019', '12/12/2019'",
            };
            // ColumnNames must watch the order of the initialize data above
            string columnNames = "RentalID, CustomerID, WorkspaceID, DateRented, DateReturned";
            // Loop through the list and push the data to the database table
            foreach (var rental in rentals)
            {
                _sql.InsertRecord("Rental", columnNames, rental);
            }
        }


        /// <summary>
        /// This method will seed the informtion to the Customer Table
        /// </summary>
        private static void SeedCustomerTable()
        {
            List<string> customers = new List<string>
            { 
            // CustomerID, CustomerName, Phone, Email

                "1, 'Sarah Wallace', '0412 345 678', 'fake@gmail.com'",
                "2, 'William Reed', '3842 8858', 'lemon@hotmail.com'",
                "3, 'Lucy See', '0432 456 765', 'lucy@gmail.com'",
            };
            // ColumnName must watch the oredr of the initialie data aboce
            string columnNames = "CustomerID, CustomerName, Phone, Email";
            // Loop through the list and push the data to the database table
            foreach (var customer in customers)
            {
                _sql.InsertRecord("Customer", columnNames, customer);
            }
        }

        /// <summary>
        /// This method will seed the informtion to the Workspace Table
        /// </summary>
        private static void SeedWorkSpaceTable()
        {
            List<string> workspaces = new List<string>
            {
                // WorkspaceID, WorkspaceName
                "1, 'Workspace 1'",
                "2, 'Workspace 2'",
                "3, 'Workspace 3'",
            };
            // Column names to suit create table
            string columnNames = "WorkspaceID, WorkspaceName";

            foreach (var workspace in workspaces)
            {
                _sql.InsertRecord("Workspace", columnNames, workspace);
            }
        }


        /// <summary>
        /// This method will seed the informtion to the RentalItems Table
        /// </summary>
        private static void seedRentalItemsTable()
        {
            List<string> rentalItems = new List<string>
            {
                // RentalItemID, RentalID, ToolID
                "1, 1, 1",
                "2, 2, 3",
                "3, 3, 2"
            };
            // Assign Column name
            string columnNames = "RentalItemID, RentalID, ToolID";

            foreach(var rentalItem in rentalItems)
            {
                _sql.InsertRecord("RentalItems", columnNames, rentalItem);
            }
        }
        #endregion
    }
}
