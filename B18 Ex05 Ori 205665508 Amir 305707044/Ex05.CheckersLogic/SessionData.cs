namespace Ex05.CheckersLogic
{
    public enum eTypeOfGame
    {
        Undefined, singlePlayer, doublePlayer
    }

    public enum eBoardSizeOptions
    {
        Undefined, SmallBoard = 6, MediumBoard = 8, LargeBoard = 10
    }

    public enum eGameState
    {
        KeepGoing = 0, Tie, WinPlayer1, WinPlayer2, player1Quit, player2Quit, StartOver, Quit, Undefined
    }

    public class SessionData
    {
        public static eBoardSizeOptions m_BoardSize = eBoardSizeOptions.Undefined;
        public static int m_Player1OverallScore = 0;
        public static int m_Player2OverallScore = 0;
        public static int m_Player1Points = 0;
        public static int m_Player2Points = 0;
        public static eTypeOfGame s_GameType = eTypeOfGame.Undefined;
        public static ePlayerOptions m_CurrentActivePlayer = ePlayerOptions.Player1;
        public static ePlayerOptions m_TheOtherPlayer;

        private static readonly Player s_Player1 = new Ex05.CheckersLogic.Player();
        private static readonly Player s_Player2 = new Ex05.CheckersLogic.Player();

        public static Player GetCurrentPlayer()
        {
            if (m_TheOtherPlayer != ePlayerOptions.Player1)
            {
                return s_Player1;
            }
            else
            {
                // pc also sits at player2 spot
                return s_Player2;
            }
        }

        public static Player GetOtherPlayer()
        {
            if (m_TheOtherPlayer == ePlayerOptions.Player1)
            {
                return s_Player1;
            }
            else
            {
                // pc also sits at player2 spot
                return s_Player2;
            }
        }

        public static string GetPlayerName(ePlayerOptions i_PlayerNameToReturn)
        {
            string returnedName = string.Empty;
            switch (i_PlayerNameToReturn)
            {
                case ePlayerOptions.Player1:
                    returnedName = s_Player1.PlayerName;
                    break;
                case ePlayerOptions.Player2:
                    returnedName = s_Player2.PlayerName;
                    break;
            }

            return returnedName;
        }

        public static void InitializePlayers(InitialGameSetting i_NameSettings)
        {
            s_Player1.InitializePlayer(i_NameSettings.GetPlayerName(ePlayerOptions.Player1), Ex05.CheckersLogic.ePlayerOptions.Player1);
            switch (Ex05.CheckersLogic.SessionData.s_GameType)
            {
                case Ex05.CheckersLogic.eTypeOfGame.singlePlayer:
                    s_Player2.InitializePlayer("PC", Ex05.CheckersLogic.ePlayerOptions.ComputerPlayer);
                    m_TheOtherPlayer = ePlayerOptions.ComputerPlayer;

                    break;
                case Ex05.CheckersLogic.eTypeOfGame.doublePlayer:
                    s_Player2.InitializePlayer(i_NameSettings.GetPlayerName(ePlayerOptions.Player2), Ex05.CheckersLogic.ePlayerOptions.Player2);
                    m_TheOtherPlayer = ePlayerOptions.Player2;
                    break;
            }
        }

        public static eGameState checkGameState()
        {
            eGameState resultState;
            if (!s_Player1.SomeBodyAlive())
            {
                resultState = eGameState.WinPlayer2;
            }
            else if (!s_Player2.SomeBodyAlive())
            {
                resultState = eGameState.WinPlayer1;
            }
            else
            {
                if (!s_Player1.ThereIsPossibleMovements() || !s_Player2.ThereIsPossibleMovements())
                {
                    resultState = eGameState.Tie;
                }
                else
                {
                    resultState = eGameState.KeepGoing;
                }
            }

            return resultState;
        }

        public static void CalculateScore(eGameState io_gameState)
        {
            switch (io_gameState)
            {
                case eGameState.WinPlayer1:
                    updateSessionPoints(s_Player1, s_Player2, ref m_Player1Points);

                    m_Player1OverallScore++;
                    break;
                case eGameState.WinPlayer2:
                    updateSessionPoints(s_Player2, s_Player1, ref m_Player2Points);
                    m_Player2OverallScore++;
                    break;
                case eGameState.player1Quit:
                    m_Player2OverallScore++;
                    break;
                case eGameState.player2Quit:
                    m_Player1OverallScore++;
                    break;
            }
        }

        private static void updateSessionPoints(Player i_WinningPlayer, Player i_LosingPlayer, ref int o_WinningPlayerCurrentPoints)
        {
            int pointsHolder = i_WinningPlayer.CalculateTeamValue() - i_LosingPlayer.CalculateTeamValue();
            o_WinningPlayerCurrentPoints += pointsHolder;
        }

        public static void InitializeSessionData(InitialGameSetting gameSettings)
        {
            m_BoardSize = gameSettings.GetBoardSize();
            s_GameType = gameSettings.GetGameType();
        }

        internal static void ChangeTurn()
        {
            ePlayerOptions temp = m_TheOtherPlayer;
            m_TheOtherPlayer = m_CurrentActivePlayer;
            m_CurrentActivePlayer = temp;
        }
    }
}
