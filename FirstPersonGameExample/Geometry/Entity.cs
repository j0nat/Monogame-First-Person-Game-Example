using Microsoft.Xna.Framework;

namespace FirstPersonGameExample.Geometry
{
    class Entity
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public Vector3 Rotation { get; set; }
        public float Angle { get; set; }

        public Entity(Vector3 position, Vector3 size, Vector3 rotation, float angle)
        {
            this.Position = position;
            this.Size = size;
            this.Rotation = rotation;
            this.Angle = angle;
        }
    }
}
