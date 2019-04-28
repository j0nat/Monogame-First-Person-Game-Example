using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstPersonGameExample.Geometry
{
    public struct CustomVertexFormat : IVertexType
    {
        private Vector3 position;
        private Vector2 texCoord;
        private Vector3 normal;

        public CustomVertexFormat(Vector3 position, Vector2 texCoord, Vector3 normal)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.normal = normal;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * (3 + 2), VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }
    }
}