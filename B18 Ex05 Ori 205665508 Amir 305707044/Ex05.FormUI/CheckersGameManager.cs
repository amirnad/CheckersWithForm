using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex05.CheckersLogic;

namespace Ex05.FormUI
{
    public class CheckersGameManager
    {
        private FormCheckersGame fb = new FormCheckersGame();

        public void RunGame()
        {
            fb.ShowDialog();
        }
    }
}
