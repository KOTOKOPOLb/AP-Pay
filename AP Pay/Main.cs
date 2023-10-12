using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SP_tools
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);

        public enum DWMWINDOWATTRIBUTE : uint { DARK_MODE = 20 }

        public string id;
        public string token;
        public string theme;
        string btoken;
        private void button1_Click(object sender, EventArgs e) => pay();

        async Task balance()
        {
            btoken = Convert.ToBase64String(Encoding.UTF8.GetBytes(id + ":" + token));
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", btoken);
                toolStripStatusLabel1.Text = "Идёт запрос...";
                HttpResponseMessage response = await client.GetAsync("https://spworlds.ru/api/public/card");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    label4.Text = @$"Баланс: {jsonObject.balance} АР";
                    toolStripStatusLabel1.Text = "";
                }
                else
                {
                    if (response.StatusCode.ToString() == "Unauthorized")
                        toolStripStatusLabel1.Text = "Ошибка в ID или Токене карты";
                    else
                        toolStripStatusLabel1.Text = "Ошибка: " + response.StatusCode.ToString();
                }
            }
        }

        async Task pay()
        {
            btoken = Convert.ToBase64String(Encoding.UTF8.GetBytes(id + ":" + token));
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", btoken);
                StringContent content = new StringContent("{\"receiver\":" + " \"" + textBox1.Text + "\"," + " \"amount\": " + textBox2.Text + ", " + "\"comment\":" + " \"" + textBox3.Text + "\"}", Encoding.UTF8, "application/json");
                toolStripStatusLabel1.Text = "Идёт запрос...";
                HttpResponseMessage response = await client.PostAsync("https://spworlds.ru/api/public/transactions", content);
                if (response.IsSuccessStatusCode)
                    toolStripStatusLabel1.Text = "АРы были отправлены!";
                else
                {
                    if (response.StatusCode.ToString() == "Unauthorized")
                        toolStripStatusLabel1.Text = "Ошибка в ID или Токене карты";
                    else
                        toolStripStatusLabel1.Text = "Ошибка: " + response.StatusCode.ToString();
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ReadSett();
            if (id == null || token == null)
            {
                button2_Click(null, null);
            }
            else balance();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cards cards = new Cards();
            if (theme == "True")
                cards.checkBox1.Checked = true;
            else if (theme == "False")
                cards.checkBox1.Checked = false;
            cards.textBox1.Text = id;
            cards.textBox2.Text = token;
            cards.ShowDialog();
            id = cards.textBox1.Text;
            token = cards.textBox2.Text;
            if (cards.checkBox1.Checked)
            {
                SetTheme(true);
                theme = "True";
            }
            else
            {
                SetTheme(false);
                theme = "False";
            }
            balance();
        }

        void ReadSett()
        {
            if (File.Exists("sett"))
            {
                string filePath = "sett";
                string[] lines = File.ReadAllLines(filePath);
                try
                {
                    id = lines[0];
                    token = lines[1];
                    theme = lines[2];
                }
                catch (IndexOutOfRangeException) { }
                if (theme != "False" && theme != "True")
                    theme = "False";

                if (theme == "True")
                    SetTheme(true);
                else if (theme == "False")
                    SetTheme(false);
            }
        }

        Regex regex = new Regex(@"\D");
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int sele = textBox1.SelectionStart;
            int ll = textBox1.TextLength;
            textBox1.Text = regex.Replace(textBox1.Text, "");
            textBox1.SelectionStart = sele - (ll - textBox1.TextLength);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int sele = textBox2.SelectionStart;
            int ll = textBox2.TextLength;
            textBox2.Text = regex.Replace(textBox2.Text, "");
            textBox2.SelectionStart = sele - (ll - textBox1.TextLength);
        }

        private void button3_Click(object sender, EventArgs e) => balance();

        public void SetTheme(bool theme)
        {
            var preference = Convert.ToInt32(true);
            if (theme)
            {
                preference = Convert.ToInt32(true);
                DwmSetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DARK_MODE, ref preference, sizeof(uint));
                BackColor = Color.FromArgb(55, 57, 63);
                label1.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = textBox1.ForeColor = textBox2.ForeColor = textBox3.ForeColor = button1.ForeColor = button2.ForeColor = button3.ForeColor = statusStrip1.ForeColor = Color.White;
                button1.BackColor = button2.BackColor = button3.BackColor = Color.FromArgb(24, 25, 28);
                textBox1.BackColor = textBox2.BackColor = textBox3.BackColor = Color.FromArgb(65, 68, 75);
                button1.FlatStyle = button2.FlatStyle = button3.FlatStyle = FlatStyle.Popup;
                textBox1.BorderStyle = textBox2.BorderStyle = textBox3.BorderStyle = BorderStyle.FixedSingle;
                statusStrip1.BackColor = Color.FromArgb(47, 49, 54);
            }
            else
            {
                preference = Convert.ToInt32(false);
                DwmSetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DARK_MODE, ref preference, sizeof(uint));
                BackColor = statusStrip1.BackColor = Color.FromArgb(240, 240, 240);
                label1.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = textBox1.ForeColor = textBox2.ForeColor = textBox3.ForeColor = button1.ForeColor = button2.ForeColor = button3.ForeColor = statusStrip1.ForeColor = Color.Black;
                button1.BackColor = button2.BackColor = button3.BackColor = Color.FromArgb(253, 253, 253);
                textBox1.BackColor = textBox2.BackColor = textBox3.BackColor = Color.White;
                button1.FlatStyle = button2.FlatStyle = button3.FlatStyle = FlatStyle.Standard;
                textBox1.BorderStyle = textBox2.BorderStyle = textBox3.BorderStyle = BorderStyle.Fixed3D;
            }
        }
    }
}
