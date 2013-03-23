/// <summary>
/// Animation demo.
/// 2013-March
/// </summary>
namespace Demo
{
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using TexturePacker.Import;
	using Spine.Runtime.MonoGame;
	using Spine.Runtime.MonoGame.Attachments;
	using Spine.Runtime.MonoGame.Json;

	public class AnimationDemo : Game
	{
		private readonly GraphicsDeviceManager graphicsDeviceManager;

		private List<Texture2D> textureMaps;

		private SpriteBatch spriteBatch;

		private Skeleton skeleton;
		private Animation animationWalk;
		private Animation animationJump;


		public AnimationDemo ()
		{
			this.graphicsDeviceManager = new GraphicsDeviceManager(this);
			this.graphicsDeviceManager.IsFullScreen = true;

			this.textureMaps = new List<Texture2D> ();

			this.Content.RootDirectory = "Content";
		}

		protected override void LoadContent ()
		{
			this.spriteBatch = new SpriteBatch (this.graphicsDeviceManager.GraphicsDevice);

			var crabTextureMap = Content.Load<Texture2D>("crab.png");
			this.textureMaps.Add (crabTextureMap);

			var texturePackerReader = new TexturePackerReader ();			
			var spriteSheet = texturePackerReader.CreateSpriteSheet("Content/crab.json", crabTextureMap);

			// TODO
			var skeletonReader = new SkeletonJsonReader(new TextureAtlasAttachmentLoader( new TextureAtlas()));
			this.skeleton = skeletonReader.ReadSkeletonJsonFile("Content/skeleton-skeleton.json");

			var animationReader= new AnimationJsonReader();
			this.animationWalk = animationReader.ReadAnimationJsonFile("Content/skeleton-WalkLeft.json", skeleton.getData());
			this.animationJump = animationReader.ReadAnimationJsonFile("Content/skeleton-Jump.json", skeleton.getData());

			base.LoadContent ();
		}

		protected override void Draw (GameTime gameTime)
		{
			this.spriteBatch.Begin ();

			this.spriteBatch.Draw (this.textureMaps [0], new Vector2 (0, 0), Color.White);

			this.skeleton.draw(this.spriteBatch);

			this.spriteBatch.End ();

			base.Draw (gameTime);
		}

		protected override void Dispose (bool disposing)
		{
			if (this.textureMaps != null)
			{
				foreach (var textureMap in this.textureMaps)
				{
					if (textureMap != null && !textureMap.IsDisposed)
					{
						textureMap.Dispose ();
					}
				}
			
				this.textureMaps = null;
			}

			base.Dispose (disposing);
		}
	}
}

