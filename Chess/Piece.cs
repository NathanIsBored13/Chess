using System;
using System.Collections.Generic;

namespace Chess
{
    public enum Type
    {
        King,
        Queen,
        Rook,
        Knight,
        Bishop,
        Pawn
    }

    enum MoveType
    {
        Capture,
        Move
    }

    struct PieceMove
    {
        public Vector vector;
        public Vector[] additionalMoves;
        public MoveType type;

        public PieceMove(Vector vector, MoveType type)
        {
            this.vector = vector;
            additionalMoves = null;
            this.type = type;
        }

        public PieceMove(Vector vector, Vector[] additionalMoves, MoveType type)
        {
            this.vector = vector;
            this.additionalMoves = additionalMoves;
            this.type = type;
        }
    }

    struct PieceMovesMask
    {
        public BitBoard attacks;
        public BitBoard moves;

        public PieceMovesMask(BitBoard attacks, BitBoard moves)
        {
            this.attacks = attacks;
            this.moves = moves;
        }
    }

    abstract class Piece : ICloneable
    {
        private readonly bool colour;

        public Piece(bool colour)
        {
            this.colour = colour;
        }

        public static Piece Initialise(Type type, bool colour)
        {
            switch (type)
            {
                case Type.Bishop:
                    return new Bishop(colour);
                case Type.King:
                    return new King(colour);
                case Type.Knight:
                    return new Knight(colour);
                case Type.Pawn:
                    return new Pawn(colour);
                case Type.Queen:
                    return new Queen(colour);
                case Type.Rook:
                    return new Rook(colour);
            }
            return null;
        }

        public object Clone() => MemberwiseClone();

        public bool GetColour() => colour;

        public abstract new Type GetType();

        public abstract List<PieceMove> GetMovesMask(Board board, Point position);

        public abstract BitBoard GetSeen(Board board, Point position);
    }
}
