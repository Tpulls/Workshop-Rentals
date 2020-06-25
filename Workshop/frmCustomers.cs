using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmCustomers : Form
    {

        #region Member Variables
        // Create a variable for the Primary Key
        long _PKID = 0;
        // Create a variable for the DataTable and Tool DataTable
        DataTable _customerTable = null;
        // Create a variable for the New DataTable
        bool _isNew = false;


        #endregion
        #region Constructors

        /// <summary>
        /// Create a new instance of frmCustomer
        /// If a Rental Primary Key was provided
        /// </summary>
        /// <param name="rentalID"></param>
        public frmCustomers()
        {
            // Initialize the form user interface
            // Set the form as 'new'
            // Initialize form
            InitializeComponent();
            _isNew = true;
            InitializeForm();
        }


        /// <summary>
        /// Create a new instance of frmCustomer
        /// If a primary key was provided
        /// </summary>
        /// <param name="pkID"></param>
        public frmCustomers(long pkID)
        {
            // Initialize the form user interface
            // Set the pkID as a global variable
            // Initialize form
            InitializeComponent();
            _PKID = pkID;
            InitializeForm();
        }

        #endregion
        #region Form Events

        /// <summary>
        /// Initialize the form paint standards
        /// </summary>
        private void frmCustomers_Paint(object sender, PaintEventArgs e)
        {
            // Paint the form according to the default setting
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        #endregion
        #region Button Events

        /// <summary>
        /// Form close button
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            Close();
        }

        /// <summary>
        /// Initialize the Button to save data
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Commit and end the edit operation on the current cell using the default error context.
            _customerTable.Rows[0].EndEdit();

            // Call the save method of the Context Class to save the changes to the Database
            Context.SaveDatabaseTable(_customerTable);
        }

        #endregion
        #region HelperMethods

        /// <summary>
        /// Initialize the form with data access and binding controls
        /// </summary>
        private void InitializeForm()
        {
            InitializeDataTable();
            BindControls();
        }

        /// <summary>
        /// BindControls for the TextFields
        /// </summary>
        private void BindControls()
        {
            // Set the DataBinding for enteries
            txtCustomerID.DataBindings.Add("Text", _customerTable, "CustomerID");
            txtCustomerName.DataBindings.Add("Text", _customerTable, "CustomerName");
            txtCustomerPhone.DataBindings.Add("Text", _customerTable, "Phone");
            txtCustomerEmail.DataBindings.Add("Text", _customerTable, "Email");
        }

        /// <summary>
        /// Initialize the DataTable
        /// </summary>
        private void InitializeDataTable()
        {
            // Create sql string
            string sqlQuery = $"SELECT * FROM Customer WHERE CustomerID = {_PKID}";
            // Get acccess to the Customer Table
            _customerTable = Context.GetDataTable(sqlQuery, "Customer");

            // If the entry is new, then create a new row in the table
            if (_isNew)
            {
                // Generate a CustomerID +1 of total entries
                DataRow row = _customerTable.NewRow();
                row["CustomerID"] = GetCustomerCount();
                _customerTable.Rows.Add(row);
            }
        }
        /// <summary>
        /// Method to count the Customer table entries
        /// </summary>
        private int GetCustomerCount()
        {
            // Access the Customer table and store as a temp DataTable
            // Return the total rows +1 to resemble new entry ID
            DataTable tempTable = Context.GetDataTable("Customer");
            return tempTable.Rows.Count + 1;
        }

        #endregion

    }
}
