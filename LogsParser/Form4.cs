using System.Net;

namespace LogsParser;

public partial class Form4 : Form
{
    private readonly Worker4 _worker = new();

    public Form4()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox1.Text = openFileDialog1.FileName;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            textBox3.Text = folderBrowserDialog1.SelectedPath + "\\output4.txt";
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
            string.IsNullOrWhiteSpace(textBox3.Text))
        {
            MessageBox.Show("Есть пустые поля.");
            return;
        }

        var ipString = textBox2.Text.Trim();
        if (!IPAddress.TryParse(ipString, out _))
        {
            MessageBox.Show($"Строка '{ipString}' не является валидным айпи-адресом.");
            return;
        }

        CancellationTokenSource cts = new();
        var token = cts.Token;
        try
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 15;

            await Task.Run(() =>
            {
                IEnumerable<GclidVisit> visitsByIp = Worker4.GetGclidVisitsByIp(textBox1.Text, ipString);
                GclidVisit[] visitsByIpArray = visitsByIp as GclidVisit[] ?? visitsByIp.ToArray();
                if (visitsByIpArray.Length == 0)
                {
                    progressBar1.Invoke(() =>
                    {
                        progressBar1.Style = ProgressBarStyle.Continuous;
                        progressBar1.MarqueeAnimationSpeed = 0;
                    });
                    MessageBox.Show($"Айпи {ipString} не найден в файле {textBox1.Text}.");
                    cts.Cancel();
                    token.ThrowIfCancellationRequested();
                }

                _worker.WriteData(textBox3.Text, visitsByIpArray);
            }, token);

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            MessageBox.Show($"Файл {textBox3.Text} готов.");
        }
        catch (OperationCanceledException)
        {
        }
        catch
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            MessageBox.Show("Ошибка.");
        }
        finally
        {
            cts.Dispose();
        }
    }
}