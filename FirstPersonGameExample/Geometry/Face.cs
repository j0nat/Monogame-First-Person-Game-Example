using Microsoft.Xna.Framework;

namespace FirstPersonGameExample.Geometry
{
    class Face
    {
        public Vector3 P1 { get; set; }
        public Vector3 P2 { get; set; }
        public Vector3 P3 { get; set; } 
        public string TextureName { get; set; }

        public Face(Vector3 p1, Vector3 p2, Vector3 p3, string textureName)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
            this.TextureName = textureName;
        }
    }
}
