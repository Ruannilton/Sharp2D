using OpenTK;

namespace LibNet.Sharp2D
{
    /// <summary>
    /// Class that represent the position and the size of a square
    /// </summary>
    public struct RectPosition
    {
        /// <summary>
        /// Size of the square
        /// </summary>
        public Vector2 size;

        /// <summary>
        /// Position of the square
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// Create an instance of RectPosition
        /// </summary>
        /// <param name="position">Position of the square</param>
        /// <param name="size">Size of the square</param>
        public RectPosition(Vector3 position = default, Vector2 size = default)
        {
            this.size = size;
            this.position = position;
        }

        /// <summary>
        /// Create an instance of RectPosition
        /// </summary>
        /// <param name="position">Position of the square</param>
        /// <param name="size">Size of the square</param>
        public RectPosition(Vector2 position = default, Vector2 size = default)
        {
            this.size = size;
            this.position = new Vector3(position);
        }

        /// <summary>
        /// Create an instance of RectPosition
        /// </summary>
        /// <param name="x">Horizontal position of the square</param>
        /// <param name="y">Vertical position of the square</param>
        /// <param name="width">Horizontal size of the square</param>
        /// <param name="height">Vertical size of the square</param>
        public RectPosition(float x = 10, float y = 10, float width = 100, float height = 100)
        {
            this.position = new Vector3(x, y, 0);
            this.size = new Vector2(width, height);
        }


        /// <summary>
        /// Verify if the point is inside the squre area
        /// </summary>
        /// <param name="square"></param>
        /// <param name="point"></param>
        public static bool Inside(RectPosition square, Vector2 point)
        {

            if (square.position.X <= point.X && point.X < square.position.X + square.size.X)
            {
                if (square.position.Y <= point.Y && point.Y < square.position.Y + square.size.Y)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verify if two square are in contact
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Return this object in a string format
        /// </summary>
        public override string ToString()
        {
            return $"Position: X:{position.X} Y:{position.Y} Z:{position.Z} Size: X:{size.X} Y:{size.Y}";
        }

        /// <summary>
        /// Convert RecPosition to OpenTk.Matrix4
        /// </summary>
        public static implicit operator Matrix4(RectPosition rect) => Matrix4.CreateScale(rect.size.X, rect.size.Y, 1) * Matrix4.CreateTranslation(rect.position.X, rect.position.Y, rect.position.Z);

        /// <summary>
        /// Verify if two RectPositions are equals
        /// </summary>
        public static bool operator ==(RectPosition left, RectPosition right) => (left.position, left.size) == (right.position, right.size);

        /// <summary>
        /// Verify if two RectPositions aren´t equals
        /// </summary>
        public static bool operator !=(RectPosition left, RectPosition right) => (left.position, left.size) != (right.position, right.size);

        /// <summary>
        /// Verify if an RectPosition is inside or matching another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Return true if match or right is inside left</returns>
        public static bool operator >=(RectPosition left, RectPosition right)
        {
            if (left == right) return true;
            return left > right;
        }

        /// <summary>
        /// Verify if an RectPosition is inside or matching another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Return true if match or left is inside right</returns>
        public static bool operator <=(RectPosition left, RectPosition right)
        {
            if (left == right) return true;
            return left < right;
        }

        /// <summary>
        /// Verify if an RectPosition is inside another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Return true if right is inside left</returns>
        public static bool operator >(RectPosition left, RectPosition right)
        {

            if (right.position.X > left.position.X && right.position.X < left.position.X + left.size.X)
                if (right.position.Y < left.position.Y && right.position.Y > left.position.Y - left.size.Y)
                    return true;

            return false;
        }

        /// <summary>
        /// Verify if an RectPosition is inside another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Return true if left is inside right</returns>
        public static bool operator <(RectPosition left, RectPosition right)
        {
            if (left.position.X > right.position.X && left.position.X < right.position.X + right.size.X)
                if (left.position.Y < right.position.Y && left.position.Y > right.position.Y - right.size.Y)
                    return true;

            return false;
        }


    }
}