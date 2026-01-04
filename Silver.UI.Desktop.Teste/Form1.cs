using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace Silver.UI.Desktop.Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Silver.AsteriskClient.AsteriskCommand cmd = new AsteriskClient.AsteriskCommand();
            
            cmd.RamaisStatus();
        }
    }
}
