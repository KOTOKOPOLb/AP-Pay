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
                //Main main = new Main();
                File.WriteAllText("sett", textBox1.Text + "\n" + textBox2.Text);
                //main.id = textBox1.Text;
                //main.token = textBox2.Text;
                Close();
            }
            else MessageBox.Show("Введите ID и Токен!");
        }

        private void button2_Click(object sender, EventArgs e) => MessageBox.Show("1) Заходите на нужный сервер в Minecraft\n2) Заходите на сайт SP Worlds\n3) Нажимаете на свой аватар справо сверху и ыбираете нужный сервер\n4) Переходите в 'Кошелёк', выбираете нужную карту, нажимаете 'Поделиться'\n5) Нажимаете 'Сгенерировать новый API токен' и подтверждаете\n6) Получаете в чате Minecraft ID и токен вашей карты");
    }
}
