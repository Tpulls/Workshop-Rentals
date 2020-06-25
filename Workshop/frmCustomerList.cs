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

namespace Workshop
{
    public partial class frmCustomerList : Form
    {

        #region Member Variables
        // Initialize a logger variable
        private Logger _log;
        #endregion


        #region Constructors
        /// <summary>
        /// Create a new instance of frmCustomerList
        /// </summary>
        public frmCustomerList()
        {
            // Call the InitializeComponent to access user interface objects
            InitializeComponent();
            // Call the Populate Grid
            PopulateGrid();
        }

        #endregion
        #region Form Events

        private void frmCustomerList_Paint(object sender, PaintEventArgs e)
        {
            // Paint the form according to the selected default
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        #endregion
        #region Button Events

        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open form Customers
            frmCustomers frm = new frmCustomers();
            if (frm.ShowDialog() == DialogResult.OK) PopulateGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Create a MessageBox
            if (MessageBox.Show("Are you sure you want to delete the selected item?", Properties.Settings.Default.ProjectName, MessageBoxButtons.YesNo)
                == DialogResult.Yes)
                // Try to delete the selected record
                // Catch the error 
                try
                {
                    // Store the PKID of the selected cell
                    long PKID = long.Parse(dgvCustomer[0, dgvCustomer.CurrentCell.RowIndex].Value.ToString());
                    // Initialize the Delete record method 
                    Context.DeleteRecord("Customer", "CustomerID", PKID.ToString());
                    // Repopulate the grid following the action
                    PopulateGrid();
                }
                catch (Exception ex)
                {
                    // Throw an error if there is no selected item
                    MessageBox.Show("No Record Exists.", Properties.Settings.Default.ProjectName);
                    _log.Error(e.ToString());
                }


        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            // If no row is selected, return
            if (dgvCustomer.CurrentCell == null) return;
            // Store the PK of the selected row
            long pkid = long.Parse(dgvCustomer[0, dgvCustomer.CurrentCell.RowIndex].Value.ToString());
            // Open the Customers form
            frmCustomers frm = new frmCustomers(pkid);
            if (frm.ShowDialog() == DialogResult.OK) PopulateGrid();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        #endregion
        #region Helper_Methods

        private void PopulateGrid()
        {
            // Instantiate a new Datatable
            DataTable table = new DataTable();
            // Access the Customer Table
            table = Context.GetDataTable("Customer");
            // Set the dgvCustomer to the Customer DataTable
            dgvCustomer.DataSource = table;

        }

        #endregion

        private void btnMovieList_Click(object sender, EventArgs e)
        {

        }




    }
}
