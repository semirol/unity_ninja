namespace rocker
{
    public class RockerScrollCircle :ScrollCircle
    {
        protected override float MRadiusScale => 0.3f;

        // public GameObject content;

        public float GetAxis(string axis)
        {
            if (axis == "Horizontal") return MoveRate.x;
            if (axis == "Vertical") return MoveRate.y;
            return 0f;
        }
    }
}
