/// <summary>
/// Animation demo.
/// 2013-March
/// </summary>
namespace Demo
{
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Spine.Runtime.MonoGame;
	using Spine.Runtime.MonoGame.Attachments;
	using Spine.Runtime.MonoGame.Graphics;
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

			this.graphicsDeviceManager.SupportedOrientations = 
				DisplayOrientation.LandscapeLeft | 
				DisplayOrientation.LandscapeRight |
				DisplayOrientation.PortraitDown| 
				DisplayOrientation.Portrait;

			this.textureMaps = new List<Texture2D> ();

			this.Content.RootDirectory = "Content";
		}

		protected override void LoadContent ()
		{
			this.spriteBatch = new SpriteBatch (this.graphicsDeviceManager.GraphicsDevice);

			var crabTextureMap = Content.Load<Texture2D>("crab.png");
			this.textureMaps.Add (crabTextureMap);

			var texturePackerReader = new TextureMapJsonReader ();			
			var textureAtlas = texturePackerReader.ReadTextureJsonFile("Content/crab.json", crabTextureMap);

			var skeletonReader = new SkeletonJsonReader(new TextureAtlasAttachmentLoader( textureAtlas));
			this.skeleton = skeletonReader.ReadSkeletonJsonFile("Content/skeleton-skeleton.json");
			this.skeleton.FlipY = true;

			var animationReader= new AnimationJsonReader();
			this.animationWalk = animationReader.ReadAnimationJsonFile("Content/skeleton-WalkLeft.json", skeleton.Data);
			this.animationJump = animationReader.ReadAnimationJsonFile("Content/skeleton-Jump.json", skeleton.Data);

			Bone root = skeleton.GetRootBone();
			root.X = 500;
			root.Y = 700;
		
			skeleton.UpdateWorldTransform();

			base.LoadContent ();
		}

		private float timer = 1;

		protected override void Draw (GameTime gameTime)
		{
			this.spriteBatch.Begin ();

			this.GraphicsDevice.Clear (Color.CornflowerBlue);

			// this.spriteBatch.Draw (this.textureMaps [0], new Vector2 (0, 0), Color.White);

			this.animationWalk.Apply(this.skeleton, timer++ / 100, true);
			// this.animationJump.Apply(this.skeleton, timer++ / 100, true);

			this.skeleton.UpdateWorldTransform();
			this.skeleton.Draw(this.spriteBatch);

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

