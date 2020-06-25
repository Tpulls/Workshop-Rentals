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
    public partial class frmRentalList : Form
    {

        #region Member Variables
        //Initialize a logger variable
        private Logger _log;

        #endregion
        #region Constructors
        /// <summary>
        /// Initialize the Form
        /// </summary>
        public frmRentalList()
        {
            // Initialize the form components
            // Initialize the method to populate the grid
            InitializeComponent();
            PopulateGrid();
        }


        #endregion
        #region Form Events
        /// <summary>
        /// Initialize the form paint setting
        /// </summary>
        private void frmRentalList_Paint(object sender, PaintEventArgs e)
        {
            // Color the form according to the selected default setting
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        #endregion
        #region Button Events
        /// <summary>
        /// Initialize the form close button
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected item?", Properties.Settings.Default.ProjectName, MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                try
                {
                    // Determine the row index and stored as PKID
                    long PKID = long.Parse(dgvRentalList[0, dgvRentalList.CurrentCell.RowIndex].Value.ToString());

                    // Use the Delete Record method of the Context Class and pass the primary key value to delete
                    Context.DeleteRecord("Rental", "RentalID", PKID.ToString());
                    PopulateGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No Records exists.", Properties.Settings.Default.ProjectName);
                    _log.Error(e.ToString());
                }
            }
        }

        #endregion

        #region Helper_Methods
        /// <summary>
        /// Initialize the Populate Grid for the DGV
        /// </summary>
        private void PopulateGrid()
        {
            // Create the sql Query
            // Access the dataTable
            // Assign the dgv source to the dataTable
            string sqlQuery = "SELECT Rental.RentalID, Customer.CustomerName, Workspace.WorkspaceName, Rental.DateRented, Rental.DateReturned " +
                 "FROM Rental INNER JOIN " +
                 "Customer ON Rental.CustomerID = Customer.CustomerID INNER JOIN " +
                 "Workspace ON Rental.WorkspaceID = Workspace.WorkspaceID";
            DataTable table = Context.GetDataTable(sqlQuery, "Rental");
            dgvRentalList.DataSource = table;
        }

        #endregion
        /// <summary>
        /// Initialize the Populate Grid for the DGV
        /// </summary>
        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the form Rentals
            // Populate the grid to capture changes
            frmRentals frm = new frmRentals();
            if (frm.ShowDialog() == DialogResult.OK)
                PopulateGrid();
        }

        /// <summary>
        /// Initialize the double click event
        /// </summary>
        private void dgvRentalList_DoubleClick(object sender, EventArgs e)
        {
            // If the table is doubled clicked, populate the form according to the index Primary Key value
            // If cell is empty, return nothing
            if (dgvRentalList.CurrentCell == null) return;
            // Take the cell index and primary key
            long PKID = long.Parse(dgvRentalList[0, dgvRentalList.CurrentCell.RowIndex].Value.ToString());
            // Open the form accoridng to the PKID
            frmRentals frm = new frmRentals(PKID);
            if (frm.ShowDialog() == DialogResult.OK)
                PopulateGrid();
        }
    }
}
