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
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            // Initialize the form to access the user interface objects
            InitializeComponent();
        }


        private void btnRentals_Click(object sender, EventArgs e)
        {
            // Open the Rental List Form
            frmRentalList frm = new frmRentalList();
            frm.ShowDialog();
        }

        private void btnTools_Click(object sender, EventArgs e)
        {
            // Open the Tool List Form
            frmToolsList frm = new frmToolsList();
            frm.ShowDialog();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            // Open the Customer List Form
            frmCustomerList frm = new frmCustomerList();
            frm.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            // Open the Settings Form
            frmSettings frm = new frmSettings();
            frm.ShowDialog();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            // Open the Reports Form
            frmReport frm = new frmReport();
            frm.ShowDialog();
        }

        private void frmMenu_Paint(object sender, PaintEventArgs e)
        {
            // Paint the form according to the default selected 
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }


        private void label1_Click(object sender, EventArgs e) { }
    }
}
