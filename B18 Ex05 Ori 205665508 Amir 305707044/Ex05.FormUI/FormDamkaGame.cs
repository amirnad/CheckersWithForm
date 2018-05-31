using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.FormUI
{
    public class FormDamkaGame : Form
    {
        private const string k_SettingsOk = "Settings were set, Enjoy Your Game!";
        private const string k_SettingsBad = "Invalid game settings, Try Again!";

        private bool m_GameSetProperly = false;
        private readonly int boardDim;
        private const int buttonWidth = 60;
        private const int buttonHeight = 60;
        private int startLeft = 10;
        private int startHeight = 50;

        private Label m_Player1Label;
        private Label m_Player2Label;
        private Button[,] m_GameButtonsBoard;
        private FormSettings m_Settings = new FormSettings();
        
        public FormDamkaGame()
        {
            ensureGameSettingsOk();
            boardDim = (int)m_Settings.BoardSize;
            initializeBoard();
        }

        private void ensureGameSettingsOk()
        {
            if (!m_GameSetProperly)
            {
                if (m_Settings.ShowDialog() == DialogResult.OK)
                {
                    if (m_Settings.Player1Name != string.Empty)
                    {
                        if (m_Settings.DoublePlayer && m_Settings.Player2Name != string.Empty)
                        {
                            m_GameSetProperly = true;
                        }
                        else if (!m_Settings.DoublePlayer)
                        {
                            m_GameSetProperly = true;
                        }
                        else
                        {
                            ensureGameSettingsOk();
                        }
                    }
                    else
                    {
                        ensureGameSettingsOk();
                    }

                }
                else if (m_Settings.DialogResult == DialogResult.Cancel)
                {
                    this.Close();
                    Application.Exit();
                }
            }
        }

        public void initializeBoard()
        {
            InitializeComponent();
            m_GameButtonsBoard = new Button[boardDim, boardDim];
            for (int row = 0; row < boardDim; row++)
            {
                for (int col = 0; col < boardDim; col++)
                {
                    m_GameButtonsBoard[row, col] = new Button();
                    m_GameButtonsBoard[row, col].Width = buttonWidth;
                    m_GameButtonsBoard[row, col].Height = buttonHeight;
                    m_GameButtonsBoard[row, col].Text = string.Format("({0},{1})", row, col);
                    m_GameButtonsBoard[row, col].Location = new Point(startLeft + col * buttonWidth, startHeight + row * buttonHeight);
                    this.Controls.Add(m_GameButtonsBoard[row, col]);
                }
                startHeight++;
            }
        }

        private void InitializeComponent()
        {
            this.m_Player1Label = new System.Windows.Forms.Label();
            this.m_Player2Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.m_Player1Label.AutoSize = false;

            this.m_Player1Label.Location = new System.Drawing.Point(66, 18);
            this.m_Player1Label.Name = "label1";
            this.m_Player1Label.Size = new System.Drawing.Size(76, 17);
            this.m_Player1Label.TabIndex = 0;
            this.m_Player1Label.Text = "Player 1: 0";
            // 
            // label2
            // 
            this.m_Player2Label.AutoSize = false;
            this.m_Player2Label.Location = new System.Drawing.Point(192, 18);
            this.m_Player2Label.Name = "label2";
            this.m_Player2Label.Size = new System.Drawing.Size(76, 17);
            this.m_Player2Label.TabIndex = 1;
            this.m_Player2Label.Text = "Player 2: 0";
            this.SuspendLayout();
            // 
            // FormDamkaGame
            // 
            this.ClientSize = new Size(startLeft * 2 + buttonWidth * boardDim, startHeight * 2 + buttonHeight * boardDim);// new System.Drawing.Size(282, 255);
            this.Controls.Add(this.m_Player2Label);
            this.Controls.Add(this.m_Player1Label);
            this.Name = "FormDamkaGame";
            this.ResumeLayout(false);

        }

    }
}

