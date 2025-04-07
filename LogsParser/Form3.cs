namespace LogsParser;

public partial class Form3 : Form
{
    private readonly Worker3 _worker = new();

    public Form3()
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
            textBox3.Text = folderBrowserDialog1.SelectedPath + "\\output3.txt";
    }

    private async void button3_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
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
                var visits1 = _worker.GetGclidVisits(textBox1.Text, GclidVisit.Imaster24By);
                var visits2 = _worker.GetGclidVisits(textBox2.Text, GclidVisit.MinskremontBy);
                var sameDayVisits = Worker3.GetSameDayVisits(visits1, visits2);
                _worker.WriteData(textBox3.Text, sameDayVisits);
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