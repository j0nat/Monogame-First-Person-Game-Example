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
    class WorldRenderer : DrawableGameComponent
    {
        private List<Block> blocks;
        private Camera camera;
        private SpriteBatch spriteBatch;
        private Effect worldEffect;
        private PlayerController playerController;

        private Texture2D crosshairTexture;
        private RenderTarget2D crosshairRenderTarget;

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public WorldRenderer(Game game) : 
            base(game)
        {
            blocks = new List<Block>();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            worldEffect = Game.Content.Load<Effect>("BasicLightEffect");
            crosshairTexture = Game.Content.Load<Texture2D>("crosshair");
            crosshairRenderTarget = new RenderTarget2D(GraphicsDevice, crosshairTexture.Width, crosshairTexture.Height);

            camera = new Camera(Game);
            Game.Components.Add(camera);

            LoadMap(@"Content\game.map");

            playerController = new PlayerController(this.Game, camera, blocks);
            Game.Components.Add(playerController);

            playerController.SetPosition(new Vector3(0, 15, 10));
            playerController.SetRotation(new Vector3(180, 0, 0));

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            SetLighting();

            foreach (Block block in blocks)
            {
                block.Draw(worldEffect, camera);
            }

            spriteBatch.Begin();
            spriteBatch.Draw(crosshairTexture, new Rectangle(
                (GraphicsDevice.Viewport.Width / 2) - (crosshairTexture.Width / 2),
                (GraphicsDevice.Viewport.Height / 2) - (crosshairTexture.Height / 2), crosshairTexture.Width, crosshairTexture.Height), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadMap(string map)
        {
            blocks.Clear();

            List<BlockBrush> brushes = Map.Load(map);

            foreach (BlockBrush brush in brushes)
            {
                Texture2D textureFront, textureBack, textureLeft, textureRight, textureTop, textureBottom;
                
                textureTop = GetTexture(brush.FaceDataTop.TextureName);
                textureBottom = GetTexture(brush.FaceDataBottom.TextureName);
                textureFront = GetTexture(brush.FaceDataLeft.TextureName);  // These are switched (textureFront comes from left face)
                textureBack = GetTexture(brush.FaceDataRight.TextureName);  // Seems to be limited to the .map imports since
                textureRight = GetTexture(brush.FaceDataFront.TextureName); // normal blockbrush works fine.
                textureLeft = GetTexture(brush.FaceDataBack.TextureName);

                blocks.Add(new Block(GraphicsDevice, new Entity(brush.Position * 0.5f, brush.Size * 0.5f,
                    new Vector3(0, 0, 0), 0), textureFront, textureBack, textureLeft, textureRight, textureTop, textureBottom,
                    true, true, true, true, true, true));
            }
        }

        private void SetLighting()
        {
            worldEffect.Parameters["SunLightDirection"].SetValue(new Vector3(10, 2, 50));
            worldEffect.Parameters["SunLightColor"].SetValue(Color.White.ToVector3());
            worldEffect.Parameters["SunLightIntensity"].SetValue(0.3f);

            var _lightingEffectPointLightPosition = worldEffect.Parameters["PointLightPosition"];
            var _lightingEffectPointLightColor = worldEffect.Parameters["PointLightColor"];
            var _lightingEffectPointLightIntensity = worldEffect.Parameters["PointLightIntensity"];

            var _lightingEffectPointLightRadius = worldEffect.Parameters["PointLightRadius"];
            var _lightingEffectMaxLightsRendered = worldEffect.Parameters["MaxLightsRendered"];

            const int MaxLights = 3;

            Vector3[] lightPositions = new Vector3[MaxLights];
            Vector3[] lightColors = new Vector3[MaxLights];
            float[] lightIntensities = new float[MaxLights];
            float[] lightRadii = new float[MaxLights];

            lightPositions[0] = new Vector3(-25, 45, -15);
            lightColors[0] = Color.White.ToVector3();
            lightIntensities[0] =3f;
            lightRadii[0] = 360;

            lightPositions[1] = new Vector3(-25, 45, -325);
            lightColors[1] = Color.White.ToVector3();
            lightIntensities[1] = 5f;
            lightRadii[1] = 360;

            lightPositions[2] = camera.Position;
            lightColors[2] = Color.Purple.ToVector3();
            lightIntensities[2] = 5f;
            lightRadii[2] = 222;

            _lightingEffectMaxLightsRendered.SetValue(MaxLights);
            _lightingEffectPointLightPosition.SetValue(lightPositions);
            _lightingEffectPointLightColor.SetValue(lightColors);
            _lightingEffectPointLightIntensity.SetValue(lightIntensities);
            _lightingEffectPointLightRadius.SetValue(lightRadii);
        }

        private Texture2D GetTexture(string textureName)
        {
            if (textures.ContainsKey(textureName))
            {
                return textures[textureName];
            }
            else
            {
                Texture2D texture = FlipTexture(Game.Content.Load<Texture2D>(textureName));
                textures.Add(textureName, texture);
                return texture;
            }
        }

        private Texture2D LoadImage(string image)
        {
            return FlipTexture(Game.Content.Load<Texture2D>(image));
        }

        private Texture2D FlipTexture(Texture2D texture)
        {
            Texture2D returnTexture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);

            Color[] data = new Color[texture.Width * texture.Height];
            Color[] dataFlipped = new Color[data.Length];

            texture.GetData<Color>(data);

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    int index = texture.Width - 1 - x + (texture.Height - 1 - y) * texture.Width;

                    dataFlipped[x + y * texture.Width] = data[index];
                }
            }

            returnTexture.SetData<Color>(dataFlipped);

            return returnTexture;
        }
    }
}
