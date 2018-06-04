using System;
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
        private const string k_WinnerMessage = " Won! Congratulations :)";
        private const string k_AnotherGameMessage = "Another Round?";
        private const string k_GoodByeMessage = "Thank you for playing bye bye!";
        private const string k_TieMessage = "Tie!";
        private const string k_SoldierAlreadyExistsMessage = "There is Already a Soldier In This Spot!!!";
        private const string k_EmptySpotMessage = "This Spot is Empty!!!";
        private const string k_IllegalMoveMessage = "This Move is Illegal!!!";
        private const string k_currentActivePlayerTextLabel = "Current Player: {0}";
        private const string k_DefaultName = "Momo";
        private const int buttonWidth = 60;
        private const int buttonHeight = 60;

        private readonly int boardDim;
        private bool m_GameSetProperly = false;
        private int startLeft = 10;
        private int startHeight = 50;

        private Color m_BackColor = Color.FromArgb(255, 237, 204);
        private Label m_Player1Label;
        private Label m_Player2Label;
        private Label m_Player3Label;
        private SoldierButton[,] m_GameButtonsBoard;
        private GameBoard m_CheckersSoldiersBoard = new GameBoard();
        private FormSettings m_Settings = new FormSettings();
        private InitialGameSetting m_InitialSettings = new InitialGameSetting();

        private enum eClickNumber
        {
            MadeNoClicks,
            MadeOneClick,
            SecondClick
        }

        private eClickNumber m_clickNumber;
        private CheckersGameStep m_currentInSelectionMove = new CheckersGameStep();

        private CheckersLogic.Player m_currentActivePlayer;
        private eGameState m_GameState;
        private bool m_DefaultiveSettings = false;

        public FormCheckersGame()
        {
            ensureGameSettingsOk();
            boardDim = (int)m_Settings.BoardSize;
            initializeButtonsBoard();
            initializeGameEngine();
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
                    m_DefaultiveSettings = true;
                }
            }
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
                    m_GameButtonsBoard[row, col].BackgroundImage = Ex05.FormUI.Properties.Resources.whiteButton;
                    m_GameButtonsBoard[row, col].Click += new EventHandler(button_Click);
                    m_GameButtonsBoard[row, col].Width = buttonWidth;
                    m_GameButtonsBoard[row, col].Height = buttonHeight;
                    m_GameButtonsBoard[row, col].SoldierPosition = new Ex05.CheckersLogic.Point(col, row);
                    m_GameButtonsBoard[row, col].Location = new System.Drawing.Point(startLeft + (col * buttonWidth), startHeight + (row * buttonHeight));
                    if (row % 2 == 0)
                    {
                        if (col % 2 == 0)
                        {
                            m_GameButtonsBoard[row, col].Enabled = false;
                            m_GameButtonsBoard[row, col].BackgroundImage = Ex05.FormUI.Properties.Resources.blackButton;
                        }
                        else
                        {
                            if (row < (boardDim / 2) - 1)
                            {
                                m_GameButtonsBoard[row, col].Image = Ex05.FormUI.Properties.Resources.blackPiece;
                            }

                            if (row > boardDim / 2)
                            {
                                m_GameButtonsBoard[row, col].Image = Ex05.FormUI.Properties.Resources.whitePiece;
                            }
                        }
                    }

                    if (row % 2 == 1)
                    {
                        if (col % 2 == 1)
                        {
                            m_GameButtonsBoard[row, col].Enabled = false;
                            m_GameButtonsBoard[row, col].BackgroundImage = Ex05.FormUI.Properties.Resources.blackButton;
                        }
                        else
                        {
                            if (row < (boardDim / 2) - 1)
                            {
                                m_GameButtonsBoard[row, col].Image = Ex05.FormUI.Properties.Resources.blackPiece;
                            }

                            if (row > boardDim / 2)
                            {
                                m_GameButtonsBoard[row, col].Image = Ex05.FormUI.Properties.Resources.whitePiece;
                            }
                        }
                    }

                    this.Controls.Add(m_GameButtonsBoard[row, col]);
                }

                startHeight++;
            }
        }

        public void initializeGameEngine()
        {
            string player1Name = m_Settings.Player1Name;
            string player2Name = m_Settings.Player2Name;
            eBoardSizeOptions boardSize = m_Settings.BoardSize;
            eTypeOfGame gameType = m_Settings.DoublePlayer ? eTypeOfGame.doublePlayer : eTypeOfGame.singlePlayer;

            if (!m_DefaultiveSettings)
            {
                m_InitialSettings.SetGameSettings(player1Name, player2Name, boardSize, gameType);
            }
            else
            {
                m_InitialSettings.SetGameSettings(k_DefaultName, string.Empty, eBoardSizeOptions.MediumBoard, eTypeOfGame.singlePlayer);
            }

            SessionData.InitializeSessionData(m_InitialSettings);
            SessionData.InitializePlayers(m_InitialSettings);
            m_CheckersSoldiersBoard.InitializeCheckersBoard();
        }

        public FormSettings GameSettings
        {
            get { return m_Settings; }
        }

        private void button_Click(object sender, EventArgs e)
        {
            SoldierButton theButton = sender as SoldierButton;
            GameBoard.Soldier soldier = getSoldierFromButtonsBoard(theButton);

            interpretButtonClick(theButton, soldier);
        }

        private GameBoard.Soldier getSoldierFromButtonsBoard(SoldierButton i_ButtonFromBoard)
        {
            GameBoard.Soldier soldierToReturn = m_CheckersSoldiersBoard.GetSoldierFromMatrix(i_ButtonFromBoard.SoldierPosition);

            return soldierToReturn;
        }

        private void interpretButtonClick(SoldierButton io_Button, GameBoard.Soldier o_Soldier)
        {
            m_currentActivePlayer = SessionData.GetCurrentPlayer();
            io_Button.BackgroundImage = Ex05.FormUI.Properties.Resources.pushedButton;

            switch (m_clickNumber)
            {
                case eClickNumber.MadeNoClicks:
                    interpretFirstUserClick(o_Soldier, io_Button);
                    break;
                case eClickNumber.MadeOneClick:
                    interpretSecondUserClick(o_Soldier, io_Button);
                    break;
            }
        }

        private void interpretFirstUserClick(GameBoard.Soldier i_Soldier, SoldierButton o_Button)
        {
            if (i_Soldier == null)
            {
                resetSelectionAndShowMessage(o_Button);
            }
            else
            {
                m_clickNumber = eClickNumber.MadeOneClick;
                m_currentInSelectionMove.CurrentPosition = i_Soldier.Position;
            }
        }

        private void interpretSecondUserClick(GameBoard.Soldier io_Soldier, SoldierButton o_Button)
        {
            if (io_Soldier != null)
            {
                resetSelectionAndShowMessage(o_Button);
            }
            else
            {
                createAStepAndMakeIt(io_Soldier, o_Button);
            }

            if (gameIsFinished())
            {
                endAGame();
            }
        }

        private void resetSelectionAndShowMessage(SoldierButton i_Button)
        {
            backToNormalBackgroundColor(i_Button.SoldierPosition);
            if (m_clickNumber == eClickNumber.MadeOneClick)
            {
                backToNormalBackgroundColor(m_currentInSelectionMove.CurrentPosition);
                if (i_Button.SoldierPosition.XCoord != m_currentInSelectionMove.CurrentPosition.XCoord || i_Button.SoldierPosition.YCooord != m_currentInSelectionMove.CurrentPosition.YCooord)
                {
                    MessageBox.Show(k_SoldierAlreadyExistsMessage);
                }
            }
            else if (m_clickNumber == eClickNumber.MadeNoClicks)
            {
                MessageBox.Show(k_EmptySpotMessage);
            }

            m_clickNumber = eClickNumber.MadeNoClicks;
        }

        private void backToNormalBackgroundColor(CheckersLogic.Point i_Position)
        {
            m_GameButtonsBoard[i_Position.YCooord, i_Position.XCoord].BackgroundImage = Ex05.FormUI.Properties.Resources.whiteButton;
        }

        private void createAStepAndMakeIt(GameBoard.Soldier i_Soldier, SoldierButton i_Button)
        {
            m_currentActivePlayer.updateArmy(m_CheckersSoldiersBoard);
            m_currentInSelectionMove.RequestedPosition = i_Button.SoldierPosition;
            m_currentInSelectionMove.MoveTypeInfo = m_CheckersSoldiersBoard.SortMoveType(m_currentInSelectionMove, m_currentActivePlayer);

            if (m_currentInSelectionMove.MoveTypeInfo.TypeIndicator != eMoveTypes.Undefined)
            {
                moveTheSoldier();
            }
            else
            {
                MessageBox.Show(k_IllegalMoveMessage);
                backToNormalBackgroundColor(m_currentInSelectionMove.CurrentPosition);
                backToNormalBackgroundColor(m_currentInSelectionMove.RequestedPosition);
                m_clickNumber = eClickNumber.MadeNoClicks;
            }

            m_GameState = SessionData.checkGameState();
        }

        // this method moves the soldier on both buttons board and logic board
        // it only gets called if needed - all validity checks were made before
        private void moveTheSoldier()
        {
            m_currentActivePlayer.MakeAMove(m_currentInSelectionMove, m_CheckersSoldiersBoard);
            moveOnScreen(m_currentInSelectionMove);
            m_currentActivePlayer.updateArmy(m_CheckersSoldiersBoard);
            m_GameState = SessionData.checkGameState();
            m_clickNumber = eClickNumber.MadeNoClicks;

            if (m_GameState == eGameState.KeepGoing && m_currentActivePlayer.Team == ePlayerOptions.ComputerPlayer)
            {
                m_currentActivePlayer.updateArmy(m_CheckersSoldiersBoard);
                m_currentInSelectionMove = m_currentActivePlayer.GetRandomMoveForPc();

                // We only do this in case the PC had the last move but it still has available eating moves
                moveTheSoldier();
            }
        }

        private void moveOnScreen(CheckersGameStep i_currentInSelectionMove)
        {
            int currentX = m_currentInSelectionMove.CurrentPosition.XCoord;
            int currentY = m_currentInSelectionMove.CurrentPosition.YCooord;

            int requestedX = m_currentInSelectionMove.RequestedPosition.XCoord;
            int requestedY = m_currentInSelectionMove.RequestedPosition.YCooord;

            Image tempButtonImageForSwap = null;

            backToNormalBackgroundColor(i_currentInSelectionMove.CurrentPosition);
            backToNormalBackgroundColor(i_currentInSelectionMove.RequestedPosition);

            tempButtonImageForSwap = m_GameButtonsBoard[currentY, currentX].Image;
            m_GameButtonsBoard[currentY, currentX].Image = m_GameButtonsBoard[requestedY, requestedX].Image;

            m_GameButtonsBoard[requestedY, requestedX].Image = tempButtonImageForSwap;

            m_currentActivePlayer = SessionData.GetCurrentPlayer();

            if (m_currentActivePlayer.Team == ePlayerOptions.Player1)
            {
                m_Player3Label.Text = string.Format(k_currentActivePlayerTextLabel, m_Settings.Player1Name);
            }
            else
            {
                m_Player3Label.Text = string.Format(k_currentActivePlayerTextLabel, m_Settings.Player2Name);
            }

            Refresh();

            if (i_currentInSelectionMove.MoveTypeInfo.TypeIndicator == eMoveTypes.EatMove)
            {
                CheckersLogic.Point eatenSoldierPosition = m_CheckersSoldiersBoard.calculatePositionOfEatenSoldier(i_currentInSelectionMove);

                int EatenSoldierX = eatenSoldierPosition.XCoord;
                int EatenSoldierY = eatenSoldierPosition.YCooord;

                SoldierButton eatenSoldier = m_GameButtonsBoard[EatenSoldierY, EatenSoldierX];
                eatenSoldier.Image = null;
            }

            if (i_currentInSelectionMove.MoveTypeInfo.KingMove)
            {
                GameBoard.Soldier soldier = m_CheckersSoldiersBoard.GetSoldierFromMatrix(i_currentInSelectionMove.RequestedPosition);
                if (soldier.Team == ePlayerOptions.Player1)
                {
                    m_GameButtonsBoard[requestedY, requestedX].Image = Ex05.FormUI.Properties.Resources.whiteKing;
                }
                else
                {
                    m_GameButtonsBoard[requestedY, requestedX].Image = Ex05.FormUI.Properties.Resources.blackKing;
                }
            }
        }

        private bool gameIsFinished()
        {
            return m_GameState != eGameState.KeepGoing;
        }

        private void endAGame()
        {
            SessionData.CalculateScore(m_GameState);

            showGameFinishedMessageAndAskForAnotherGame();

            if (m_GameState == eGameState.StartOver)
            {
                initializeANotherGame();
            }
            else if (m_GameState == eGameState.Quit)
            {
                MessageBox.Show(k_GoodByeMessage);
                this.Close();
            }
        }

        private void showGameFinishedMessageAndAskForAnotherGame()
        {
            string winnerName = string.Empty;
            StringBuilder messageToShow = new StringBuilder();
            switch (m_GameState)
            {
                case eGameState.WinPlayer1:
                    messageToShow.AppendFormat("Player 1 ({0}){1}", m_Settings.Player1Name, k_WinnerMessage);
                    break;
                case eGameState.WinPlayer2:
                    messageToShow.AppendFormat("Player 2 ({0}){1}", m_Settings.Player2Name, k_WinnerMessage);
                    break;
                case eGameState.Tie:
                    messageToShow.AppendFormat("{0}", k_TieMessage);
                    break;
            }

            messageToShow.Append(Environment.NewLine);
            messageToShow.AppendLine(k_AnotherGameMessage);

            if (MessageBox.Show(messageToShow.ToString(), "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                m_GameState = eGameState.StartOver;
            }
            else
            {
                m_GameState = eGameState.Quit;
            }
        }

        private void initializeANotherGame()
        {
            SessionData.m_CurrentActivePlayer = ePlayerOptions.Player1;
            SessionData.InitializePlayers(m_InitialSettings);
            m_GameState = eGameState.KeepGoing;
            m_CheckersSoldiersBoard.InitializeCheckersBoard();

            foreach (Button b in m_GameButtonsBoard)
            {
                b.Dispose();
            }

            m_Player1Label.Dispose();
            m_Player2Label.Dispose();
            initializeButtonsBoard();
        }

        private void InitializeComponent()
        {
            this.m_Player1Label = new System.Windows.Forms.Label();
            this.m_Player2Label = new System.Windows.Forms.Label();
            this.m_Player3Label = new System.Windows.Forms.Label();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = m_BackColor;
            this.SuspendLayout();

            // label1
            this.m_Player1Label.AutoSize = true;
            this.m_Player1Label.Location = new System.Drawing.Point(startLeft + buttonWidth, startHeight / 3);
            this.m_Player1Label.Name = "label1";
            this.m_Player1Label.Size = new System.Drawing.Size(76, 17);
            this.m_Player1Label.TabIndex = 0;
            this.m_Player1Label.Font = new Font("Calibri", 14, FontStyle.Bold);
            this.m_Player1Label.Text = string.Format("Player 1: {1}", m_Settings.Player1Name, SessionData.m_Player1OverallScore);

            // label2
            this.m_Player2Label.AutoSize = true;
            this.m_Player2Label.Location = new System.Drawing.Point(startLeft + (buttonWidth * (boardDim - 3)), startHeight / 3);
            this.m_Player2Label.Name = "label2";
            this.m_Player2Label.Size = new System.Drawing.Size(76, 17);
            this.m_Player2Label.TabIndex = 1;
            this.m_Player2Label.Font = new Font("Calibri", 14, FontStyle.Bold);
            this.m_Player2Label.Text = string.Format("Player 2: {1}", m_Settings.Player2Name, SessionData.m_Player2OverallScore);
            this.SuspendLayout();

            // label3
            this.m_Player3Label.AutoSize = false;
            this.m_Player3Label.Dock = DockStyle.Fill;
            this.m_Player3Label.Name = "label2";
            this.m_Player3Label.TabIndex = 1;
            this.m_Player3Label.Size = new Size(startLeft + (buttonWidth * boardDim), buttonHeight / 2);
            this.m_Player3Label.Location = new System.Drawing.Point(startLeft, startHeight + (buttonHeight * boardDim) + (buttonHeight / 4));
            this.m_Player3Label.Font = new Font("Calibri", 12, FontStyle.Bold);
            this.m_Player3Label.Text = string.Format(k_currentActivePlayerTextLabel, m_Settings.Player1Name);
            this.m_Player3Label.TextAlign = ContentAlignment.MiddleCenter;
            if (!m_Settings.DoublePlayer)
            {
                this.m_Player3Label.Hide();
            }

            this.SuspendLayout();

            // FormDamkaGame
            this.ClientSize = new Size((startLeft * 2) + (buttonWidth * boardDim), (startHeight * 2) + (buttonHeight * boardDim));
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Controls.Add(this.m_Player3Label);
            this.Controls.Add(this.m_Player2Label);
            this.Controls.Add(this.m_Player1Label);
            this.Name = "FormDamkaGame";
            this.Text = "FormDamkaGame";
            this.ResumeLayout(false);
        }
    }
}