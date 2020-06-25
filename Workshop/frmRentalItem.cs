using Controller;
using System;
using System.Data;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmRentalItem : Form
    {
        #region Member Variables

        // Create a variable for the Primary Key
        long _PKID = 0;
        // Create a variable for the Rental Primary Key
        long _rentalID = 0;
        // Create a variable for the DataTable and Tool DataTable
        DataTable _rentalTable = null, _toolTable = null;
        // Create a variable for the New DataTable
        bool _isNew = false;

        #endregion
        #region Constructors

        /// <summary>
        /// Create a new instance of frmRentalItem
        /// If a primary key was provided
        /// </summary>
        /// <param name="pkID"></param>
        public frmRentalItem(long pkID)
        {
            // Initialize the form components
            // Assign the Primary Key to the Global Variable
            // Initialize the form
            InitializeComponent();
            _PKID = pkID;
            InitializeForm();
        }

        /// <summary>
        /// Create a new instance of frmRentalItem
        /// If a Rental Primary Key was provided
        /// </summary>
        /// <param name="rentalID"></param>
        public frmRentalItem(string rentalID)
        {
            // Initialize the form components
            // Assign true to _isNew
            // Assign the Rental Primary Key to the Global Variable
            // Initialize the form
            InitializeComponent();
            _isNew = true;
            _rentalID = long.Parse(rentalID);
            InitializeForm();
        }

        /// <summary>
        /// Initialize the form
        /// </summary>
        private void InitializeForm()
        {
            // Initialize the DataTable
            // Initialize the Movie DataTable
            // Bind data to the form components
            InitializeDatatable();
            InitializeToolTable();
            BindControls();
        }


        #endregion
        #region Form Events
        /// <summary>
        /// Initialize the form paint default settings
        /// </summary>
        private void frmRentalItem_Paint(object sender, PaintEventArgs e)
        {
            // Assign the form background colour with the ColorTheme from the project settings
            BackColor = Properties.Settings.Default.ColorTheme;
        }

        #endregion
        #region Button Events
        /// <summary>
        /// Initialize the form  close function
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close this form
            Close();
        }

        /// <summary>
        /// Initialize the form save function
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if a cell has been selected in the DataGridView
            // If not show a MessageBox and stop the method
            if (cboTool.SelectedIndex == -1)
            {
                MessageBox.Show("No tool has been selected!",
                    Properties.Settings.Default.ProjectName,
                    MessageBoxButtons.OK);

                return;
            }

            // End the edit operation on the current cell
            _rentalTable.Rows[0].EndEdit();

            // Save the table
            Context.SaveDatabaseTable(_rentalTable);
         // Extra Feature: Update the Tool Availability once a rental has been created with the Tool included
            // Store the Selected value to the ToolID
            // Update the ToolID 
            long toolID = long.Parse(cboTool.SelectedValue.ToString());
            updateStatus(toolID);
        }

        #endregion
        #region ComboBoxEvent

        /// <summary>
        /// Button event -Selected Index Changed- for the cboTool
        /// </summary>
        private void CboTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the Tools SelectedIndex is greater than 0
            // Assign the Tools SelectedValue to the DataTable ToolID
            if (cboTool.SelectedIndex >= 0)
            {
                _rentalTable.Rows[0]["ToolID"] = cboTool.SelectedValue;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Initialize the DataTable
        /// </summary>
        private void InitializeDatatable()
        {
            // Create and assign the SQL Query
            string sqlQuery = $"SELECT * FROM RentalItems WHERE RentalItemID = {_PKID}";

            // Assign the DataTable with the RentItem Datatable
            _rentalTable = Context.GetDataTable(sqlQuery, "RentalItems");

            // Check if _isNew is true
            if (_isNew)
            {
                // Create and assign a new DataRow
                // Assign the empty row to the DataTable
                DataRow row = _rentalTable.NewRow();
                _rentalTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Initialize the Tool DataTable
        /// </summary>
        private void InitializeToolTable()
        {
            // Create and assign a new SQl Query
            string sqlQuery = $"SELECT ToolID, ToolDescription FROM Tool";

            // Assign the tool DataTable with the tool DataTable
            // Create a new column with the ToolID and ToolDescription
            _toolTable = Context.GetDataTable(sqlQuery, "Tool");
            _toolTable.Columns.Add("Display", typeof(string), "ToolID + ' - ' + ToolDescription");
        }

        /// <summary>
        /// Bind data to the form components
        /// </summary>
        private void BindControls()
        {
            if (_rentalID == 0)
            {
                _rentalID = long.Parse(_rentalTable.Rows[0]["RentalID"].ToString());
            }
            else
            {
                _rentalTable.Rows[0]["RentalID"] = _rentalID;
            }
            // Create databdings for the cboTool & txtRentalID
            txtRentalID.DataBindings.Add("Text", _rentalTable, "RentalID");

            cboTool.ValueMember = "ToolID";
            cboTool.DisplayMember = "Display";
            cboTool.DataSource = _toolTable;
            cboTool.BindingContext = BindingContext;
            // If new form, set the cboTool to display nothing
            if (_isNew)
            {
                cboTool.SelectedIndex = -1;
            }
            else
            {
                cboTool.SelectedIndex = int.Parse(_rentalTable.Rows[0]["ToolID"].ToString()) - 1;
            }
        }

        private void updateStatus(long toolID)
        {
            // Create and assign a new SQL Query
            string sqlQuery = $"SELECT * FROM Tool WHERE ToolID={toolID}";
            // Get the temporary datatable from the query
            DataTable tempTable = Context.GetDataTable(sqlQuery, "Tool");

                tempTable.Rows[0]["Availability"] = "Checked-out";
            // Call the SaveDatabaseTable method
            Context.SaveDatabaseTable(tempTable);
        }

        #endregion
    }
}