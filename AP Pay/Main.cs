using Newtonsoft.Json;
using System.Net.Http.Headers;
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

        public string id;
        public string token;
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
                StringContent content = new StringContent("{\"receiver\":" + " \"" + textBox1.Text + "\"," + " \"amount\": " + textBox2.Text + ", " + "\"comment\":" + " \"" + textBox3.Text + "\",", Encoding.UTF8, "application/json");
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
            cards.textBox1.Text = id;
            cards.textBox2.Text = token;
            cards.ShowDialog();
            id = cards.textBox1.Text;
            token = cards.textBox2.Text;
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
                }
                catch (IndexOutOfRangeException) { }
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
    }
}