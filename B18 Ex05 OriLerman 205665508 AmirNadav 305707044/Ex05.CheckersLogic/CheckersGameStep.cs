using static System.Math;

namespace Ex05.CheckersLogic
{
    public struct CheckersGameStep
    {
        public struct MoveType
        {
            private bool m_KingMove;
            private eMoveTypes m_MoveType;

            public eMoveTypes TypeIndicator
            {
                get { return m_MoveType; }
                set
                {
                    m_MoveType = value;
                }
            }

            public bool KingMove
            {
                get { return m_KingMove; }
                set
                {
                    m_KingMove = value;
                }
            }

            public static MoveType Initalize()
            {
                MoveType result = new MoveType();

                result.m_MoveType = eMoveTypes.Undefined;
                result.m_KingMove = false;

                return result;
            }

            public static MoveType CalculateMoveType(CheckersGameStep i_requestedStep)
            {
                MoveType result = new MoveType();

                int distanceY = 0;
                int distanceX = 0;

                int indexForLastLineOnBoard = 0;

                distanceY = Abs(i_requestedStep.RequestedPosition.YCooord - i_requestedStep.CurrentPosition.YCooord);
                distanceX = Abs(i_requestedStep.RequestedPosition.XCoord - i_requestedStep.CurrentPosition.XCoord);

                if (distanceY == 2 && distanceX == 2)
                {
                    result.m_MoveType = eMoveTypes.EatMove;
                }
                else if (distanceY == 1 && distanceX == 1)
                {
                    result.m_MoveType = eMoveTypes.RegularMove;
                }
                else
                {
                    result.m_MoveType = eMoveTypes.Undefined;
                }

                if (distanceX > 2 || distanceY > 2 || distanceX < 1 || distanceY < 1)
                {
                    result.m_MoveType = eMoveTypes.Undefined;
                }

                switch (SessionData.m_CurrentActivePlayer)
                {
                    case ePlayerOptions.Player1:
                        indexForLastLineOnBoard = 0;
                        break;

                    case ePlayerOptions.ComputerPlayer:
                    case ePlayerOptions.Player2:
                        indexForLastLineOnBoard = (int)SessionData.m_BoardSize - 1;
                        break;
                }

                if (i_requestedStep.RequestedPosition.YCooord == indexForLastLineOnBoard)
                {
                    result.KingMove = true;
                }
                else
                {
                    result.KingMove = false;
                }

                return result;
            }
        }

        private Point m_currentSoldierPosition;
        private Point m_requestedSoldierPosition;
        private MoveType m_moveInfo;
        private bool m_quitIndicator; 

        public static CheckersGameStep CreateCheckersGameStep(Point i_currentSoldierPosition, Point i_requestedSoldierPosition, bool i_ToQuit = false)
        {
            CheckersGameStep result = new CheckersGameStep();

            result.CurrentPosition = i_currentSoldierPosition;
            result.RequestedPosition = i_requestedSoldierPosition;
            result.MoveTypeInfo = MoveType.CalculateMoveType(result);
            result.m_quitIndicator = i_ToQuit;

            return result;
        }

        public bool WantsToQuitIndicator
        {
            get { return m_quitIndicator; }
            set { m_quitIndicator = value; }
        }

        public Point CurrentPosition
        {
            get { return m_currentSoldierPosition; }
            set
            {
                m_currentSoldierPosition.XCoord = value.XCoord;
                m_currentSoldierPosition.YCooord = value.YCooord;
            }
        }

        public Point RequestedPosition
        {
            get { return m_requestedSoldierPosition; }
            set
            {
                m_requestedSoldierPosition.XCoord = value.XCoord;
                m_requestedSoldierPosition.YCooord = value.YCooord;
            }
        }

        public MoveType MoveTypeInfo
        {
            get { return m_moveInfo; }
            set
            {
                m_moveInfo = value;
            }
        }

        public bool Equals(CheckersGameStep i_step)
        {
            bool validity = true;
            if (!(i_step.CurrentPosition.XCoord == CurrentPosition.XCoord && i_step.CurrentPosition.XCoord == CurrentPosition.XCoord && i_step.CurrentPosition.YCooord == CurrentPosition.YCooord && i_step.CurrentPosition.YCooord == CurrentPosition.YCooord))
            {
                validity = false;
            }

            return validity;
        }
    }
}