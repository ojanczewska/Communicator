using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Communicator
{
    public class Polaczenie : ApplicationContext
    {
        private int openForms;
        public Polaczenie(params Form[] forms)
        {
            openForms = forms.Length;

            foreach (var form in forms)
            {
                form.FormClosed += (s, args) =>
                {
                    if (Interlocked.Decrement(ref openForms) == 0)
                        ExitThread();

                    if (openForms == 0 || openForms == 1)
                    {
                        Application.ExitThread();
                    }
                };

                form.Show();
            }
        }
    }
}
