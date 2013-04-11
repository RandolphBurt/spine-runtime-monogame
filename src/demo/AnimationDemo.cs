/// <summary>
/// Animation demo.
/// 2013-March
/// </summary>
namespace Demo
{
	using Spine;
	using System.Collections.Generic;
	using System.IO;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;

	public class AnimationDemo : Game
	{
		private readonly GraphicsDeviceManager graphicsDeviceManager;
		private Texture2D lineTexture;
		private List<Texture2D> textureMaps;
		private SpriteBatch spriteBatch;
		private SkeletonRenderer skeletonRenderer;
		private Skeleton skeleton;
		private Animation animationWalk;
		private Animation animationJump;
		private int animation = 0;
		private float timer = 1;

		public AnimationDemo ()
		{
			this.graphicsDeviceManager = new GraphicsDeviceManager (this);
			this.graphicsDeviceManager.IsFullScreen = true;

			this.graphicsDeviceManager.SupportedOrientations = 
				DisplayOrientation.LandscapeLeft | 
				DisplayOrientation.LandscapeRight |
				DisplayOrientation.PortraitDown | 
				DisplayOrientation.Portrait;

			this.textureMaps = new List<Texture2D> ();

			this.Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			TouchPanel.EnabledGestures = GestureType.Tap;
			
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
			Atlas atlas = new Atlas(GraphicsDevice, "Content/crab.atlas");
			SkeletonJson json = new SkeletonJson(atlas);
			skeleton = new Skeleton(json.readSkeletonData("crab", File.ReadAllText("Content/crab-skeleton.json")));
			this.animationWalk = skeleton.Data.FindAnimation ("WalkLeft");
			this.animationJump = skeleton.Data.FindAnimation ("Jump");

			this.animation = 0;
			this.SetSkeletonStartPosition ();

			// used for debugging - draw the bones
			this.lineTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			this.lineTexture.SetData(new[]{Color.White});
			this.textureMaps.Add(lineTexture);

			base.LoadContent ();
		}

		protected override void Draw (GameTime gameTime)
		{
			this.GraphicsDevice.Clear (Color.CornflowerBlue);

			timer += gameTime.ElapsedGameTime.Milliseconds / 1000f;

			if (animation == 0)
			{
				this.animationJump.Apply (this.skeleton, timer, true);
			}
			else
			{
				this.animationWalk.Apply (this.skeleton, timer, true);
			}

			this.skeleton.UpdateWorldTransform ();
			this.skeletonRenderer.Begin();
			//this.skeleton.DrawDebug(this.spriteBatch, this.lineTexture);
			this.skeletonRenderer.Draw(this.skeleton);
			this.skeletonRenderer.End();

			base.Draw (gameTime);
		}

		private void SetSkeletonStartPosition ()
		{
			this.skeleton.RootBone.X = 500;
			this.skeleton.RootBone.Y = 700;
			
			this.skeleton.UpdateWorldTransform ();
		}

		// Switch animations whenever the user touches the screen
		protected override void Update (GameTime gameTime)
		{
			if (TouchPanel.IsGestureAvailable)
			{
				while (TouchPanel.IsGestureAvailable)
				{
					// swallow all touches 
					TouchPanel.ReadGesture ();
				}
				
				if (++animation > 1)
				{
					animation = 0;
				}
				
				this.SetSkeletonStartPosition ();
			}
			
			base.Update (gameTime);
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

