using Microsoft.Xna.Framework;

namespace FirstPersonGameExample.Geometry
{
    class BlockBrush
    {
        public Face FaceDataTop { get; set; }
        public Face FaceDataBottom { get; set; }
        public Face FaceDataFront { get; set; }
        public Face FaceDataBack { get; set; }
        public Face FaceDataRight { get; set; }
        public Face FaceDataLeft { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }

        public BlockBrush(Face faceDataTop, Face faceDataBottom, Face faceDataFront, Face faceDataBack,
            Face faceDataRight, Face faceDataLeft, Vector3 position, Vector3 size)
        {
            this.FaceDataTop = faceDataTop;
            this.FaceDataBottom = faceDataBottom;
            this.FaceDataFront = faceDataFront;
            this.FaceDataBack = faceDataBack;
            this.FaceDataRight = faceDataRight;
            this.FaceDataLeft = faceDataLeft;
            this.Position = position;
            this.Size = size;
        }
    }
}
