using static SP_tools.Main;

namespace SP_tools
{
    public partial class Cards : Form
    {
        public Cards()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                File.WriteAllText("sett", textBox1.Text + "\n" + textBox2.Text + "\n" + checkBox1.Checked.ToString());
                Close();
            }
            else MessageBox.Show("Введите ID и Токен!");
        }

        private void button2_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("explorer", "https://github.com/KOTOKOPOLb/SP-Tools/wiki/");

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var preference = Convert.ToInt32(true);
            if (checkBox1.Checked)
            {
                preference = Convert.ToInt32(true);
                DwmSetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DARK_MODE, ref preference, sizeof(uint));
                BackColor = Color.FromArgb(55, 57, 63);
                label1.ForeColor = label2.ForeColor = checkBox1.ForeColor = button1.ForeColor = button2.ForeColor = textBox1.ForeColor = textBox2.ForeColor = Color.White;
                button1.BackColor = button2.BackColor = Color.FromArgb(24, 25, 28);
                textBox1.BackColor = textBox2.BackColor = Color.FromArgb(65, 68, 75);
                button1.FlatStyle = button2.FlatStyle = FlatStyle.Popup;
                textBox1.BorderStyle = textBox2.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                preference = Convert.ToInt32(false);
                DwmSetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DARK_MODE, ref preference, sizeof(uint));
                BackColor = Color.FromArgb(240, 240, 240);
                label1.ForeColor = label2.ForeColor = checkBox1.ForeColor = button1.ForeColor = button2.ForeColor = textBox1.ForeColor = textBox2.ForeColor = Color.Black;
                button1.BackColor = button2.BackColor = Color.FromArgb(253, 253, 253);
                textBox1.BackColor = textBox2.BackColor = Color.White;
                button1.FlatStyle = button2.FlatStyle = FlatStyle.Standard;
                textBox1.BorderStyle = textBox2.BorderStyle = BorderStyle.Fixed3D;
            }
        }
    }
}
