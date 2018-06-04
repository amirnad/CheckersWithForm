using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.CheckersLogic
{
    public class InitialGameSetting
    {
        private string m_Player1Name = string.Empty;
        private string m_Player2Name = string.Empty;
        private eBoardSizeOptions m_BoardSize = eBoardSizeOptions.Undefined;
        private eTypeOfGame m_GameType = eTypeOfGame.Undefined;

        public void SetGameSettings(string o_playerXInput, string o_playerOInput, eBoardSizeOptions o_BoardSizeInput, eTypeOfGame o_GameTypeInput)
        {
            m_Player1Name = o_playerXInput;
            m_Player2Name = o_playerOInput;
            m_BoardSize = o_BoardSizeInput;
            m_GameType = o_GameTypeInput;
        }

        public string GetPlayerName(ePlayerOptions i_PlayerIdentifier)
        {
            string nameToReturn = string.Empty;
            switch (i_PlayerIdentifier)
            {
                case ePlayerOptions.Player1:
                    nameToReturn = m_Player1Name;
                    break;
                case ePlayerOptions.Player2:
                    nameToReturn = m_Player2Name;
                    break;
                case ePlayerOptions.ComputerPlayer:
                    nameToReturn = "PC";
                    break;                     
            }

            return nameToReturn;
        }

        public eBoardSizeOptions GetBoardSize()
        {
            return m_BoardSize;
        }

        public eTypeOfGame GetGameType()
        {
            return m_GameType;
        }
    }
}
