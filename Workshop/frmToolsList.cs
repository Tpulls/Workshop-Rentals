using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using NLog;

namespace Workshop
{
    public partial class frmToolsList : Form
    {

        #region Member Variables
        // Initalize the logger variable
        private Logger _log;

        #endregion
        #region Constructors
        // Initialize the Form Tool List
        public frmToolsList()
        {
            // Initialize the Component
            InitializeComponent();
        }

        #endregion
        #region Form Events
        /// <summary>
        /// Define the Form Paint conditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmToolsList_Paint(object sender, PaintEventArgs e)
        {
            // set the form paint settings to the default selection
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }
        /// <summary>
        /// Define the Form load event
        /// </summary>
        private void frmToolsList_Load(object sender, EventArgs e)
        {
            // Initialize the PopulateGrid
            PopulateGrid();
        }

        #endregion
        #region Button Events
        /// <summary>
        /// Define the Form link event
        /// </summary>
        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open form Tools
            // Populate grid 
            frmTools frm = new frmTools();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                PopulateGrid();
            }
        }
        /// <summary>
        /// Define the Form button close event
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }


        #endregion

        #region Helper_Methods

        #endregion

        /// <summary>
        /// Define the dgv populate grid
        /// </summary>
        private void PopulateGrid()
        {
            // Create a sql prompt
            // Access the Tool DataTable
            // Set teh DGV to the DataTable source
            string sqlQuery = "SELECT ToolID, ToolDescription, Brand, AssetNumber, Status, Availability, Comments FROM Tool";
            DataTable table = Context.GetDataTable(sqlQuery, "Tool");
            dgvTools.DataSource = table;
        }

        /// <summary>
        /// Define the form button delete event
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Create a confirmation message box
            // Try to take the selected index, delete it and re-populate the grid
            // Catch if fails
            if (MessageBox.Show("Are you sure you want to delete the selected item?", Properties.Settings.Default.ProjectName,
            MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    long PKID = long.Parse(dgvTools[0, dgvTools.CurrentCell.RowIndex].Value.ToString());

                    // Use the DeleteRecord method of the Context class and pass the primary key value to delete

                    Context.DeleteRecord("Tool", "ToolID", PKID.ToString());
                    PopulateGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No Records exists.", Properties.Settings.Default.ProjectName);
                    _log.Error(e.ToString());
                }

            }
        }
        /// <summary>
        /// Define the DGV double click event
        /// </summary>
        private void dgvTools_DoubleClick(object sender, EventArgs e)
        {
            // If there is no cell selected, do nothing
            if (dgvTools.CurrentCell == null) return;

            // Get the primary key of the selected row, which is in column 0
            long pkID = long.Parse(dgvTools[0, dgvTools.CurrentCell.RowIndex].Value.ToString());

            frmTools frm = new frmTools(pkID);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                PopulateGrid();
            }
        }
    }
}
