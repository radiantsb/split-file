using System.Security.Cryptography.X509Certificates;

namespace split_file
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            status.Text = "running";
            var filedialog = new OpenFileDialog();
            filedialog.ShowDialog();
            string filename = filedialog.FileName;
            int index = 0;
            Stream filestream = File.OpenRead(filename);
            double size = new FileInfo(filename).Length;
            size = Math.Ceiling(size / 1000000);
            while (index <= size)
            {
                byte[] buffer = new byte[long.Parse(textBox1.Text) * 1000000];
                await filestream.ReadAsync(buffer);
                await File.WriteAllBytesAsync($"splitfile-{index}.{Path.GetExtension(filename)}", buffer);
                index++;
            }
            status.Text = "done";

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status.Text = "running";
            var filedialog = new OpenFileDialog();
            filedialog.Multiselect = true;
            filedialog.ShowDialog();
            string[] filenames = filedialog.FileNames;
            string path = Path.GetExtension(filenames[0]);
            using (var stream = new FileStream($"combined.{path}", FileMode.Append))
            {
                for (int i = 0; i < filenames.Length; i++)
                {
                    byte[] buffer = File.ReadAllBytesAsync(filenames[i]).Result;
                    stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            status.Text = "done";
            
        }
    }
}
