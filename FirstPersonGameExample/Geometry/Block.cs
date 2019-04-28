using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstPersonGameExample.Geometry
{
    class Block
    {
        private GraphicsDevice graphicsDevice;
        private Entity entity;
        private Texture2D textureFront, textureBack, textureLeft, textureRight, textureTop, textureBottom;
        private Mesh meshFront, meshBack, meshLeft, meshRight, meshTop, meshBottom;
        private bool textureRepeatFront, textureRepeatBack, textureRepeatLeft, textureRepeatRight, textureRepeatTop, textureRepeatBottom;
        private enum plane { front, back, left, right, top, bottom };

        public Block(GraphicsDevice graphicsDevice, Entity entity, Texture2D texture)
        {
            this.graphicsDevice = graphicsDevice;
            this.entity = entity;
            this.textureFront = texture;
            this.textureBack = texture;
            this.textureLeft = texture;
            this.textureRight = texture;
            this.textureTop = texture;
            this.textureBottom = texture;
            this.textureRepeatFront = true;
            this.textureRepeatBack = true;
            this.textureRepeatLeft = true;
            this.textureRepeatRight = true;
            this.textureRepeatTop = true;
            this.textureRepeatBottom = true;

            CreateBlock();
        }

        public Block(GraphicsDevice graphicsDevice, Entity entity, Texture2D textureFront, 
            Texture2D textureBack, Texture2D textureLeft, Texture2D textureRight,
            Texture2D textureTop, Texture2D textureBottom)
        {
            this.graphicsDevice = graphicsDevice;
            this.entity = entity;
            this.textureFront = textureFront;
            this.textureBack = textureBack;
            this.textureLeft = textureLeft;
            this.textureRight = textureRight;
            this.textureTop = textureTop;
            this.textureBottom = textureBottom;
            this.textureRepeatFront = true;
            this.textureRepeatBack = true;
            this.textureRepeatLeft = true;
            this.textureRepeatRight = true;
            this.textureRepeatTop = true;
            this.textureRepeatBottom = true;

            CreateBlock();
        }

        public Block(GraphicsDevice graphicsDevice, Entity entity, Texture2D textureFront,
            Texture2D textureBack, Texture2D textureLeft, Texture2D textureRight,
            Texture2D textureTop, Texture2D textureBottom, bool textureRepeatFront,
            bool textureRepeatBack, bool textureRepeatLeft, bool textureRepeatRight,
            bool textureRepeatTop, bool textureRepeatBottom)
        {
            this.graphicsDevice = graphicsDevice;
            this.entity = entity;
            this.textureFront = textureFront;
            this.textureBack = textureBack;
            this.textureLeft = textureLeft;
            this.textureRight = textureRight;
            this.textureTop = textureTop;
            this.textureBottom = textureBottom;
            this.textureRepeatFront = textureRepeatFront;
            this.textureRepeatBack = textureRepeatBack;
            this.textureRepeatLeft = textureRepeatLeft;
            this.textureRepeatRight = textureRepeatRight;
            this.textureRepeatTop = textureRepeatTop;
            this.textureRepeatBottom = textureRepeatBottom;

            CreateBlock();
        }

        private void CreateBlock()
        {
              // face FRONT
              List<CustomVertexFormat> verticesFront =
                  VerticalFace(new Vector3(entity.Size.X, 0, 0), new Vector3(0, entity.Size.Y, 0), textureFront, plane.front);
              meshFront = new Mesh(graphicsDevice, verticesFront, entity);

            // face BACK
            List<CustomVertexFormat> verticesBack =
                  VerticalFace(new Vector3(0, 0, entity.Size.Z), new Vector3(entity.Size.X, entity.Size.Y, entity.Size.Z), textureBack, plane.back);
              meshBack = new Mesh(graphicsDevice, verticesBack, entity);

            // face LEFT
            List<CustomVertexFormat> verticesLeft =
                VerticalFace(new Vector3(entity.Size.X, 0, entity.Size.Z), new Vector3(entity.Size.X, entity.Size.Y, 0), textureLeft, plane.left);
            meshLeft = new Mesh(graphicsDevice, verticesLeft, entity);

            // face RIGHT
            List<CustomVertexFormat> verticesRight =
                VerticalFace(new Vector3(0, 0, 0), new Vector3(0, entity.Size.Y, entity.Size.Z), textureRight, plane.right);
            meshRight = new Mesh(graphicsDevice, verticesRight, entity);

            // face TOP
            List<CustomVertexFormat> verticesTop =
                HorizontalFace(new Vector3(0, entity.Size.Y, 0), new Vector3(entity.Size.X, entity.Size.Y, entity.Size.Z), textureTop, plane.top);
            meshTop = new Mesh(graphicsDevice, verticesTop, entity);

            // face BOTTOM
            List<CustomVertexFormat> verticesBottom =
                HorizontalFace(new Vector3(0, 0, entity.Size.Z), new Vector3(entity.Size.X, 0, 0), textureBottom, plane.bottom);
            meshBottom = new Mesh(graphicsDevice, verticesBottom, entity);
        }

        private List<CustomVertexFormat> VerticalFace(Vector3 p1, Vector3 p2, Texture2D texture, plane plane)
        {
            List<CustomVertexFormat> vertices = new List<CustomVertexFormat>();

            Vector3 normal;
            GetNormal(out normal);

            float repeatTextureX;
            float repeatTextureY;
            GetTextureRepeat(out repeatTextureX, out repeatTextureY, texture, plane);

            vertices.Add(new CustomVertexFormat(new Vector3(p1.X, p1.Y, p1.Z), new Vector2(repeatTextureX, 0), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p1.X, p2.Y, p1.Z), new Vector2(repeatTextureX, repeatTextureY), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p2.X, p2.Y, p2.Z), new Vector2(0, repeatTextureY), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p2.X, p2.Y, p2.Z), new Vector2(0, repeatTextureY), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p2.X, p1.Y, p2.Z), new Vector2(0, 0), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p1.X, p1.Y, p1.Z), new Vector2(repeatTextureX, 0), normal));

            return vertices;
        }

        private List<CustomVertexFormat> HorizontalFace(Vector3 p1, Vector3 p2, Texture2D texture, plane plane)
        {
            List<CustomVertexFormat> vertices = new List<CustomVertexFormat>();
            
            Vector3 normal;
            GetNormal(out normal);

            float repeatTextureX;
            float repeatTextureY;
            GetTextureRepeat(out repeatTextureX, out repeatTextureY, texture, plane);

            vertices.Add(new CustomVertexFormat(new Vector3(p1.X, p1.Y, p1.Z), new Vector2(0, repeatTextureY), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p2.X, p1.Y, p1.Z), new Vector2(repeatTextureX, repeatTextureY), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p2.X, p2.Y, p2.Z), new Vector2(repeatTextureX, 0), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p1.X, p1.Y, p1.Z), new Vector2(0, repeatTextureY), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p2.X, p2.Y, p2.Z), new Vector2(repeatTextureX, 0), normal));
            vertices.Add(new CustomVertexFormat(new Vector3(p1.X, p1.Y, p2.Z), new Vector2(0, 0), normal));

            return vertices;
        }

        private void GetTextureRepeat(out float repeatTextureX, out float repeatTextureY, Texture2D texture, plane plane)
        {
            repeatTextureX = 1f;
            repeatTextureY = 1f;

            float textureRepeatMultiplier = 9f;

            if (textureRepeatBack && plane == plane.back || textureRepeatFront && plane == plane.front ||
                textureRepeatLeft && plane == plane.left || textureRepeatRight && plane == plane.right ||
                textureRepeatTop && plane == plane.top || textureRepeatBottom && plane == plane.bottom)
            {
                if (plane == plane.front || plane == plane.back)
                {
                    repeatTextureX = (entity.Size.X / texture.Width) * textureRepeatMultiplier;
                    repeatTextureY = (entity.Size.Y / texture.Height) * textureRepeatMultiplier;
                }
                else
                if (plane == plane.right || plane == plane.left)
                {
                    repeatTextureX = (entity.Size.Z / texture.Width) * textureRepeatMultiplier;
                    repeatTextureY = (entity.Size.Y / texture.Height) * textureRepeatMultiplier;
                }
                else
                if (plane == plane.top || plane == plane.bottom)
                {
                    repeatTextureX = (entity.Size.X / texture.Width) * textureRepeatMultiplier;
                    repeatTextureY = (entity.Size.Z / texture.Height) * textureRepeatMultiplier;
                }
            }
        }

        private void GetNormal(out Vector3 normal)
        {
            normal = new Vector3(0, 1, 0);

            if (entity.Rotation != Vector3.Zero)
            {
                // If the mesh is rotated then set
                // normal to -1, 0, 0 for lighting purposes
                normal = new Vector3(-1, 0, 0);
            }
        }

        public Mesh GetTopMesh()
        {
            return meshTop;
        }

        public void Draw(Effect effect, Camera camera)
        {
            meshFront.DrawMesh(effect, camera, textureFront);
            meshBack.DrawMesh(effect, camera, textureBack);
            meshLeft.DrawMesh(effect, camera, textureLeft);
            meshRight.DrawMesh(effect, camera, textureRight);
            meshTop.DrawMesh(effect, camera, textureTop);
            meshBottom.DrawMesh(effect, camera, textureBottom);
        }
    }
}
