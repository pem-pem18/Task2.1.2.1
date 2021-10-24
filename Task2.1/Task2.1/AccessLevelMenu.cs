using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task2._1
{
    public partial class AccessLevelMenu : Form
    {

        public AccessLevelMenu()
        {
            InitializeComponent();
        }

        // клик по кнопке ОК
        private void OkayButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // смена значения бокса -> смена уровня доступа
        private void AccessLevelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainMenu.AccessLevel = AccessLevelBox.Text;
        }
    }
}
