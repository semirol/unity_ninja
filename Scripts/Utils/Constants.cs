namespace Utils
{
    public class Constants
    {
        // 传输层协议
        public static ProtocolType PROTOCOL_TYPE = ProtocolType.Udp;
        // 玩家数据同步方式
        public static SyncType SYNC_TYPE = SyncType.Frame;
        // 帧间隔
        public static int FRAME_INTERVAL = 66;
        // 帧频率
        public static int FRAME_FREQ = 15;
        // 玩家移动速度
        public static int PLAYER_MOVE_SPEED = 10;
        // 技能释放时距离玩家身前的距离
        public static int SKILL_OFFSET = 1;
        // EnergyWave 参数
        public static float ENERGY_WAVE_SPEED = 12f;
        public static float ENERGY_WAVE_DURING_TIME = 2f;
        // Dart 参数
        public static float DART_SPEED = 8f;
        public static float DART_DURING_TIME = 5f;
        public static float FIND_ENEMY_DISTANCE = 30f; // 初始自动寻敌最大范围
        // Shield 参数
        public static float SHIELD_DURING_TIME = 1f;
        // Blink 参数
        public static float BLINK_DISTANCE = 5f;

        public enum ProtocolType
        {
            Tcp,
            Udp
        }
        public enum SyncType
        {
            Frame,
            State
        }
    }
}