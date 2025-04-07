namespace LogsParser;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private static void OpenForm<TForm>() where TForm : Form, new()
    {
        var form = Application.OpenForms[typeof(TForm).Name];
        form ??= new TForm();
        form.Show();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        OpenForm<Form1>();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        OpenForm<Form2>();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        OpenForm<Form3>();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        OpenForm<Form4>();
    }
}