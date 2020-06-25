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
    public partial class frmTools : Form
    {

        #region Member Variables

        // Declare the member variable
        // Create a variable PKID
        // Create a DataTable variable
        // Create a new form variable
        long _PKID = 0;
        DataTable _ToolTable = null;
        bool _isNew = false;


        #endregion
        #region Constructors

        /// <summary>
        /// Initialize the new form Tools
        /// </summary>
        public frmTools()
        {
            // Initialize the Component
            // Set the form to new
            // Initialize the form setup
            InitializeComponent();
            _isNew = true;
            setupForm();
        }

        /// <summary>
        /// Initialize the new form Tools from an existing data parameter
        /// </summary>
        public frmTools(long pkID)
        {
            // Initialize the component
            // Setthe PKID to a global variable
            // Initialize the form setup
            InitializeComponent();
            _PKID = pkID;
            setupForm();
        }


        #endregion
        #region Form Events

        /// <summary>
        /// Define the form paint conditions
        /// </summary>
        private void frmTools_Paint(object sender, PaintEventArgs e)
        {
            // Form painting conditions to suit default paint color
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        #endregion
        #region Button events
        /// <summary>
        /// Define the form button close event
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }
        /// <summary>
        /// Define the form button save event
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // End edit to close editing
            _ToolTable.Rows[0].EndEdit();
            // Call the save method of the context class to save the changes
            Context.SaveDatabaseTable(_ToolTable);
        }

        #endregion
        #region Helper_Methods
        /// <summary>
        /// Initialize the form setup
        /// </summary>
        private void setupForm()
        {
            // Initialize the DataTable
            // Initialize the Combobox
            // Initialize the form Bind controls
            initializeDataTable();
            PopulateComboBox();
            BindControls();
        }
        /// <summary>
        /// Define the initialzieDataTable
        /// </summary>
        private void initializeDataTable()
        {
            // The method to access the data Table 'Tools' to collect the ToolID reference as PK
            string sqlQuery = $"SELECT * FROM Tool WHERE ToolID = {_PKID}";

            // Get the existing tool record based on the _PKID
            _ToolTable = Context.GetDataTable(sqlQuery, "Tool");

            // If the entry is new, then create a new row in the table
            if (_isNew)
            {
                DataRow row = _ToolTable.NewRow();
                row["ToolID"] = GetToolCount();
                _ToolTable.Rows.Add(row);
            }
        }
        /// <summary>
        /// Define the GetToolCount
        /// </summary>
        private int GetToolCount()
        {
            // Access the Tool DataTable
            // Count the total entries and add an index value
            DataTable tempTable = Context.GetDataTable("Tool");
            return tempTable.Rows.Count + 1;
        }
        /// <summary>
        /// Define the form Bind Controls
        /// </summary>
        private void BindControls()
        {
            // To store the entered data in the form text fields to the database
            txtToolID.DataBindings.Add("Text", _ToolTable, "ToolID");
            txtDescription.DataBindings.Add("Text", _ToolTable, "ToolDescription");
            txtBrand.DataBindings.Add("Text", _ToolTable, "Brand");
            txtAssetNumber.DataBindings.Add("Text", _ToolTable, "AssetNumber");
            cboStatus.DataBindings.Add("Text", _ToolTable, "Status");
            txtComments.DataBindings.Add("Text", _ToolTable, "Comments");
            // If the form is new, set the cbo to display nothing
            if(_isNew)
            {
                cboStatus.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Define the options in the ComboBox
        /// </summary>
        private void PopulateComboBox()
        {
            // Add the text options
            cboStatus.Items.Add("Active");
            cboStatus.Items.Add("Checked Out");
            cboStatus.Items.Add("In-Service");
            cboStatus.Items.Add("Retired");
        }

        #endregion

        private void btnMovieList_Click(object sender, EventArgs e)
        {

        }

        private void txtMovieid_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void frmTools_Load(object sender, EventArgs e)
        {

        }


        private void txtMovieName_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
