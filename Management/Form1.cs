

using Wen.BusinessLogicLayer;
using Wen.Models.Entity;

namespace Management
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var userRepo =new SysDictBll()._GetAll();
            dataGridView1.DataSource = userRepo;
        }
    }
}
