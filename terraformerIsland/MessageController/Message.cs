using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace terraformerIsland.MessageController
{
    class Message
    {
        public static void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
