using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FirstPersonGameExample.Geometry;

namespace FirstPersonGameExample
{
    class PlayerController : GameComponent
    {
        private Camera camera;
        private List<Block> worldMeshes;
        private MouseState previousMouseState;
        private Vector3 mouseRotationBuffer;

        private float playerSpeed;
        private float playerHeight;

        public PlayerController(Game game, Camera camera, List<Block> worldMeshes) : base(game)
        {
            this.camera = camera;
            this.worldMeshes = worldMeshes;
            this.playerSpeed = 225f;
            this.playerHeight = 69;
            this.mouseRotationBuffer = Vector3.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();
            Vector3 moveVector = Vector3.Zero;
            bool isPlayerGrounded = IsPlayerGrounded();

            if (ks.IsKeyDown(Keys.W))
                moveVector.Z = 1;

            if (ks.IsKeyDown(Keys.S))
                moveVector.Z = -1;

            if (ks.IsKeyDown(Keys.A))
                moveVector.X = 1;

            if (ks.IsKeyDown(Keys.D))
                moveVector.X = -1;

            if (!isPlayerGrounded)
                moveVector.Y = -2; // falling

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();

                moveVector *= (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;

                Vector3 location = camera.PreviewMove(moveVector);
                CharacterMove(location);
            }

            float deltaX;
            float deltaY;
            if (ms != previousMouseState)
            {
                deltaX = ms.X - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = ms.Y - (Game.GraphicsDevice.Viewport.Height / 2);

                mouseRotationBuffer.X -= 0.1f * deltaX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mouseRotationBuffer.Y -= 0.1f * deltaY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (mouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                }

                if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                }

                camera.SetRotation(new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    MathHelper.WrapAngle(mouseRotationBuffer.X), 0));
            }

            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);

            previousMouseState = ms;

            base.Update(gameTime);
        }

        public void SetPosition(Vector3 position)
        {
            camera.SetPosition(position);
        }

        public void SetRotation(Vector3 rotation)
        {
            mouseRotationBuffer = new Vector3(
                MathHelper.ToRadians(rotation.X),
                MathHelper.ToRadians(rotation.Y),
                MathHelper.ToRadians(rotation.Z));
        }

        private void CharacterMove(Vector3 newPlayerPosition)
        {
            bool movePlayer = false;
            bool movePlayerUpwards = false;

            foreach (Block mesh in worldMeshes)
            {
                Mesh meshTop = mesh.GetTopMesh();

                Vector3 meshPosition = meshTop.GetPosition();
                Vector3 meshSize = meshTop.GetSize();

                // Is mesh within XZ range of player
                if (newPlayerPosition.X >= (meshPosition.X) && newPlayerPosition.X <= (meshPosition.X + meshSize.X)
                    && newPlayerPosition.Z >= (meshPosition.Z) && newPlayerPosition.Z <= (meshPosition.Z + meshSize.Z))
                {
                    float meshTopHeightPosition = meshPosition.Y + meshSize.Y;
                    float meshPlayerHeightDifference = Math.Abs(meshPosition.Y - newPlayerPosition.Y);

                    // Is mesh below or above the player
                    if (meshTopHeightPosition < newPlayerPosition.Y)
                    {
                        // Is the mesh height difference small enough for the
                        // player to step up on it.
                        if ((meshPlayerHeightDifference) <= playerHeight)
                        {
                            movePlayerUpwards = true;
                            movePlayer = true;

                            newPlayerPosition.Y = meshPosition.Y + playerHeight;
                            break;
                        }
                        else
                        {
                            // The mesh is below the player so the player can walk past it.
                            movePlayer = true;
                        }
                    }
                    else
                    {
                        if (newPlayerPosition.Y > meshPosition.Y &&
                             newPlayerPosition.Y < meshTopHeightPosition)
                        {
                            // Player is walking into a wall
                            movePlayer = false;
                            break;
                        }
                    }
                }
            }

            // If the player is moving upwards then we need to re-check the
            // meshes for roof collisions
            if (movePlayer && movePlayerUpwards)
            {
                foreach (Block mesh in worldMeshes)
                {
                    Mesh meshTop = mesh.GetTopMesh();

                    Vector3 meshPosition = meshTop.GetPosition();
                    Vector3 meshSize = meshTop.GetSize();

                    // Is mesh within XZ range of player
                    if (newPlayerPosition.X >= (meshPosition.X) && newPlayerPosition.X <= (meshPosition.X + meshSize.X)
                        && newPlayerPosition.Z >= (meshPosition.Z) && newPlayerPosition.Z <= (meshPosition.Z + meshSize.Z))
                    {
                        float meshBottomHeightDifference = Math.Abs(newPlayerPosition.Y - meshPosition.Y);

                        // Is the distance between the player and the roof X amount
                        if (meshPosition.Y >= newPlayerPosition.Y && meshBottomHeightDifference <= 5)
                        {
                            // Player is walking into the roof
                            movePlayer = false;
                            break;
                        }
                    }
                }
            }

            if (movePlayer)
            {
                camera.Move(newPlayerPosition);
            }
        }

        private bool IsPlayerGrounded()
        {
            bool isPlayerGrounded = false;

            foreach (Block mesh in worldMeshes)
            {
                Mesh meshTop = mesh.GetTopMesh();

                Vector3 meshPosition = meshTop.GetPosition();
                Vector3 meshSize = meshTop.GetSize();
                Vector3 playerPosition = camera.Position;

                // Is mesh within XZ range of player
                if (playerPosition.X >= (meshPosition.X) && playerPosition.X <= (meshPosition.X + meshSize.X)
                    && playerPosition.Z >= (meshPosition.Z) && playerPosition.Z <= (meshPosition.Z + meshSize.Z))
                {
                    float meshTopHeightPosition = (meshPosition.Y + meshSize.Y);
                    float playerDistanceToMesh = Math.Abs(meshTopHeightPosition - playerPosition.Y);

                    if (playerDistanceToMesh <= playerHeight &&
                        meshTopHeightPosition < playerPosition.Y)
                    {
                        isPlayerGrounded = true;
                        break;
                    }
                }
            }

            return isPlayerGrounded;
        }
    }
}