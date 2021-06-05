using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsCampApplication
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (usernameTxtBox.Text.Equals("User name") || 
                usernameTxtBox.Text.Equals("") || 
                usernameTxtBox.Text.Equals(null) ||
                passwordTxtBox.Text.Equals("Password") ||
                passwordTxtBox.Text.Equals("") ||
                passwordTxtBox.Text.Equals(null))
            {
                String message = "Please enter the user name and password";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
            }
            else
            {
                if (usernameTxtBox.Text.Equals("admin") && passwordTxtBox.Text.Equals("admin"))
                {
                    this.Hide();
                    webCampingWindows ss = new webCampingWindows();
                    ss.Show();
                }
                else
                {
                    String message = "Please insert the right user name and password";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                }
            }
            
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            usernameTxtBox.Text = "User Name";
            passwordTxtBox.Text = "Password";
        }

        private void usernameTxtBox_MouseClick(object sender, MouseEventArgs e)
        {
            if(usernameTxtBox.Text.Equals("User Name"))
            {
                usernameTxtBox.Text = "";
            }
        }

        private void passwordTxtBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (passwordTxtBox.Text.Equals("Password"))
            {
                passwordTxtBox.Text = "";
            }
        }
    }
}
