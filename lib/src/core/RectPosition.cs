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
    }
}