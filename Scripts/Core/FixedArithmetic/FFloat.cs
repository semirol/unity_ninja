using UnityEngine;

namespace Core.FixedArithmetic
{
    public class FFloat
    {
        public long Value;

        private FFloat()
        {
            
        }
        public FFloat(float f)
        {
            Value = (long)(f * (1 << 10));
        }

        public FFloat(int i)
        {
            Value = i << 10;
        }

        public static FFloat Original(long l)
        {
            FFloat ff = new FFloat();
            ff.Value = l;
            return ff;
        }

        public float ToFloat()
        {
            return (float)Value / (1 << 10);
        }
        
        public static FFloat operator+ (FFloat b, FFloat c)
        {
            return Original(b.Value + c.Value);
        }
    }

    public class FVector3
    {
        public FFloat X;
        public FFloat Y;
        public FFloat Z;
        
        public FVector3(Vector3 vector3)
        {
            X = new FFloat(vector3.x);
            Y = new FFloat(vector3.y);
            Z = new FFloat(vector3.z);
        }

        private FVector3()
        {
            
        }

        public static FVector3 Original(long x, long y, long z)
        {
            FVector3 fVector3 = new FVector3();
            fVector3.X = FFloat.Original(x);
            fVector3.Y = FFloat.Original(y);
            fVector3.Z = FFloat.Original(z);
            return fVector3;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X.ToFloat(), Y.ToFloat(), Z.ToFloat());
        }
        
        public static FVector3 operator+ (FVector3 b, FVector3 c)
        {
            FVector3 result = new FVector3();
            result.X = b.X + c.X;
            result.Y = b.Y + c.Y;
            result.Z = b.Z + c.Z;
            return result;
        }
    }

    public class FQuaternion
    {
        private FFloat _w;
        private FVector3 _xyz;
        public FQuaternion(Quaternion quaternion)
        {
            _w = new FFloat(quaternion.w);
            _xyz = new FVector3(new Vector3(quaternion.x, quaternion.y, quaternion.z));
        }

        public Quaternion ToQuaternion()
        {
            Vector3 xyz = _xyz.ToVector3();
            return new Quaternion(xyz.x, xyz.y, xyz.z, _w.ToFloat());
        }
    }
}