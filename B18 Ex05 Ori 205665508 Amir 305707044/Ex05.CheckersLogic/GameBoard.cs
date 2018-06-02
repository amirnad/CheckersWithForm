using System;
using System.Collections.Generic;
using static System.Math;
namespace Ex05.CheckersLogic
{
    public class GameBoard
    {
        private Soldier[,] m_CheckersBoard = null;
        public enum eSoldierRanks { Regular = 1, King = 4 }

        public class Soldier
        {

            private static Point nextPointToFillPlayer1;
            private static Point nextPointToFillPlayer2;
            private Point m_CoordinateInMatrix;
            internal List<CheckersGameStep> m_PossibleRegularMovements = null;
            internal List<CheckersGameStep> m_PossibleEatMovements = null;

            private ePlayerOptions m_SoldierTeam;
            private eSoldierRanks m_Rank;

            public static Point PointToFillPlayer1
            {
                get { return nextPointToFillPlayer1; }
                private set
                {
                    nextPointToFillPlayer1 = value;
                }
            }
            public static Point PointToFillPlayer2
            {
                get { return nextPointToFillPlayer2; }
                set
                {
                    nextPointToFillPlayer2 = value;
                }
            }
            public Point Position
            {
                get { return m_CoordinateInMatrix; }
                set
                {
                    m_CoordinateInMatrix = value;
                }
            }

            public List<CheckersGameStep> eatPossibleMovements
            {
                get { return m_PossibleEatMovements; }
                set { m_PossibleEatMovements = value; }
            }
            public List<CheckersGameStep> regularPossibleMovements
            {
                get { return m_PossibleRegularMovements; }
                set { m_PossibleRegularMovements = value; }
            }
            public ePlayerOptions Team
            {
                get { return m_SoldierTeam; }
                set { m_SoldierTeam = value; }
            }
            public eSoldierRanks Rank
            {
                get { return m_Rank; }
                set { m_Rank = value; }
            }

            public static Soldier InitializeSoldier(Point i_PositionInMatrix, ePlayerOptions i_Team)
            {
                Soldier returnedSoldier = new Soldier();

                returnedSoldier.Position = i_PositionInMatrix;
                returnedSoldier.regularPossibleMovements = calculateInitPossibleMovements(i_PositionInMatrix, i_Team);
                returnedSoldier.eatPossibleMovements = new List<CheckersGameStep>();
                returnedSoldier.Team = i_Team;
                returnedSoldier.Rank = eSoldierRanks.Regular;
                return returnedSoldier;
            }

            public static List<CheckersGameStep> calculateInitPossibleMovements(Point i_CurrentSoldierPosition, ePlayerOptions playerId)
            {


                List<CheckersGameStep> resultPossibleMovesArray = null;

                int indexForTopRow = 0;
                if (playerId == ePlayerOptions.Player1)
                {
                    switch (SessionData.m_BoardSize)
                    {
                        case eBoardSizeOptions.SmallBoard:
                            indexForTopRow = 4;
                            break;
                        case eBoardSizeOptions.MediumBoard:
                            indexForTopRow = 5;
                            break;

                        case eBoardSizeOptions.LargeBoard:
                            indexForTopRow = 6;
                            break;


                    }
                }
                else
                {
                    switch (SessionData.m_BoardSize)
                    {
                        case eBoardSizeOptions.SmallBoard:
                            indexForTopRow = 1;

                            break;
                        case eBoardSizeOptions.MediumBoard:
                            indexForTopRow = 2;
                            break;

                        case eBoardSizeOptions.LargeBoard:
                            indexForTopRow = 3;
                            break;


                    }
                }
                resultPossibleMovesArray = resetPossibleMovesArray(indexForTopRow, i_CurrentSoldierPosition, playerId);
                return resultPossibleMovesArray;





            }

            internal void BecomeAKing()
            {
                Rank = eSoldierRanks.King;
            }

            private static List<CheckersGameStep> resetPossibleMovesArray(int indexOfTopRow, Point i_CurrentSoldierPosition, ePlayerOptions playerId)
            {
                List<CheckersGameStep> resultPossibleMovesArray = new List<CheckersGameStep>();

                CheckersGameStep stepToTheLeft = new CheckersGameStep();
                CheckersGameStep stepToTheRight = new CheckersGameStep();

                if (i_CurrentSoldierPosition.YCooord == indexOfTopRow)
                {
                    Point MoveToTheLeft = new Point();
                    Point MoveToTheRight = new Point();


                    stepToTheLeft.CurrentPosition = i_CurrentSoldierPosition;
                    stepToTheRight.CurrentPosition = i_CurrentSoldierPosition;
                    if (playerId == ePlayerOptions.Player1)
                    {
                        MoveToTheLeft.XCoord = i_CurrentSoldierPosition.XCoord - 1;
                        MoveToTheLeft.YCooord = i_CurrentSoldierPosition.YCooord - 1;


                        MoveToTheRight.XCoord = i_CurrentSoldierPosition.XCoord + 1;
                        MoveToTheRight.YCooord = i_CurrentSoldierPosition.YCooord - 1;
                    }
                    else
                    {
                        MoveToTheLeft.XCoord = i_CurrentSoldierPosition.XCoord - 1;
                        MoveToTheLeft.YCooord = i_CurrentSoldierPosition.YCooord + 1;


                        MoveToTheRight.XCoord = i_CurrentSoldierPosition.XCoord + 1;
                        MoveToTheRight.YCooord = i_CurrentSoldierPosition.YCooord + 1;
                    }


                    stepToTheLeft.RequestedPosition = MoveToTheLeft;
                    stepToTheRight.RequestedPosition = MoveToTheRight;

                    if (stepToTheLeft.RequestedPosition.XCoord >= 0 && stepToTheLeft.RequestedPosition.XCoord < (int)SessionData.m_BoardSize)
                    {
                        resultPossibleMovesArray.Add(stepToTheLeft);
                    }
                    if (stepToTheRight.RequestedPosition.XCoord >= 0 && stepToTheRight.RequestedPosition.XCoord < (int)SessionData.m_BoardSize)
                    {
                        resultPossibleMovesArray.Add(stepToTheRight);
                    }


                }

                return resultPossibleMovesArray;


            }


            internal static void initializeNextPointToFill()
            {
                int boardSize = (int)SessionData.m_BoardSize;
                nextPointToFillPlayer2.XCoord = 1;
                nextPointToFillPlayer2.YCooord = 0;

                nextPointToFillPlayer1.XCoord = 0;
                nextPointToFillPlayer1.YCooord = boardSize - 1;
            }

            internal static void moveToNextPlace()
            {
                Point localPoint1 = PointToFillPlayer1;
                Point localPoint2 = PointToFillPlayer2;

                localPoint2.XCoord += 2;
                int boardSize = (int)SessionData.m_BoardSize;
                if (localPoint2.XCoord >= boardSize)
                {
                    localPoint2.YCooord++;
                    if (localPoint2.YCooord % 2 != 0)
                    {
                        localPoint2.XCoord = 0;
                    }
                    else
                    {
                        localPoint2.XCoord = 1;
                    }
                }
                localPoint1.XCoord += 2;
                if (localPoint1.XCoord >= boardSize)
                {
                    localPoint1.YCooord--;
                    if (localPoint1.YCooord % 2 != 0)
                    {
                        localPoint1.XCoord = 0;
                    }
                    else
                    {
                        localPoint1.XCoord = 1;
                    }
                }

                PointToFillPlayer1 = localPoint1;
                PointToFillPlayer2 = localPoint2;



            }

            private static List<Point> bringPossibleNeigboursPositions(Point centerPoint)
            {
                const int TopLeft = 0;
                const int TopRight = 1;
                const int BottomLeft = 2;
                const int BottomRight = 3;


                Point LocalPoint = centerPoint;
                List<Point> affectedSoldiersPositions = new List<Point>();
                Point[] points = new Point[4];


                points[TopLeft].XCoord = centerPoint.XCoord - 1;
                points[TopLeft].YCooord = centerPoint.YCooord + 1;

                points[TopRight].XCoord = centerPoint.XCoord + 1;
                points[TopRight].YCooord = centerPoint.YCooord + 1;

                points[BottomRight].XCoord = centerPoint.XCoord + 1;
                points[BottomRight].YCooord = centerPoint.YCooord - 1;

                points[BottomLeft].XCoord = centerPoint.XCoord - 1;
                points[BottomLeft].YCooord = centerPoint.YCooord - 1;



                foreach (Point p in points)
                {
                    if (p.IsInsideBoard())
                    {
                        affectedSoldiersPositions.Add(p);
                    }
                }


                return affectedSoldiersPositions;
            }

            internal void calculatePossibleMovements(ref GameBoard board)
            {

                m_PossibleEatMovements.Clear();
                m_PossibleRegularMovements.Clear();

                CheckersGameStep gameStep;
                List<Point> inspectedPoints = bringPossibleNeigboursPositions(m_CoordinateInMatrix);
                foreach (Point p in inspectedPoints)
                {
                    Soldier s = board.GetSoldierFromMatrix(p);
                    if (s == null)
                    {
                        if (Rank == eSoldierRanks.King)
                        {
                            gameStep = CheckersGameStep.CreateCheckersGameStep(m_CoordinateInMatrix, p);
                            m_PossibleRegularMovements.Add(gameStep);
                        }
                        else//regular soldier
                        {
                            if (Team == ePlayerOptions.Player1)
                            {
                                if (p.YCooord - Position.YCooord < 0)
                                {
                                    gameStep = CheckersGameStep.CreateCheckersGameStep(m_CoordinateInMatrix, p);
                                    m_PossibleRegularMovements.Add(gameStep);
                                }
                            }
                            else
                            {
                                if (p.YCooord - Position.YCooord > 0)
                                {
                                    gameStep = CheckersGameStep.CreateCheckersGameStep(m_CoordinateInMatrix, p);
                                    m_PossibleRegularMovements.Add(gameStep);
                                }
                            }
                        }
                    }
                    else//there is a soldier there
                    {
                        Point PossibleEatingNextPosition = new Point();

                        Point localPointDiffrenceBetweenPoints = new Point();
                        localPointDiffrenceBetweenPoints.XCoord = p.XCoord - m_CoordinateInMatrix.XCoord;
                        localPointDiffrenceBetweenPoints.YCooord = p.YCooord - m_CoordinateInMatrix.YCooord;
                        if (Team != s.Team)
                        {
                            if (Rank == eSoldierRanks.King)
                            {


                                PossibleEatingNextPosition.XCoord = p.XCoord + localPointDiffrenceBetweenPoints.XCoord;
                                PossibleEatingNextPosition.YCooord = p.YCooord + localPointDiffrenceBetweenPoints.YCooord;


                                if (PossibleEatingNextPosition.IsInsideBoard())
                                {
                                    if (board.GetSoldierFromMatrix(PossibleEatingNextPosition) == null)//means the spot is clear
                                    {
                                        gameStep = CheckersGameStep.CreateCheckersGameStep(m_CoordinateInMatrix, PossibleEatingNextPosition);
                                        m_PossibleEatMovements.Add(gameStep);
                                    }
                                }

                            }
                            else//regular soldier
                            {
                                if (Team == ePlayerOptions.Player1)
                                {
                                    if (p.YCooord - Position.YCooord < 0)
                                    {
                                        PossibleEatingNextPosition.XCoord = p.XCoord + localPointDiffrenceBetweenPoints.XCoord;
                                        PossibleEatingNextPosition.YCooord = p.YCooord + localPointDiffrenceBetweenPoints.YCooord;
                                        if (PossibleEatingNextPosition.IsInsideBoard())
                                        {
                                            if (board.GetSoldierFromMatrix(PossibleEatingNextPosition) == null)//means the spot is clear
                                            {
                                                gameStep = CheckersGameStep.CreateCheckersGameStep(m_CoordinateInMatrix, PossibleEatingNextPosition);
                                                m_PossibleEatMovements.Add(gameStep);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (p.YCooord - Position.YCooord > 0)
                                    {
                                        PossibleEatingNextPosition.XCoord = p.XCoord + localPointDiffrenceBetweenPoints.XCoord;
                                        PossibleEatingNextPosition.YCooord = p.YCooord + localPointDiffrenceBetweenPoints.YCooord;
                                        if (PossibleEatingNextPosition.IsInsideBoard())
                                        {
                                            if (board.GetSoldierFromMatrix(PossibleEatingNextPosition) == null)//means the spot is clear
                                            {
                                                gameStep = CheckersGameStep.CreateCheckersGameStep(m_CoordinateInMatrix, PossibleEatingNextPosition);
                                                m_PossibleEatMovements.Add(gameStep);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

            internal void killed(GameBoard gb)
            {

                Player p = SessionData.GetOtherPlayer();
                p.decrementNumberOfSoldier();
                Soldier eatenSoldier = this;
                p.removeFromPlayerArmy(eatenSoldier);
            }
        }

        internal void MoveSoldier(CheckersGameStep io_MoveToExecute)
        {
            Soldier theOneWeMove = GetSoldierFromMatrix(io_MoveToExecute.CurrentPosition);
            theOneWeMove.Position = io_MoveToExecute.RequestedPosition;
            m_CheckersBoard[io_MoveToExecute.CurrentPosition.YCooord, io_MoveToExecute.CurrentPosition.XCoord] = null;
            m_CheckersBoard[io_MoveToExecute.RequestedPosition.YCooord, io_MoveToExecute.RequestedPosition.XCoord] = theOneWeMove;


            if (io_MoveToExecute.MoveTypeInfo.TypeIndicator == eMoveTypes.EatMove)
            {
                Point eatenSoldierPosition = calculatePositionOfEatenSoldier(io_MoveToExecute);

                Soldier eatenSoldier = GetSoldierFromMatrix(eatenSoldierPosition);
                GameBoard gb = this;
                eatenSoldier.killed(gb);
                m_CheckersBoard[eatenSoldier.Position.YCooord, eatenSoldier.Position.XCoord] = null;

            }

        }


        public Point calculatePositionOfEatenSoldier(CheckersGameStep i_move)//chnaged to publc becuse i wanted to use this function to calculate the position of eaten soldier in the matrix form
        {

            Point resultPosition = new Point();

            resultPosition.XCoord = i_move.CurrentPosition.XCoord + ((i_move.RequestedPosition.XCoord - i_move.CurrentPosition.XCoord) / 2);
            resultPosition.YCooord = i_move.CurrentPosition.YCooord + ((i_move.RequestedPosition.YCooord - i_move.CurrentPosition.YCooord) / 2);

            return resultPosition;
        }

        public void InitializeCheckersBoard()
        {

            Soldier.initializeNextPointToFill();
            switch (SessionData.m_BoardSize)
            {
                case eBoardSizeOptions.SmallBoard:
                    m_CheckersBoard = new Soldier[(int)eBoardSizeOptions.SmallBoard, (int)eBoardSizeOptions.SmallBoard];
                    break;
                case eBoardSizeOptions.MediumBoard:
                    m_CheckersBoard = new Soldier[(int)eBoardSizeOptions.MediumBoard, (int)eBoardSizeOptions.MediumBoard];
                    break;
                case eBoardSizeOptions.LargeBoard:
                    m_CheckersBoard = new Soldier[(int)eBoardSizeOptions.LargeBoard, (int)eBoardSizeOptions.LargeBoard];
                    break;
            }

            switch (SessionData.m_BoardSize)
            {

                case eBoardSizeOptions.SmallBoard:
                    InitializeAllSoldiersOnBoard(Player.k_NumberOfSoldiersInSmallBoard);
                    break;
                case eBoardSizeOptions.MediumBoard:
                    InitializeAllSoldiersOnBoard(Player.k_NumberOfSoldiersInMediumBoard);
                    break;
                case eBoardSizeOptions.LargeBoard:
                    InitializeAllSoldiersOnBoard(Player.k_NumberOfSoldiersInLargeBoard);
                    break;
            }


        }

        private void InitializeAllSoldiersOnBoard(int NumberOfSoldiers)
        {
            Point localPointPlayer1 = new Point();
            Point localPointPlayer2 = new Point();

            for (int i = 0; i < NumberOfSoldiers; i++)
            {

                localPointPlayer1 = Soldier.PointToFillPlayer1;
                localPointPlayer2 = Soldier.PointToFillPlayer2;

                m_CheckersBoard[localPointPlayer1.YCooord, localPointPlayer1.XCoord] = Soldier.InitializeSoldier(localPointPlayer1, ePlayerOptions.Player1);
                if (SessionData.s_GameType == eTypeOfGame.doublePlayer)
                {
                    m_CheckersBoard[localPointPlayer2.YCooord, localPointPlayer2.XCoord] = Soldier.InitializeSoldier(localPointPlayer2, ePlayerOptions.Player2);
                }
                else
                {
                    m_CheckersBoard[localPointPlayer2.YCooord, localPointPlayer2.XCoord] = Soldier.InitializeSoldier(localPointPlayer2, ePlayerOptions.ComputerPlayer);
                }




                SessionData.GetCurrentPlayer().addToPlayerArmy(m_CheckersBoard[localPointPlayer1.YCooord, localPointPlayer1.XCoord]);
                SessionData.GetOtherPlayer().addToPlayerArmy(m_CheckersBoard[localPointPlayer2.YCooord, localPointPlayer2.XCoord]);////AI-Practice-MODE






                Soldier.moveToNextPlace();

            }


        }
        public CheckersGameStep.MoveType SortMoveType(CheckersGameStep i_RequestedMove, Player i_currentActivePlayer)
        {
            Soldier currentPositonSoldier = GetSoldierFromMatrix(i_RequestedMove.CurrentPosition);
            Soldier NextPositonSoldier = GetSoldierFromMatrix(i_RequestedMove.RequestedPosition);
            CheckersGameStep.MoveType result = new CheckersGameStep.MoveType();
            List<CheckersGameStep> arrayOfMovements;
            bool validity = true;
            bool exists = false;

            if (currentPositonSoldier == null && !i_RequestedMove.WantsToQuitIndicator)
            {
                validity = false;
            }
            if (validity && currentPositonSoldier.Team != i_currentActivePlayer.Team)
            {
                validity = false;
            }
            if (validity && NextPositonSoldier != null)
            {
                validity = false;
            }
            if (validity && !i_RequestedMove.RequestedPosition.IsInsideBoard())
            {
                validity = false;
            }
            //now soldier cant move back - however it is still kept in the possible movemnts
            if (validity && currentPositonSoldier.Rank != eSoldierRanks.King)
            {
                if (i_RequestedMove.RequestedPosition.YCooord > currentPositonSoldier.Position.YCooord && currentPositonSoldier.Team == ePlayerOptions.Player1)
                {
                    validity = false;
                }
                else if (i_RequestedMove.RequestedPosition.YCooord < currentPositonSoldier.Position.YCooord && currentPositonSoldier.Team == ePlayerOptions.Player2)
                {
                    validity = false;
                }
            }
            if (validity && i_currentActivePlayer.HasEatMoves() && CheckersGameStep.MoveType.CalculateMoveType(i_RequestedMove).TypeIndicator != eMoveTypes.EatMove && !i_RequestedMove.WantsToQuitIndicator)
            {
                validity = false;
            }
            if (validity)
            {
                result = CheckersGameStep.MoveType.CalculateMoveType(i_RequestedMove);

                if (result.TypeIndicator!= eMoveTypes.Undefined)
                {
                    if (currentPositonSoldier.m_PossibleEatMovements.Count != 0 && result.TypeIndicator == eMoveTypes.EatMove)
                        arrayOfMovements = currentPositonSoldier.eatPossibleMovements;
                    else
                        arrayOfMovements = currentPositonSoldier.regularPossibleMovements;

                    foreach (CheckersGameStep step in arrayOfMovements)
                    {
                        if (step.Equals(i_RequestedMove))
                        {
                            exists = true;
                        }
                    }
                }
                else
                {
                    arrayOfMovements = null;
                }



            }

            if (!validity || !exists)
                result.TypeIndicator = eMoveTypes.Undefined;


            return result;
        }

        public Soldier GetSoldierFromMatrix(Point i_GivenCoordinate)
        {
            Soldier returnedSoldier = null;

            if (i_GivenCoordinate.IsInsideBoard())
            {
                returnedSoldier = m_CheckersBoard[i_GivenCoordinate.YCooord, i_GivenCoordinate.XCoord];
            }
            return returnedSoldier;
        }
    }
}
