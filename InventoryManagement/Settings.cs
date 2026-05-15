using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement
{
    public partial class Settings : Form
    {


        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.timeInterval.ToString();
            textBox2.Text = Properties.Settings.Default.minQuantity.ToString();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.Parse(textBox1.Text) <= 0 || int.Parse(textBox2.Text) <= 0)
            {
                MessageBox.Show("Интервал и кол-во должны быть больше 0!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Properties.Settings.Default.timeInterval = int.Parse(textBox1.Text);
            Properties.Settings.Default.minQuantity = int.Parse(textBox2.Text);
            Properties.Settings.Default.Save();

            var main = Application.OpenForms.OfType<InventoryForm>().FirstOrDefault();
            if (main != null)
            {
                main.RefreshSettings();
            }

            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            e.Handled = true;
        }
    }
}
