using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Communicator
{
    public partial class Form1 : Form
    {
        private static readonly int dlugoscramki = 11;
        public int formNumer;
        private char[] bitowo = new char[12];
        private char[] bezprzerw = new char[dlugoscramki];
        private List<char[]> byteArray = new List<char[]>();
        private string binary;
        private string filename;
        private string filepath = @"";

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle; 
            MaximizeBox = false;  
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.listOfForms[0].label4.Text = " To : Komputer 2 ";
            Program.listOfForms[1].label4.Text = " To : Komputer 1 ";
            Program.listOfForms[0].label3.Text = " Subject : ASK -Projekt 4 ";
            Program.listOfForms[1].label3.Text = " Subject : ASK -Projekt 4 ";
            Program.listOfForms[0].Text = "Komputer 1";
            Program.listOfForms[1].Text = "Komputer 2";

        }

        private void ZamianaZnakNaBit(StringBuilder stringBuilder, StringBuilder stringBuilderCleanMessage)
        {
            bitowo[0] = '0';
            bitowo[9] = '1'; 
            bitowo[10] = '1'; 
            bitowo[11] = ' ';
            bezprzerw[0] = '0';
            bezprzerw[9] = '1';
            bezprzerw[10] = '1';

            foreach (char k in textBox.Text)
            {
                int i = 8;
                char znak = BezPolskichZnaków(k);

                binary = Convert.ToString(znak, 2).PadLeft(8, '0');
                foreach (char c in binary)
                {
                    bitowo[i] = c;
                    bezprzerw[i] = c;
                    i--;
                }
                stringBuilder.Append(bitowo);
                stringBuilderCleanMessage.Append(bezprzerw);
            }
        }
        private void LabelText()
        {
            label2.Text = "Kodowanie: ";
            label5.Text = "";
            label2.BackColor = SystemColors.ActiveCaption;
            label2.Dock = DockStyle.Bottom;
            label5.BackColor = SystemColors.ActiveCaption;
            label5.Dock = DockStyle.Bottom;
            label2.BorderStyle = BorderStyle.None;
            label5.BorderStyle = BorderStyle.None;
        }
        private void KonwersjaWiadomosci()
        {
            filename = String.Format("Message{0}.txt", formNumer.ToString());
            StringBuilder stringBuilderWithSpaces = new StringBuilder();
            StringBuilder stringBuilderCleanMessage = new StringBuilder();

            ZamianaZnakNaBit(stringBuilderWithSpaces, stringBuilderCleanMessage);
            readBox.Text = stringBuilderWithSpaces.ToString();
            LabelText();

            try
            {
                System.IO.File.WriteAllText(filepath + filename, stringBuilderCleanMessage.ToString());
            }
            catch (System.UnauthorizedAccessException)
            {
                filepath = @"C:\\Users\Public";
                System.IO.File.WriteAllText(filepath + filename, stringBuilderCleanMessage.ToString());
            }
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            KonwersjaWiadomosci();
            
            int differentFormNumber = 0;

            if (formNumer == 0)
            {
                differentFormNumber = 1;
            }
            else if (formNumer == 1)
            {
                differentFormNumber = 0;
            }
            ReadMessage(differentFormNumber);
         
        }

        private void ReadMessage(int differentFormNumber)
        {
            string readFileName = String.Format(filepath + "Message{0}.txt", formNumer.ToString());  //mine filepath addded
            try
            {
                using (var sr = new StreamReader(readFileName))
                {
                    string sentFile = sr.ReadToEnd();
                    DecodeMessage(sentFile, differentFormNumber);
                }
            }
            catch (IOException e)
            {
                Program.listOfForms[differentFormNumber].readBox.Text = "ERROR";
            }
        }

        private void DecodeMessage(string fileContent, int differentFormNumber)
        {
            int messageLength = fileContent.Length;
            int zdekodowane = 0;
            StringBuilder stringBuilder = new StringBuilder(messageLength);

            for (int i = 0; i < messageLength / dlugoscramki; i++)
            {
                string substring = fileContent.Substring(zdekodowane, dlugoscramki);
                int numerBitu = dlugoscramki;
                char[] binarnie = new char[8];

                WyciagnijOsiem(substring, numerBitu, binarnie);
                zdekodowane += dlugoscramki;
                stringBuilder.Append(binarnie);
            }

            Encoding ascii = Encoding.ASCII;
            String decodedString = ascii.GetString(GetBytesFromBinaryString(stringBuilder.ToString()));
            string finalText = BrzydkieSlowa(decodedString);
            Program.listOfForms[differentFormNumber].readBox.Text = finalText;

            if(differentFormNumber == 0)
            {
                Program.listOfForms[differentFormNumber].label2.Text = "From :  Komputer 2 ";
                Program.listOfForms[differentFormNumber].label5.Text = "Subject : ASK -Projekt 4 ";
                Program.listOfForms[differentFormNumber].label2.BackColor = Color.LightGray;
                Program.listOfForms[differentFormNumber].label2.Dock = DockStyle.Bottom ;
                Program.listOfForms[differentFormNumber].label5.BackColor = Color.LightGray;
                Program.listOfForms[differentFormNumber].label5.Dock = DockStyle.Bottom;
                Program.listOfForms[differentFormNumber].label5.BorderStyle = BorderStyle.Fixed3D;
                Program.listOfForms[differentFormNumber].label2.BorderStyle = BorderStyle.Fixed3D;

            }
            else
            {
                Program.listOfForms[differentFormNumber].label2.Text = "From :  Komputer 1 ";
                Program.listOfForms[differentFormNumber].label5.Text = "Subject : ASK -Projekt 4 ";
                Program.listOfForms[differentFormNumber].label2.BackColor = Color.LightGray;
                Program.listOfForms[differentFormNumber].label2.Dock = DockStyle.Bottom;
                Program.listOfForms[differentFormNumber].label5.BackColor = Color.LightGray;
                Program.listOfForms[differentFormNumber].label5.Dock = DockStyle.Bottom;
                Program.listOfForms[differentFormNumber].label5.BorderStyle = BorderStyle.Fixed3D;
                Program.listOfForms[differentFormNumber].label2.BorderStyle = BorderStyle.Fixed3D;
            }

        }

        private void WyciagnijOsiem(string substring, int bytesNumber, char[] result)
        {
            foreach (char c in substring)
            {
                if (bytesNumber % dlugoscramki == 0 || bytesNumber % dlugoscramki == 1
                    || bytesNumber % dlugoscramki == 2)
                {
                }
                else
                {
                    result[bytesNumber - 3] = c;
                }
                bytesNumber--;
            }
        }

        private Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }

        private char BezPolskichZnaków(char c)
        {
            var changerDict = new Dictionary<Char, Char>()
            {
                { 'ą', 'a' },
                { 'ć', 'c' },
                { 'ę', 'e' },
                { 'ł', 'l' },
                { 'ń', 'n' },
                { 'ó', 'o' },
                { 'ś', 's' },
                { 'ż', 'z' },
                { 'ź', 'z' }
            };
            return changerDict.ContainsKey(c) ? changerDict[c] : c;
        }

        private string BrzydkieSlowa(string decodedText)
        {
            string slownikGrubianstw = File.ReadAllText("slownik.txt");
            string[] slowaBrzydkie = CleanQuotesAndEndOfLines(slownikGrubianstw).Split(',');
            string[] text = decodedText.Split(' ');
            string ladnyText = String.Empty;

            foreach (string slowo in text)
            {
                string sprawdzaneSlowo = slowo;
                string sprawdzaneSlowo2 = slowo.ToLower();
                string dobreSlowo = String.Empty;
                bool czyBrzydkie = Array.Exists(slowaBrzydkie, brzydkie => brzydkie == sprawdzaneSlowo2);
                if (czyBrzydkie) dobreSlowo = new Regex("\\S").Replace(sprawdzaneSlowo, "*");
                else dobreSlowo = sprawdzaneSlowo;
                ladnyText += $"{dobreSlowo} ";
            }

            return ladnyText;
        }

        private string CleanQuotesAndEndOfLines(string filename)
        {
            return filename.Replace("'", "").Replace(" ", "").Replace("\n", "").Replace("\r\n", "").Replace("\r", "");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void informacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();
        }
    }
}
