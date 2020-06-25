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
    public partial class frmSettings : Form
    {
        /// <summary>
        /// Initialize the settings form
        /// </summary>
        public frmSettings()
        {
            // Initialze the component
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the button click event
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }


        /// <summary>
        /// Initialize the form paint settings
        /// </summary>
        private void frmSettings_Paint(object sender, PaintEventArgs e)
        {
            // Set the form paint to the default selection
            this.BackColor = Properties.Settings.Default.ColorTheme;
        }

        /// <summary>
        /// Initialize the form color change click event
        /// </summary>
        private void btnChangeColour_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Change the property settings of the ColorTheme
                Properties.Settings.Default.ColorTheme = colorDialog.Color;
                Properties.Settings.Default.Save();
                // Read the new property settings of the ColorTheme
                this.BackColor = Properties.Settings.Default.ColorTheme;
            }
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {

        }
        private void btnChangeColour_Paint(object sender, PaintEventArgs e) {}
    }
}
