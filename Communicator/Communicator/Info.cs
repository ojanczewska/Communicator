using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Communicator
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            label1.Text = "Aplikacja umożliwia komunikacje między dwoma komputerami.\r\n Aby wysłac wiadomość należy ją wpisac\r\n w pole do nadawnia wiadomosci i wćisnąc Wyślij, \r\nw polu odczytu tego komputera pojawi się kodowanie\r\n wykorzytsane do wysłania wiadomości.\r\nWiadomość ojawi się ona na drugim komputerze w polu do odczytu.\r\n ";

        }

        private void Info_Load(object sender, EventArgs e)
        {

        }
    }
}
