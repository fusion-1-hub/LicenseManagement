namespace DemoLicenseApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                var _status = Fusion1.Resman.Resman.Status.UNDEFINED;
                var _msg = "";
                var _lic = (Fusion1.Resman.Resource)Fusion1.Resman.Resman.ParseResourceString(
                    typeof(Fusion1.Resman.Resource),
                    File.ReadAllText("license.lic"),
                    out _status,
                    out _msg);
                toolStripStatusLabel1.Text = _status.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = Fusion1.Resman.Resman.Status.INVALID.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblUUID.Text = Fusion1.Resman.Resman.GenerateUID("DemoWinFormApp");
        }
    }
}