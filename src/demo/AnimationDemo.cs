/// <summary>
/// Animation demo.
/// 2013-March
/// </summary>
namespace Demo
{
	using Spine;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;

	public class AnimationDemo : Game
	{
		private readonly GraphicsDeviceManager graphicsDeviceManager;
		private Texture2D lineTexture;
		private List<Texture2D> textureMaps;
		private List<Atlas> atlasList;

		private SpriteBatch spriteBatch;
		private SkeletonRenderer skeletonRenderer;

		private Skeleton spinosaurusSkeleton;
		private Animation spinosaurusAnimation;

		private Skeleton crabSkeleton;
		private Animation crabAnimationWalk;
		private Animation crabAnimationJump;

		private Skeleton dragonSkeleton;
		private Animation dragonAnimationFly;

		private Skeleton spineBoySkeleton;
		private Animation spineBoyAnimationWalk;
		private Animation spineBoyAnimationJump;

		private Skeleton goblinSkeleton;
		private Animation goblinAnimationWalk;

		private int currentAnimation = 0;
		
		private float lastTimer = 0;
		private float timer = 0;

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
			this.atlasList = new List<Atlas>();

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

			this.crabSkeleton = this.LoadSkeleton("crab.png", "crab.atlas", "crab.json");
			this.crabAnimationWalk = this.crabSkeleton.Data.FindAnimation ("WalkLeft");
			this.crabAnimationJump = this.crabSkeleton.Data.FindAnimation ("Jump");

			this.spinosaurusSkeleton = this.LoadSkeleton("spinosaurus.png", "spinosaurus.atlas", "spinosaurus.json");
			this.spinosaurusAnimation = this.spinosaurusSkeleton.Data.FindAnimation ("animation");
			
			this.dragonSkeleton = this.LoadSkeleton("dragon.png", "dragon.atlas", "dragon.json");
			this.dragonAnimationFly = this.dragonSkeleton.Data.FindAnimation("flying");

			this.goblinSkeleton = this.LoadSkeleton("goblin.png", "goblin.atlas", "goblin.json");
			this.goblinAnimationWalk = this.goblinSkeleton.Data.FindAnimation("walk");

			this.spineBoySkeleton = this.LoadSkeleton("spineboy.png", "spineboy.atlas", "spineboy.json");
			this.spineBoyAnimationWalk = this.spineBoySkeleton.Data.FindAnimation("walk");
			this.spineBoyAnimationJump = this.spineBoySkeleton.Data.FindAnimation("jump");

			skeletonRenderer = new SkeletonRenderer(GraphicsDevice);

			this.currentAnimation = 0;
			this.SetSkeletonStartPosition ();

			this.lineTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			this.lineTexture.SetData(new[]{Color.White});
			this.textureMaps.Add(lineTexture);

			base.LoadContent ();
		}

		private Skeleton LoadSkeleton(string textureGraphicsFile, string textureAtlasFile, string skeletonJsonFile)
		{
			Atlas atlas = new Atlas("Content/" + textureAtlasFile, new XnaTextureLoader(GraphicsDevice));
			this.atlasList.Add(atlas);

			SkeletonJson json = new SkeletonJson(atlas);
			var skeleton = new Skeleton(json.ReadSkeletonData("Content/" + skeletonJsonFile));

			// Without this the skin attachments won't be attached. See SetSkin.
			skeleton.SetSlotsToSetupPose(); 

			return skeleton;
		}

		protected override void Draw (GameTime gameTime)
		{
			this.GraphicsDevice.Clear (Color.CornflowerBlue);

			lastTimer = timer;
			timer += gameTime.ElapsedGameTime.Milliseconds / 1000f;

			switch (currentAnimation)
			{
				case 0:
					this.Animate(this.spinosaurusSkeleton, this.spinosaurusAnimation);
					break;
					
				case 1:
					this.Animate(this.spineBoySkeleton, this.spineBoyAnimationWalk);
					break;
					
				case 2:
					this.Animate(this.spineBoySkeleton, this.spineBoyAnimationJump);
					break;

				case 3:
					this.Animate(this.dragonSkeleton, this.dragonAnimationFly);
					break;

				case 4:
				case 5:
					this.Animate(this.goblinSkeleton, this.goblinAnimationWalk);
					break;

				case 6:
					this.Animate(this.crabSkeleton, this.crabAnimationJump);
					break;

				case 7:
					this.Animate(this.crabSkeleton, this.crabAnimationWalk);
					break;
			}

			base.Draw (gameTime);
		}

		// Switch animations whenever the user touches the screen

		protected override void Update (GameTime gameTime)
		{
			if (TouchPanel.IsGestureAvailable)
			{
				while (TouchPanel.IsGestureAvailable)
				{
					// swallow all touches 
					TouchPanel.ReadGesture();
				}

				if (++currentAnimation > 7)
				{
					currentAnimation = 0;
				}

				this.SetSkeletonStartPosition();
			}

			base.Update (gameTime);
		}

		private void SetSkeletonStartPosition()
		{
			int x = 0;
			int y = 0;
			Skeleton skeleton = null;

			switch (currentAnimation)
			{
				case 0:
					skeleton = this.spinosaurusSkeleton;
					x = 500;
					y = 900;
					break;

				case 1:
				case 2:
					skeleton = this.spineBoySkeleton;
					x = 400;
					y = 600;
					break;

				case 3:
					skeleton = this.dragonSkeleton;
					x = 500;
					y = 650;
					break;

				case 4:
					skeleton = this.goblinSkeleton;
					skeleton.SetSkin("goblingirl");
					x = 420;
					y = 540;
					break;

				case 5:
					skeleton = this.goblinSkeleton;
					skeleton.SetSkin("goblin");
					x = 420;
					y = 540;
					break;

				case 6:
				case 7:
					skeleton = this.crabSkeleton;
					x = 500;
					y = 750;
					break;
			}

			skeleton.SetToSetupPose();

			skeleton.X = x;
			skeleton.Y = y;

			skeleton.UpdateWorldTransform();
		}

		private void Animate(Skeleton skeleton, Animation animation)
		{	
			animation.Apply (skeleton, lastTimer, timer, true, null);

			skeleton.UpdateWorldTransform ();
			this.skeletonRenderer.Begin();
			//this.skeleton.DrawDebug(this.spriteBatch, this.lineTexture);
			this.skeletonRenderer.Draw(skeleton);
			this.skeletonRenderer.End();
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

			if (this.atlasList != null)
			{
				foreach (var atlas in this.atlasList)
				{
					if (atlas != null)
					{
						atlas.Dispose ();
					}
				}
				
				this.atlasList = null;
			}

			base.Dispose (disposing);
		}
	}
}

