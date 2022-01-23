using System.Collections.Generic;

namespace Core
{
    public class Frame
    {
        public int FrameNo;
        public List<BattleOperation> OperationList;

        public Frame(int frameNo, List<BattleOperation> operationList)
        {
            FrameNo = frameNo;
            OperationList = operationList;
        }
    }
}