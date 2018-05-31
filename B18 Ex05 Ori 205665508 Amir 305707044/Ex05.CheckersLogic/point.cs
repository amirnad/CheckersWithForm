using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.CheckersLogic
{
    public struct Point
    {
        private int m_x;
        private int m_y;

        public Point(int i_x, int i_y)
        {
            m_x = i_x;
            m_y = i_y;
        }

        public bool IsInsideBoard()
        {
            bool result = true;
            int boardSize = (int)SessionData.m_BoardSize;
            if (m_x > boardSize - 1 || m_x < 0)
            {
                result = false;
            }

            if (m_y > boardSize - 1 || m_y < 0)
            {
                result = false;
            }

            return result;
        }

        public int YCooord
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int XCoord
        {
            get { return m_x; }
            set { m_x = value; }
        }
    }
}
