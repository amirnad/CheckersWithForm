using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.FormUI
{
    public class FormBoard : Form
    {
        private int boardDim;
        private const int buttonWidth = 60;
        private const int buttonHeight = 60;
        private int startLeft = 10;
        private int startHeight = 10;
        private Button[,] leBoard;
        public FormGameSettings m_GameSettings = new FormGameSettings();


        public FormBoard()
        {
            initializeGameSettings();
            boardDim = (int)m_GameSettings.BoardSize;
            initializeBoard();
        }

        public void initializeBoard()
        {
            leBoard = new Button[boardDim, boardDim];
            this.ClientSize = new Size(startLeft * 2 + buttonWidth * boardDim, startHeight * 2 + buttonHeight * boardDim);
            for (int row = 0; row < boardDim; row++)
            {
                for (int col = 0; col < boardDim; col++)
                {
                    leBoard[row, col] = new Button();
                    leBoard[row, col].Width = buttonWidth;
                    leBoard[row, col].Height = buttonHeight;
                    leBoard[row, col].Text = string.Format("({0},{1})", row, col);
                    leBoard[row, col].Location = new Point(startLeft + col * buttonWidth, startHeight + row * buttonHeight);
                    this.Controls.Add(leBoard[row, col]);
                }
                startHeight++;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormBoard
            // 
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Name = "FormBoard";
            this.ResumeLayout(false);

        }


        //after this method, m_GameSettings holds the game settings
        private void initializeGameSettings()
        {
            m_GameSettings.ShowDialog();
            //if (m_GameSettings.ShowDialog() == DialogResult.Cancel)
            //{
            //    Environment.Exit(0);
            //}
            //else if (m_GameSettings.ShowDialog() == DialogResult.Retry)
            //{
            //    initializeGameSettings();
            //}
        }
    }
}

