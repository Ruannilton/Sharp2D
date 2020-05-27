using OpenTK;

namespace LibNet.Sharp2D
{
    public struct RectPosition
    {
        public Vector2 size;
        public Vector3 position;

        public RectPosition(Vector3 position, Vector2 size)
        {
            this.size = size;
            this.position = position;
        }
        public RectPosition(Vector2 position, Vector2 size)
        {
            this.size = size;
            this.position = new Vector3(position);
        }

        public static bool Inside(RectPosition a, Vector2 b)
        {

            if (a.position.X <= b.X && b.X < a.position.X + a.size.X)
            {
                if (a.position.Y <= b.Y && b.Y < a.position.Y + a.size.Y)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool Hit(RectPosition a, RectPosition b)
        {

            if (
                RectPosition.Inside(a, b.position.Xy)
             || RectPosition.Inside(a, b.position.Xy + new Vector2(b.size.X, 0))
             || RectPosition.Inside(a, b.position.Xy + new Vector2(0, b.size.Y))
             || RectPosition.Inside(a, b.position.Xy + b.size))
                return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public static bool operator ==(RectPosition left, RectPosition right) => (left.position, left.size) == (right.position, right.size);
        public static bool operator !=(RectPosition left, RectPosition right) => (left.position, left.size) != (right.position, right.size);

        public static bool operator >=(RectPosition left, RectPosition right)
        {
            if (left == right) return true;
            return left > right;
        }
        public static bool operator <=(RectPosition left, RectPosition right)
        {
            if (left == right) return true;
            return left < right;
        }
        public static bool operator >(RectPosition left, RectPosition right)
        {

            if (right.position.X > left.position.X && right.position.X < left.position.X + left.size.X)
                if (right.position.Y < left.position.Y && right.position.Y > left.position.Y - left.size.Y)
                    return true;

            return false;
        }
        public static bool operator <(RectPosition left, RectPosition right)
        {
            if (left.position.X > right.position.X && left.position.X < right.position.X + right.size.X)
                if (left.position.Y < right.position.Y && left.position.Y > right.position.Y - right.size.Y)
                    return true;

            return false;
        }


    }
}