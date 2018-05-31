using System;
using System.Collections.Generic;

namespace Ex05.CheckersLogic
{
    public enum ePlayerOptions
    {
        Player1, Player2, ComputerPlayer
    }

    public enum eMoveTypes
    {
        EatMove, RegularMove, Undefined
    }

    public class Player
    {
        public const int k_NoSoldiers = 0;
        public const int k_NumberOfSoldiersInSmallBoard = 6;
        public const int k_NumberOfSoldiersInMediumBoard = 12;
        public const int k_NumberOfSoldiersInLargeBoard = 20;
        private ePlayerOptions m_PlayerId;
        private string m_PlayerName = string.Empty;
        private short m_NumberOfSoldiers;
        private List<GameBoard.Soldier> m_PlayerArmy = null;

        public void updateArmy(GameBoard i_gameboard)
        {
            foreach (GameBoard.Soldier s in m_PlayerArmy)
            {
                s.calculatePossibleMovements(ref i_gameboard);
            }
        }

        internal void addToPlayerArmy(GameBoard.Soldier i_soldier)
        {
            m_PlayerArmy.Add(i_soldier);
        }

        internal void removeFromPlayerArmy(GameBoard.Soldier i_soldier)
        {
            m_PlayerArmy.Remove(i_soldier);
        }

        public void decrementNumberOfSoldier()
        {
            m_NumberOfSoldiers--;
        }

        public string PlayerName
        {
            get { return m_PlayerName; }
            set
            {
                m_PlayerName = value;
            }
        }

        public short NumberOfSoldiers
        {
            get { return m_NumberOfSoldiers; }
            private set
            {
                switch (SessionData.m_BoardSize)
                {
                    case eBoardSizeOptions.SmallBoard:
                        if (value > k_NoSoldiers && value <= k_NumberOfSoldiersInSmallBoard)
                        {
                            m_NumberOfSoldiers = value;
                        }

                        break;
                    case eBoardSizeOptions.MediumBoard:
                        if (value > k_NoSoldiers && value <= k_NumberOfSoldiersInMediumBoard)
                        {
                            m_NumberOfSoldiers = value;
                        }

                        break;
                    case eBoardSizeOptions.LargeBoard:
                        if (value > k_NoSoldiers && value <= k_NumberOfSoldiersInLargeBoard)
                        {
                            m_NumberOfSoldiers = value;
                        }

                        break;
                }
            }
        }


        public CheckersGameStep GetRandomMoveForPc()
        {
            Random stepToExecuteIndex = new Random();
            CheckersGameStep returnedStep = new CheckersGameStep();

            List<CheckersGameStep> arr = new List<CheckersGameStep>();
            foreach (GameBoard.Soldier s in m_PlayerArmy)
            {
                foreach (CheckersGameStep step in s.m_PossibleEatMovements)
                {
                    arr.Add(step);
                }
                foreach (CheckersGameStep step in s.m_PossibleRegularMovements)
                {
                    arr.Add(step);
                }

                returnedStep = arr[stepToExecuteIndex.Next(arr.Count)];


            }
            return returnedStep;
        }

        public ePlayerOptions Team
        {
            get { return m_PlayerId; }
            private set
            {
                m_PlayerId = value;
            }
        }

        public void InitializePlayer(string i_PlayerName, ePlayerOptions i_PlayerId = ePlayerOptions.Player1) // input is already checked in  UI project
        {
            PlayerName = i_PlayerName;
            Team = i_PlayerId;

            switch (SessionData.m_BoardSize)
            {
                case eBoardSizeOptions.SmallBoard:
                    NumberOfSoldiers = k_NumberOfSoldiersInSmallBoard;
                    break;
                case eBoardSizeOptions.MediumBoard:
                    NumberOfSoldiers = k_NumberOfSoldiersInMediumBoard;

                    break;
                case eBoardSizeOptions.LargeBoard:
                    NumberOfSoldiers = k_NumberOfSoldiersInLargeBoard;
                    break;
            }

            m_PlayerArmy = new List<GameBoard.Soldier>(NumberOfSoldiers);
        }

        // in make a move there is the logic of switcing turns
        //the rule is that after MakeAMove() the player that is playing the next move will be in current player in sessionData class 
        public void MakeAMove(CheckersGameStep io_MoveToExecute, GameBoard io_CheckersBoard)
        {
            GameBoard.Soldier currentSoldierToMove = io_CheckersBoard.GetSoldierFromMatrix(io_MoveToExecute.CurrentPosition);
            io_CheckersBoard.MoveSoldier(io_MoveToExecute);

            if (io_MoveToExecute.MoveTypeInfo.KingMove)
            {
                currentSoldierToMove.BecomeAKing();
            }

            if (io_MoveToExecute.MoveTypeInfo.TypeIndicator != eMoveTypes.EatMove)
            {
                SessionData.ChangeTurn();
            }
            else
            {
                currentSoldierToMove.calculatePossibleMovements(ref io_CheckersBoard);
                if (currentSoldierToMove.eatPossibleMovements.Count == 0)
                {
                    SessionData.ChangeTurn();
                }
            }

            // here was supposed to be else --> do nothing cuz we dont want switch turns --> player ate a soldier and can creat a combo
        }

        public bool SomeBodyAlive()
        {
            return m_NumberOfSoldiers > 0;
        }

        public bool ThereIsPossibleMovements()
        {
            bool someBodyAlive = false;
            foreach (GameBoard.Soldier s in m_PlayerArmy)
            {
                if (s.m_PossibleEatMovements.Count != 0 || s.m_PossibleRegularMovements.Count != 0)
                {
                    someBodyAlive = true;
                    break;
                }
            }

            return someBodyAlive;
        }



        internal bool HasEatMoves()
        {
            bool playerMustCommitEatMove = false;
            foreach (GameBoard.Soldier soldierToInspect in m_PlayerArmy)
            {
                if (soldierToInspect.eatPossibleMovements.Count != 0)
                {
                    playerMustCommitEatMove = true;
                    break;
                }
            }

            return playerMustCommitEatMove;
        }

        internal int CalculateTeamValue()
        {
            int returnedValue = 0;
            foreach (GameBoard.Soldier soldierToCheck in m_PlayerArmy)
            {
                switch (soldierToCheck.Rank)
                {
                    case GameBoard.eSoldierRanks.Regular:
                        returnedValue += (int)GameBoard.eSoldierRanks.Regular;
                        break;
                    case GameBoard.eSoldierRanks.King:
                        returnedValue += (int)GameBoard.eSoldierRanks.King;
                        break;
                }
            }

            return returnedValue;
        }
    }
}
