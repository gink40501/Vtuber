using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vtuber
{
    public partial class input_html : Form
    {
        string html;
        public input_html()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            html =textBox1.Text;
            html = "\n"+html ;
            Vtuber_直播 vtuber = new Vtuber_直播(html);
            this.Close();
        }
    }
}
