
using System.Collections.Generic;

namespace Core
{
    public class NinjaMessage
    {
        
    }

    public class NinjaEnterMessage : NinjaMessage
    {
        public string RoomNumber;

        public NinjaEnterMessage(string roomNumber)
        {
            RoomNumber = roomNumber;
        }
        
    }
    public class NinjaRoomMessage : NinjaMessage
    {
        public int IfP1;

        public NinjaRoomMessage(int ifP1)
        {
            IfP1 = ifP1;
        }
    }
    
    public class NinjaReadyMessage : NinjaMessage
    {
    }
    public class NinjaFrameMessage : NinjaMessage
    {
        public List<Frame> FrameList;

        public NinjaFrameMessage(List<Frame> frameList)
        {
            FrameList = frameList;
        }
    }
    public class NinjaOperationMessage : NinjaMessage
    {
        public BattleOperation Operation;

        public NinjaOperationMessage(BattleOperation operation)
        {
            Operation = operation;
        }
    }

    public class NinjaBattleEndMessage : NinjaMessage
    {
    }
}