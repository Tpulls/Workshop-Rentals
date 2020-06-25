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
using NLog;
using System.Diagnostics;

namespace Workshop
{
    public partial class frmRentals : Form
    {

        #region Member Variables
        // Create a variable for the PK
        long _PKID = 0;
        // Create a variable for the RentalTable and RentalItemsTable
        DataTable _rentalTable = null, _rentalItemsTable = null;
        // Set a variable to manage a new / existing item
        bool _isNew = false;
        // Set a variable to create a custom format for the DateTimePicker
        bool customFormat = false;
        // Initialize a logger variable
        private Logger _log;

        #endregion
        #region Constructors

        /// <summary>
        /// Initialize a new form Rental
        /// </summary>
        public frmRentals()
        {
            // Initialize the component
            // Initialize the new Rental
            InitializeComponent();
            InitializeNewRental();
        }
        /// <summary>
        /// Initialize a new form Rental from the associated PKID
        /// </summary>
        public frmRentals(long pkID)
        {
            // Initialize Component
            // Initialize the data from the existing PKID
            InitializeComponent();
            InitializeExistingRental(pkID);
        }

        #endregion
        #region Form Events
        /// <summary>
        /// Initialize a paint settings 
        /// </summary>
        private void frmRentals_Paint(object sender, PaintEventArgs e)
        {
            // Set the form paint to the default settings
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        /// <summary>
        /// Initialize a new form Rental from the associated PKID
        /// </summary>
        private void frmRentals_Load(object sender, EventArgs e)
        {
            // Populate Combo Boxes
            // Add bind controls to the editable fields
            PopulateComboBoxCustomer();
            PopulateComboBoxWorkspace();
            BindControls();
        }

        #endregion
        #region Button Events
        /// <summary>
        /// Initialize the button close event
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        /// <summary>
        /// Initialize the button save event
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Create a message when there is no data

            if (dgvRentalItems.Rows.Count == 0)
            {
                MessageBox.Show("There is no information to save.", Properties.Settings.Default.ProjectName, MessageBoxButtons.OK);
            }

            else if (dtpDateReturned.Text.Equals(" ") == false)
            { 
                _rentalTable.Rows[0]["DateReturned"] = dtpDateReturned.Value.ToString("yyyy-MM-dd");

            // Always do an EndEdit before saving, otherwise the data will not persis in the database
            _rentalTable.Rows[0].EndEdit();
            Context.SaveDatabaseTable(_rentalTable);
            }
        }
        /// <summary>
        /// Initialize the button insert event
        /// </summary>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Open the RentalItem form
            // Populate grid to capture changes
            frmRentalItem frm = new frmRentalItem(txtRentalID.Text);
            if (frm.ShowDialog() == DialogResult.OK)
                PopulateGrid();
        }
        /// <summary>
        /// Initialize the button Create Event
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {

            // Before we create a child record, we will force our program to create the parent record based on the selection the user made in the customer 
            // ComboBox and DateRented DateTimePicker
            if (_isNew && _PKID <= 0)
            {
                if (cboCustomer.SelectedIndex == -1)
                {
                    MessageBox.Show("No Customer has been selected!", Properties.Settings.Default.ProjectName, MessageBoxButtons.OK);
                    return;
                }
                else if (cboWorkspace.SelectedIndex == -1)
                {
                    MessageBox.Show("No Workspace has been selected!", Properties.Settings.Default.ProjectName, MessageBoxButtons.OK);
                    return;
                }
                string columnNames = "CustomerID, WorkspaceID, DateRented, DateReturned";

                // When sending date in SQL, we will use a string using the format of 'yyyy-MM-dd'
                string dateRented = dtpDateRented.Value.ToString("yyyy-MM-dd");
                long customerID = long.Parse(cboCustomer.SelectedValue.ToString());
                long workspaceID = long.Parse(cboWorkspace.SelectedValue.ToString());
                Debugger.Break();
                string columnValues = $"{customerID}, {workspaceID}, '{dateRented}', null";
                Debug.Print(columnValues);

                // Push the parent data using the InsertParentTable of the Context Class. It will then
                // return the primary key id of the newly created parent record and we simply store it in the _PKID variable

                _PKID = Context.InsertParentTable("Rental", columnNames, columnValues);

                Debug.Assert(_PKID > 0);
                // Display the _PKID value in the txtRentalID textbox
                txtRentalID.Text = _PKID.ToString();
                // Call the initializeDataTable method again to refresh it using the enwly created parent record from the database
                InitializeDataTable();
                gbxRentalItems.Enabled = true;
            }
        }
        /// <summary>
        /// Initialize the button Delete Event
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if the user wants to delete the item
            DialogResult msgBox = MessageBox.Show("Do you want to delete this item?",
                Properties.Settings.Default.ProjectName, MessageBoxButtons.YesNo);
            if (msgBox == DialogResult.Yes)
            {
                // Try to delete the record
                // Catch the errors
                try
                {
                    // Create and assign the DataGridView Primary Key
                    long pkID = long.Parse(dgvRentalItems[0, dgvRentalItems.CurrentCell.RowIndex].Value.ToString());

                    // Delete the record
                    // Populate the DataGridView
                    Context.DeleteRecord("RentalItems", "RentalItemID", pkID.ToString());
                    PopulateGrid();
                }
                catch (Exception)
                {
                    // Show a messagebox
                    MessageBox.Show("Can't find record. No records affected",
                        Properties.Settings.Default.ProjectName,
                        MessageBoxButtons.OK);
                    _log.Error(e.ToString());
                }
            }
        }
        #endregion
        #region ComboBox Events
        /// <summary>
        /// Initialize the ComboBox Customer
        /// </summary>
        private void PopulateComboBoxCustomer()
        {
            // Get all the records from the source database table - Customer
            DataTable table = Context.GetDataTable("Customer");

            // Set the records from the PK refernce
            cboCustomer.ValueMember = "CustomerID";

            // The value to be displayed in the cboCustomer
            cboCustomer.DisplayMember = "CustomerName";

            // Setting the datasource
            cboCustomer.DataSource = table;
        }

        /// <summary>
        /// Initialize the ComboBox Workspace
        /// </summary>
        private void PopulateComboBoxWorkspace()
        {
            // Get all the records from the source database table - Workspace
            DataTable table = Context.GetDataTable("Workspace");

            // Set the records from the PK refernce
            cboWorkspace.ValueMember = "WorkspaceID";

            // The value to be displayed in the cboWorkspace
            cboWorkspace.DisplayMember = "WorkspaceName";

            // Setting the datasource
            cboWorkspace.DataSource = table;
        }

        #endregion
        #region Helper_Methods
        /// <summary>
        /// Initialize the New Rental
        /// </summary>
        private void InitializeNewRental()
        {
            // Set the variable to true
            // Initialize the dataTable
            // Disable the GBX as there is no data
            _isNew = true;
            InitializeDataTable();
            gbxRentalItems.Enabled = false;
        }

        /// <summary>
        /// Initialize the DataTable
        /// </summary>
        private void InitializeDataTable()
        {
            // Access the DataTable and store in a variable
            // Populate the Grid
            _rentalTable = Context.GetDataTable($"SELECT * FROM Rental where RentalID = {_PKID}", "Rental");
            PopulateGrid();
        }

        /// <summary>
        /// Initialize the Existing Rental
        /// </summary>
        private void InitializeExistingRental(long pkID)
        {
            // Set the variable to global
            // Initialize the Data Table
            // Enable the group box
            _PKID = pkID;
            InitializeDataTable();
            gbxRentalItems.Enabled = true;
        }

        /// <summary>
        /// Initialize the Populate Grid
        /// </summary>
        private void PopulateGrid()
        {
            // Create a sql string query
            // Call the data table and assign to a variable
            // Set the datasource of the Group box
            string sqlQuery = "SELECT RentalItems.RentalItemID, Rental.RentalID, Tool.ToolDescription " +
                "FROM RentalItems INNER JOIN " +
                "Rental ON RentalItems.RentalID = Rental.RentalID INNER JOIN " +
                "Tool ON RentalItems.ToolID = Tool.ToolID " +
                $"WHERE Rental.RentalID = {_PKID}";
            _rentalItemsTable = Context.GetDataTable(sqlQuery, "RentalItems");
            dgvRentalItems.DataSource = _rentalItemsTable;
        }

        /// <summary>
        /// Initialize the Bind Controls 
        /// </summary>
        private void BindControls()
        {
            // Bind the data entered into the fields to a database 
            txtRentalID.DataBindings.Add("Text", _rentalTable, "RentalID");
            cboCustomer.DataBindings.Add("SelectedValue", _rentalTable, "CustomerID");
            dtpDateRented.DataBindings.Add("Text", _rentalTable, "DateRented");
            dtpDateReturned.DataBindings.Add("Text", _rentalTable, "DateReturned");
            // Set the cbo to display not selected index
            if (_isNew)
                cboCustomer.SelectedIndex = -1;
            // Set the cbo to display not selected index
            if (_isNew)
                cboWorkspace.SelectedIndex = -1;
            // Set the DateTimePicker to display a custom format
            if (_isNew || string.IsNullOrEmpty(_rentalTable.Rows[0]["DateReturned"].ToString()))
            {
                dtpDateReturned.Format = DateTimePickerFormat.Custom;
                dtpDateReturned.CustomFormat = " ";
                dtpDateReturned.Value = DateTime.Now.AddDays(1);
            }
        }

        /// <summary>
        /// Initialize the Date Returned ValueChanged event
        /// </summary>
        private void dtpDateReturned_ValueChanged(object sender, EventArgs e)
        {
            if (customFormat == true)
            {
                dtpDateReturned.Format = DateTimePickerFormat.Custom;
                dtpDateReturned.CustomFormat = "dd-MMM-yyyy";
            }
            else if (dtpDateReturned.CustomFormat == " ")
            {
                customFormat = true;
            }
        }

        #endregion




        private void label1_Click(object sender, EventArgs e)
        {

        }




    }
}
