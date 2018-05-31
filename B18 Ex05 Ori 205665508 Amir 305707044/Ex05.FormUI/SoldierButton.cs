using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Ex05.FormUI
{
    public class SoldierButton : Button
    {
        private Ex05.CheckersLogic.Point m_PositionInBoard = new Ex05.CheckersLogic.Point();

        public Ex05.CheckersLogic.Point SoldierPosition
        {
            get { return m_PositionInBoard; }
            set { m_PositionInBoard = value; }
        }


    }
}
