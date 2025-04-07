namespace LogsParser;

public partial class Form1 : Form
{
    private readonly Worker1 _worker = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox1.Text = openFileDialog1.FileName;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        if (openFileDialog2.ShowDialog() == DialogResult.OK) textBox2.Text = openFileDialog2.FileName;
    }

    private void button4_Click(object sender, EventArgs e)
    {
        if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            textBox3.Text = folderBrowserDialog1.SelectedPath + "\\output1.txt";
    }

    private async void button3_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
            string.IsNullOrWhiteSpace(textBox3.Text))
        {
            MessageBox.Show("Есть пустые поля.");
            return;
        }

        try
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 15;

            await Task.Run(() =>
            {
                var visits1 = _worker.GetGclidVisits(textBox1.Text, "i-master24.by");
                var visits2 = _worker.GetGclidVisits(textBox2.Text, "minskremont.by");
                var general = Worker1.GetGeneralVisits(visits1, visits2);
                _worker.WriteData(textBox3.Text, general);
            });

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            MessageBox.Show($"Файл {textBox3.Text} готов.");
        }
        catch
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            MessageBox.Show("Ошибка.");
        }
    }
}