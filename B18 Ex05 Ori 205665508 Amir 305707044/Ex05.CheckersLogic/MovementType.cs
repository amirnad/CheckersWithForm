namespace Ex05.CheckersLogic
{
    public struct MovementType
    {
        private eMoveTypes m_MoveType;
        private bool m_BecomeAKingMove;

        public eMoveTypes Type
        {
            get { return m_MoveType; }
            set { m_MoveType = value; }
        }
        public bool KingMove
        {
            get { return m_BecomeAKingMove; }
            set { m_BecomeAKingMove = value; }
        }
        


    }
}

