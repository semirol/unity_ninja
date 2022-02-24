using Core.FixedArithmetic;

namespace Core
{
    public class BattleOperation
    {
        public int FrameNo;
        public int PlayerId;
        public int OperationType;
        public int Direction;
        public FVector3 Position;

        public BattleOperation(int frameNo, int playerId, int operationType, int direction)
        {
            FrameNo = frameNo;
            PlayerId = playerId;
            OperationType = operationType;
            Direction = direction;
        }
        public BattleOperation(int frameNo, int playerId, int operationType, int direction, FVector3 position)
        {
            FrameNo = frameNo;
            PlayerId = playerId;
            OperationType = operationType;
            Direction = direction;
            Position = position;
        }
    }
}