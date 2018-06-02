﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Ex05.CheckersLogic;

namespace Ex05.FormUI
{
    public class FormCheckersGame : Form
    {
        private const string k_SettingsOk = "Settings were set, Enjoy Your Game!";
        private const string k_SettingsBad = "Invalid game settings, Try Again!";
        private const string k_currentActivePlayerTextLabel = "current active player:{0}";
        private const string k_player1String = "player1";
        private const string k_player2String = "player2";

        private bool m_GameSetProperly = false;
        private readonly int boardDim;
        private const int buttonWidth = 60;
        private const int buttonHeight = 60;
        private int startLeft = 10;
        private int startHeight = 50;

        private Label m_Player1Label;
        private Label m_Player2Label;
        private Label m_Player3Label;
        private SoldierButton[,] m_GameButtonsBoard;
        private GameBoard m_CheckersSoldiersBoard = new GameBoard();
        private FormSettings m_Settings = new FormSettings();

        private enum eClickNumber { none, first, second }
        private eClickNumber m_clickNumber;
        private CheckersGameStep m_currentInSelectionMove = new CheckersGameStep();

        private CheckersLogic.Player m_currentActivePlayer;
        private eGameState m_gameState;

        public FormCheckersGame()
        {
            ensureGameSettingsOk();
            boardDim = (int)m_Settings.BoardSize;
            initializeButtonsBoard();
            initializeGameEngine();
        }


        public FormSettings GameSettings
        {
            get { return m_Settings; }
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

        public void initializeGameEngine()
        {
            InitialGameSetting initialSettings = new InitialGameSetting();

            string player1Name = m_Settings.Player1Name;
            string player2Name = m_Settings.Player2Name;
            eBoardSizeOptions boardSize = m_Settings.BoardSize;
            eTypeOfGame gameType = m_Settings.DoublePlayer ? eTypeOfGame.doublePlayer : eTypeOfGame.singlePlayer;

            initialSettings.SetGameSettings(player1Name, player2Name, boardSize, gameType);
            SessionData.InitializeSessionData(initialSettings);
            SessionData.InitializePlayers(initialSettings);
            m_CheckersSoldiersBoard.InitializeCheckersBoard();

        }

        private void initializeButtonsBoard()
        {
            InitializeComponent();
            m_GameButtonsBoard = new SoldierButton[boardDim, boardDim];
            for (int row = 0; row < boardDim; row++)
            {
                for (int col = 0; col < boardDim; col++)
                {
                    m_GameButtonsBoard[row, col] = new SoldierButton();
                    m_GameButtonsBoard[row, col].Click += new EventHandler(button_Click);
                    m_GameButtonsBoard[row, col].Width = buttonWidth;
                    m_GameButtonsBoard[row, col].Height = buttonHeight;
                    m_GameButtonsBoard[row, col].Text = string.Format("({0},{1})", row, col);
                    m_GameButtonsBoard[row, col].SoldierPosition = new Ex05.CheckersLogic.Point(col, row);
                    m_GameButtonsBoard[row, col].Location = new System.Drawing.Point(startLeft + col * buttonWidth, startHeight + row * buttonHeight);
                    if (row % 2 == 0)
                    {
                        if (col % 2 == 0)
                        {
                            m_GameButtonsBoard[row, col].Enabled = false;
                            m_GameButtonsBoard[row, col].BackColor = Color.DimGray;
                        }
                        else
                        {
                            if (row < boardDim / 2 - 1)
                            {
                                m_GameButtonsBoard[row, col].Text = "Player2";
                            }
                            if (row > boardDim / 2)
                            {
                                m_GameButtonsBoard[row, col].Text = "Player1";
                            }
                        }
                    }
                    if (row % 2 == 1)
                    {
                        if (col % 2 == 1)
                        {
                            m_GameButtonsBoard[row, col].Enabled = false;
                            m_GameButtonsBoard[row, col].BackColor = Color.DimGray;
                        }
                        else
                        {
                            if (row < boardDim / 2 - 1)
                            {
                                m_GameButtonsBoard[row, col].Text = "Player2";
                            }
                            if (row > boardDim / 2)
                            {
                                m_GameButtonsBoard[row, col].Text = "Player1";
                            }
                        }
                    }
                    this.Controls.Add(m_GameButtonsBoard[row, col]);

                }
                startHeight++;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            SoldierButton theButton = sender as SoldierButton;
            GameBoard.Soldier soldier = getSoldierFromButtonsBoard(theButton);
            m_currentActivePlayer = SessionData.GetCurrentPlayer();

            if (m_clickNumber == eClickNumber.none)
            {
                if (soldier == null)
                {
                    m_clickNumber = eClickNumber.none;
                    backToGray(theButton.SoldierPosition);
                    MessageBox.Show("current point isnt filled");

                }
                else
                {
                    m_clickNumber = eClickNumber.first;
                    m_currentInSelectionMove.CurrentPosition = soldier.Position;
                }
            }

            else if (m_clickNumber == eClickNumber.first)
            {

                if (soldier != null)
                {
                    m_clickNumber = eClickNumber.none;
                    backToGray(theButton.SoldierPosition);
                    backToGray(m_currentInSelectionMove.CurrentPosition);
                    if (theButton.SoldierPosition.XCoord != m_currentInSelectionMove.CurrentPosition.XCoord || theButton.SoldierPosition.YCooord != m_currentInSelectionMove.CurrentPosition.YCooord)
                    {
                        MessageBox.Show("destination positin is filled");
                    }
                }
                else
                {

                    m_currentActivePlayer.updateArmy(m_CheckersSoldiersBoard);
                    m_currentInSelectionMove.RequestedPosition = theButton.SoldierPosition;
                    m_currentInSelectionMove.MoveTypeInfo = m_CheckersSoldiersBoard.SortMoveType(m_currentInSelectionMove, m_currentActivePlayer);
                    if (m_currentInSelectionMove.MoveTypeInfo.TypeIndicator != eMoveTypes.Undefined)
                    {
                        m_currentActivePlayer.MakeAMove(m_currentInSelectionMove, m_CheckersSoldiersBoard);
                        moveOnScreen(m_currentInSelectionMove);
                        m_gameState = SessionData.checkGameState();
                        if (m_gameState == eGameState.KeepGoing)
                        {
                            EndGame();
                        }
                    }
                    else
                    {
                        MessageBox.Show("illegal move!");
                    }

                    backToGray(m_currentInSelectionMove.CurrentPosition);
                    backToGray(m_currentInSelectionMove.RequestedPosition);
                    m_clickNumber = eClickNumber.none;

                    if(m_currentActivePlayer.Team == ePlayerOptions.ComputerPlayer)
                    {
                        m_currentActivePlayer.updateArmy(m_CheckersSoldiersBoard);
                        m_currentInSelectionMove = m_currentActivePlayer.GetRandomMoveForPc();
                        //same as 15 rows up need to seperate into function
                        m_currentActivePlayer.MakeAMove(m_currentInSelectionMove, m_CheckersSoldiersBoard);
                        moveOnScreen(m_currentInSelectionMove);
                        m_gameState = SessionData.checkGameState();
                        if (m_gameState != eGameState.KeepGoing)
                        {
                            EndGame();
                        }
                    }
                }

            }
        }

        private void EndGame()
        {
            SessionData.CalculateScore(m_gameState);
          //  PrintGameResult(m_gameState);
        //    m_gameState = CheckIfPlayerWantsAnotherGame();
        }

        private eGameState CheckIfPlayerWantsAnotherGame()
        {
            throw new NotImplementedException();
        }

        private void PrintGameResult(eGameState i_gameState)
        {
            throw new NotImplementedException();
        }

        private void backToGray(CheckersLogic.Point i_Position)
        {
            m_GameButtonsBoard[i_Position.YCooord, i_Position.XCoord].BackColor = Color.Gainsboro;
        }

        private void moveOnScreen(CheckersGameStep i_currentInSelectionMove)
        {
            int currentX = m_currentInSelectionMove.CurrentPosition.XCoord;
            int currentY = m_currentInSelectionMove.CurrentPosition.YCooord;

            int requestedX = m_currentInSelectionMove.RequestedPosition.XCoord;
            int requestedY = m_currentInSelectionMove.RequestedPosition.YCooord;



            string tempButtonTextForSwap = string.Empty;
            tempButtonTextForSwap = m_GameButtonsBoard[currentY, currentX].Text;
            m_GameButtonsBoard[currentY, currentX].Text = m_GameButtonsBoard[requestedY, requestedX].Text;
            m_GameButtonsBoard[requestedY, requestedX].Text = tempButtonTextForSwap;

            m_currentActivePlayer = SessionData.GetCurrentPlayer();
            if (m_currentActivePlayer.Team == ePlayerOptions.Player1)
            {
                m_Player3Label.Text = string.Format(k_currentActivePlayerTextLabel, k_player1String);
            }
            else
            {
                m_Player3Label.Text = string.Format(k_currentActivePlayerTextLabel, k_player2String);
            }

            Refresh();

            if (i_currentInSelectionMove.MoveTypeInfo.TypeIndicator == eMoveTypes.EatMove)
            {
                CheckersLogic.Point eatenSoldierPosition = m_CheckersSoldiersBoard.calculatePositionOfEatenSoldier(i_currentInSelectionMove);

                int EatenSoldierX = eatenSoldierPosition.XCoord;
                int EatenSoldierY = eatenSoldierPosition.YCooord;

                SoldierButton eatenSoldier = m_GameButtonsBoard[EatenSoldierY, EatenSoldierX];
                eatenSoldier.Text = "";

            }





        }


        private GameBoard.Soldier getSoldierFromButtonsBoard(SoldierButton i_ButtonFromBoard)
        {
            i_ButtonFromBoard.BackColor = Color.LightBlue;
            GameBoard.Soldier soldierToReturn = m_CheckersSoldiersBoard.GetSoldierFromMatrix(i_ButtonFromBoard.SoldierPosition);

            return soldierToReturn;
        }

        private void InitializeComponent()
        {
            this.m_Player1Label = new System.Windows.Forms.Label();
            this.m_Player2Label = new System.Windows.Forms.Label();
            this.m_Player3Label = new System.Windows.Forms.Label();

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
            // label3
            // 
            this.m_Player3Label.AutoSize = false;
            this.m_Player3Label.Location = new System.Drawing.Point(260, 18);
            this.m_Player3Label.Name = "label2";
            this.m_Player3Label.Size = new System.Drawing.Size(150, 17);
            this.m_Player3Label.TabIndex = 1;
            this.m_Player3Label.Text = "current active player: player1";
            this.SuspendLayout();
            // 
            // FormDamkaGame
            // 
            this.ClientSize = new Size(startLeft * 2 + buttonWidth * boardDim, startHeight * 2 + buttonHeight * boardDim);// new System.Drawing.Size(282, 255);
            this.Controls.Add(this.m_Player3Label);
            this.Controls.Add(this.m_Player2Label);
            this.Controls.Add(this.m_Player1Label);
            this.Name = "FormDamkaGame";
            this.Text = "FormDamkaGame";
            this.ResumeLayout(false);

        }

    }
}

