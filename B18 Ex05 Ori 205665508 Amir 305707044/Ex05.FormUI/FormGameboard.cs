using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Ex05.FormUI
{
    //public class boardForm : Form
    //{
    //    FormBoard m_board = new FormBoard();

    //    public boardForm()
    //    {
    //        m_board.initializeBoard();
    //    }
    //}
    class FormGameboard
    {
        public static void Main()
        {
            FormBoard g = new FormBoard();
            g.ShowDialog();
            //FormGameSettings f = new FormGameSettings();
            //f.ShowDialog();

        }
    }
}
