using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstPersonGameExample.Geometry
{
    class Mesh
    {
        private Entity entity;
        private CustomVertexFormat[] vertices;
        private VertexBuffer vertexBuffer;
        private GraphicsDevice graphicsDevice;

        public Mesh(GraphicsDevice graphicsDevice, List<CustomVertexFormat> vertices, Entity entity)
        {
            this.graphicsDevice = graphicsDevice;
            this.entity = entity;

            SetVertexBuffer(vertices);
        }

        public Vector3 GetSize()
        {
            return entity.Size;
        }

        public Vector3 GetPosition()
        {
            return entity.Position;
        }

        private void SetVertexBuffer(List<CustomVertexFormat> listVertex)
        {
            vertexBuffer = new VertexBuffer(graphicsDevice, CustomVertexFormat.VertexDeclaration, listVertex.Count, BufferUsage.WriteOnly);

            vertexBuffer.SetData<CustomVertexFormat>(listVertex.ToArray());

            vertices = listVertex.ToArray();
        }

        public void DrawMesh(Effect effect, Camera camera, Texture2D texture)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(entity.Rotation, MathHelper.ToRadians(entity.Angle));
            Matrix transform = rotation * Matrix.CreateTranslation(entity.Position);

            effect.Parameters["World"].SetValue(transform);
            effect.Parameters["WorldViewProj"].SetValue(transform * camera.View * camera.Projection);
            effect.Parameters["CameraPosition"].SetValue(camera.Position);
            effect.Parameters["DiffuseTexture"].SetValue(texture);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
            }
        }
    }
}
