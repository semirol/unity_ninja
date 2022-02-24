using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core;
using Core.FixedArithmetic;
using Managers;

namespace Utils
{
    public class MessageEncodingUtils
    {
        public static byte[] Encode(NinjaMessage message)
        {
            byte[] bytes = new byte[64];
            MemoryStream ms = new MemoryStream(bytes);
            BinaryWriter bw = new BinaryWriter(ms);
            if (message is NinjaEnterMessage)
            {
                NinjaEnterMessage msg = (NinjaEnterMessage)message;
                bw.Write(NinjaMessageEnum.ENTER);
                bw.Write(LocalDataManager.Instance.Get<int>("playerId"));
                bw.Write(msg.RoomNumber);
            }
            else if (message is NinjaReadyMessage)
            {
                bw.Write(NinjaMessageEnum.READY);
                bw.Write(LocalDataManager.Instance.Get<int>("playerId"));

            }
            if (message is NinjaOperationMessage)
            {
                NinjaOperationMessage msg = (NinjaOperationMessage) message;
                bw.Write(NinjaMessageEnum.OPERATION);
                bw.Write(LocalDataManager.Instance.Get<int>("playerId"));
                BattleOperation operation = msg.Operation;
                bw.Write(operation.FrameNo);
                int operationType = operation.OperationType;
                bw.Write(operationType);
                if (operationType == OperationTypeEnum.MOVE)
                {
                    bw.Write(operation.Position.X.Value);
                    bw.Write(operation.Position.Z.Value);
                }
                bw.Write(operation.Direction);
            }
            bw.Flush();
            bw.Close();
            ms.Close();
            return bytes;
        }

        public static NinjaMessage Decode(byte[] bytes)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(bytes), Encoding.UTF8);
            int messageTypeInt = br.ReadInt32();
            if (messageTypeInt == NinjaMessageEnum.ROOM)
            {
                NinjaRoomMessage message = new NinjaRoomMessage(br.ReadInt32());
                return message;
            }
            else if (messageTypeInt == NinjaMessageEnum.OPERATION)
            {
                BattleOperation operation = ReadBattleOperation(br, 0);
                NinjaOperationMessage message = new NinjaOperationMessage(operation);
                return message;
            }
            else if (messageTypeInt == NinjaMessageEnum.FRAME)
            {
                List<Frame> frameList = new List<Frame>();
                //frame数量
                int frameCount = br.ReadInt32();
                for (int i = 0; i < frameCount; i++)
                {
                    int frameNo = br.ReadInt32();
                    BattleOperation p1Operation = ReadBattleOperation(br, frameNo);
                    BattleOperation p2Operation = ReadBattleOperation(br, frameNo);

                    Frame frame = new Frame(frameNo, new List<BattleOperation>() {p1Operation, p2Operation});
                    frameList.Add(frame);
                }
                NinjaFrameMessage message = new NinjaFrameMessage(frameList);
                return message;
            }
            return null;
        }

        private static BattleOperation ReadBattleOperation(BinaryReader br, int frameNo)
        {
            BattleOperation operation;
            int playerId = br.ReadInt32();
            int opType = br.ReadInt32();
            if (opType == OperationTypeEnum.MOVE)
            {
                long positionX = br.ReadInt64();
                long positionZ = br.ReadInt64();
                int direction = br.ReadInt32();
                operation = new BattleOperation(frameNo, playerId, opType,
                    direction, FVector3.Original(positionX, 0, positionZ));
            }
            else
            {
                int direction = br.ReadInt32();
                operation = new BattleOperation(frameNo, playerId, opType, direction);
            }

            return operation;
        }
    }
}