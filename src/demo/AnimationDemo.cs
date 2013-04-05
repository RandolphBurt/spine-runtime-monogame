/// <summary>
/// Animation demo.
/// 2013-March
/// </summary>
namespace Demo
{
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;
	using Spine.Runtime.MonoGame;
	using Spine.Runtime.MonoGame.Attachments;
	using Spine.Runtime.MonoGame.Graphics;
	using Spine.Runtime.MonoGame.Json;

	public class AnimationDemo : Game
	{
		private readonly GraphicsDeviceManager graphicsDeviceManager;
		private Texture2D lineTexture;
		private List<Texture2D> textureMaps;
		private SpriteBatch spriteBatch;
		private Skeleton skeleton;
		private Animation animationWalk;
		private Animation animationJump;
		private int animation = 0;
		private float timer = 1;

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

		protected override void Initialize ()
		{
			TouchPanel.EnabledGestures = GestureType.Tap;
			
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			this.spriteBatch = new SpriteBatch (this.graphicsDeviceManager.GraphicsDevice);
			
			var crabTextureMap = Content.Load<Texture2D> ("crab.png");
			this.textureMaps.Add (crabTextureMap);
			
			var texturePackerReader = new TextureMapJsonReader ();			
			var textureAtlas = texturePackerReader.ReadTextureJsonFile ("Content/crab.json", crabTextureMap);
			
			var skeletonReader = new SkeletonJsonReader (new TextureAtlasAttachmentLoader (textureAtlas));
			this.skeleton = skeletonReader.ReadSkeletonJsonFile ("Content/crab-skeleton.json");
			this.skeleton.setFlipY(true);
			
			this.animationWalk = skeleton.getData().FindAnimation ("WalkLeft");
			this.animationJump = skeleton.getData().FindAnimation ("Jump");
			
			this.animation = 0;
			this.SetSkeletonStartPosition ();

			base.LoadContent ();
		}
	

		protected override void Draw (GameTime gameTime)
		{
			this.spriteBatch.Begin ();

			this.GraphicsDevice.Clear (Color.CornflowerBlue);

			if (animation == 0)
			{
				this.Animate (this.animationJump);
			}
			else
			{
				this.Animate (this.animationWalk);
				
			}

			this.spriteBatch.End ();

			base.Draw (gameTime);
		}

		private void Animate (Animation animation)
		{
			// In reality you'd use the gameTime to calculate the animation but this is for demonstration purposes.
			// Also you'd probably do the calculations in Update and not Draw
			animation.apply (this.skeleton, timer++ / 100, true);
			
			this.skeleton.updateWorldTransform ();
			this.skeleton.draw (this.spriteBatch);
			//this.skeleton.DrawDebug(this.spriteBatch, this.lineTexture);
		}
		
		private void SetSkeletonStartPosition ()
		{
			this.skeleton.setToBindPose ();
			Bone root = skeleton.getRootBone ();
			root.setX(500);
			root.setY (700);
			
			this.skeleton.updateWorldTransform ();
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

