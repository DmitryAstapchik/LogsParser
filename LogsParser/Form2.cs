namespace LogsParser;

public partial class Form2 : Form
{
    private readonly Worker2 _worker = new();

    public Form2()
    {
        InitializeComponent();
        comboBox1.SelectedIndex = 0;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox1.Text = openFileDialog1.FileName;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            textBox2.Text = folderBrowserDialog1.SelectedPath + "\\output2.txt";
    }

    private async void button3_ClickAsync(object sender, EventArgs e)
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
            var selectedIndex = comboBox1.SelectedIndex;
            await Task.Run(() =>
            {
                var visits = _worker.GetGclidVisits(textBox1.Text, null);
                var repetitiveVisits = visits.GroupBy(v => v.Ip).Where(g => g.Count() > 1).SelectMany(g => g);
                switch (selectedIndex)
                {
                    case 0:
                        _worker.WriteData(textBox2.Text, repetitiveVisits);
                        break;
                    case 1:
                        Worker2.WriteDataGroupedByDate(textBox2.Text, repetitiveVisits);
                        break;
                }
            });
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            MessageBox.Show($"Файл {textBox2.Text} готов.");
        }
        catch
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            MessageBox.Show("Ошибка.");
        }
    }
}