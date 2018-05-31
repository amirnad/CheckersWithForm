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
        //private GameBoard m_CheckersSoldiersBoard = new GameBoard();

        //public void InitializeCheckersGame()
        //{
        //    InitialGameSetting initialSettings = new InitialGameSetting();

        //    FormSettings tempForm = fb.GameSettings;
        //    string player1Name = tempForm.Player1Name;
        //    string player2Name = tempForm.Player2Name;
        //    eBoardSizeOptions boardSize = tempForm.BoardSize;
        //    eTypeOfGame gameType = tempForm.DoublePlayer ? eTypeOfGame.doublePlayer : eTypeOfGame.singlePlayer;

        //    initialSettings.SetGameSettings(player1Name, player2Name, boardSize, gameType);
        //    SessionData.InitializeSessionData(initialSettings);
        //    SessionData.InitializePlayers(initialSettings);
        //    m_CheckersSoldiersBoard.InitializeCheckersBoard();

        //}

        public void RunGame()
        {
            fb.ShowDialog();
        }
    }
}
