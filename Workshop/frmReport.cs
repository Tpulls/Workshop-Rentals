using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmReport : Form
    {


        #region Member Variables

        DataView _dvHistory = null; // Data source of our grid. Easier to filter than the DataTable. 

        #endregion
        #region Constructors

        /// <summary>
        /// Initialize a new form Report
        /// </summary>
        public frmReport()
        {
            // Initialize the Component
            // Initialize the PopulateGrid
            // Set the cbo display index to 'Default'
            InitializeComponent();
            PopulateGrid();
            cboReports.SelectedIndex = 0;
        }

        #endregion
        #region Form Events
        /// <summary>
        /// Initialize a form paint standards
        /// </summary>
        private void frmReport_Paint(object sender, PaintEventArgs e)
        {
            // Paint the form with the default settings
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        #endregion
        #region Button Events
        /// <summary>
        /// Initialize a form close
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            Close();
        }
        /// <summary>
        /// Initialize a form export button
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            // Create and assign a new StringBuilder
            StringBuilder csv = new StringBuilder();

            // Loop through the DataGridView rows
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                // Keep track of the row count 
                int rowCount = 1;
                // Loop through all the cells in that row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Append the cell to the file with a "," ("," is not added if it is the last cell)
                    csv.Append(cell.Value + (rowCount == row.Cells.Count ? "" : ","));
                    // increment the row count
                    rowCount++;
                }
                // append the new line
                csv.AppendLine();
            }
            // Write the StringBuilder to the ToolRental csv
            // Show a messageBox
            File.WriteAllText(Application.StartupPath + $@"\ToolRentalHistory-{cboReports.SelectedItem.ToString()}--{DateTime.Now.ToString("dd-MM-yyyy")}.csv", csv.ToString());
            MessageBox.Show($"{cboReports.SelectedItem.ToString()} exported to CSV as \"ToolRentalsHistory{cboReports.Text}.csv\"",
                Properties.Settings.Default.ProjectName);
        }

        #endregion
        #region Helper Methods
        /// <summary>
        /// Initialize the Populate Grid
        /// </summary>
        private void PopulateGrid()
        {
            // Create and assign a new SQL Query
            string sqlQuery =
            "SELECT Rental.DateRented, Customer.CustomerName, Tool.ToolDescription, Workspace.WorkspaceName, Rental.DateReturned " +
            "FROM Workspace CROSS JOIN " +
            "Rental CROSS JOIN Customer CROSS JOIN Tool " +
            "ORDER BY Rental.DateRented DESC";

            // Create and assign the DataTable with the ToolHistory DataTable
            // Assign the DataTable to the DataView
            // Assign the DataGridView with the DataView
            DataTable dtable = Context.GetDataTable(sqlQuery, "ToolHistory", true);
            _dvHistory = new DataView(dtable);
            dgvReport.DataSource = _dvHistory;
        }

        /// <summary>
        /// Initialize the txtSearch changed event
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // The '%' is a wild card. It means tha whatever is entered into txtSearch, we will search if CustomerName or 
            // ToolDescription containts the text entered.
            _dvHistory.RowFilter = $"CustomerName LIKE '%{txtSearch.Text}%' " + $"OR ToolDescription LIKE '%{txtSearch.Text}%' ";
        }
        #endregion

        #region ComboBox Events

        /// <summary>
        /// Initialize the cboReports SelectedIndex Changed event
        /// </summary>
        private void cboReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Declare the variables
            // Use switch case to assign the different reports to the corresponding cboReports
            string sqlQuery = null;
            DataTable tempTable = null;

            switch (cboReports.SelectedIndex)
            {
                case 0:
                    PopulateGrid();
                    txtSearch.Enabled = true;
                    return;
                case 1:
                    // Check if checked out tools
                    // Set the query
                    sqlQuery =
                        $"SELECT Tool.ToolID, Tool.ToolDescription, Tool.Brand, Rental.DateRented FROM Tool " +
                        $"LEFT JOIN RentalItems ON Tool.ToolID = RentalItems.ToolID " +
                        $"LEFT JOIN Rental ON RentalItems.RentalID = Rental.RentalID";
                    tempTable = Context.GetDataTable(sqlQuery, "Tool");
                    txtSearch.Enabled = false;
                    break;
                case 2:
                    // Check if tool is active 
                    // Set the query
                    sqlQuery =
                        $"SELECT ToolID, ToolDescription, Brand, Status FROM Tool " +
                        $"WHERE Availability = 'Available'";
                    tempTable = Context.GetDataTable(sqlQuery, "Tool");
                    txtSearch.Enabled = false;
                    break;
                case 3:
                    // Check if tool is active by brand
                    // Set the query
                    sqlQuery =
                        $"SELECT ToolID, ToolDescription, Brand, Status FROM Tool " +
                        $"WHERE Availability = 'Available' ORDER BY Brand";
                    tempTable = Context.GetDataTable(sqlQuery, "Tool");
                    txtSearch.Enabled = false;
                    break;
                case 4:
                    // Check if tool is retired
                    // Set the query
                    sqlQuery =
                        $"SELECT ToolID, ToolDescription, Brand, Status FROM Tool " +
                        $"WHERE Availability = 'Retired'";
                    tempTable = Context.GetDataTable(sqlQuery, "Tool");
                    txtSearch.Enabled = false;
                    break;
                case 5:
                    // Check if tool is retired
                    // Set the query
                    sqlQuery =
                        $"SELECT ToolID, ToolDescription, Brand, Status FROM Tool " +
                        $"WHERE Availability = 'Retired' ORDER BY Brand";
                    tempTable = Context.GetDataTable(sqlQuery, "Tool");
                    txtSearch.Enabled = false;
                    break;
            }
            dgvReport.DataSource = tempTable;
        }

        #endregion

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmReport_Load(object sender, EventArgs e)
        {

        }


    }
}
