using Microsoft.Xna.Framework;

namespace FirstPersonGameExample
{
    class Camera : GameComponent
    {
        private Vector3 cameraLookAt;
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Matrix Projection { get; protected set; }
        public Matrix View { get { return Matrix.CreateLookAt(Position, cameraLookAt, Vector3.Up); } }

        public Camera(Game game) : base(game)
        {
            this.cameraLookAt = Vector3.Zero;
            this.Position = Vector3.Zero;
            this.Rotation = Vector3.Zero;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f, 1000.0f);
        }

        private void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);

            cameraLookAt = Position + lookAtOffset;
        }

        public Vector3 PreviewMove(Vector3 movement)
        {
            Matrix rotate = Matrix.CreateRotationY(Rotation.Y);

            movement = Vector3.Transform(movement, rotate);

            return Position + movement;
        }

        public void Move(Vector3 position)
        {
            Position = position;

            UpdateLookAt();
        }

        public void SetPosition(Vector3 position)
        {
            Vector3 cameraView = PreviewMove(position);

            Move(cameraView);
        }

        public void SetRotation(Vector3 rotation)
        {
            this.Rotation = rotation;

            UpdateLookAt();
        }
    }
}