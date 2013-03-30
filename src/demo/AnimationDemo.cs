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
			this.spriteBatch = new SpriteBatch (this.graphicsDeviceManager.GraphicsDevice);

			var animationReader= new AnimationJsonReader();

			this.crabSkeleton = this.LoadSkeleton("crab.png", "crab.json", "crab-skeleton.json");
			this.crabAnimationWalk = animationReader.ReadAnimationJsonFile("Content/crab-WalkLeft.json", crabSkeleton.Data);
			this.crabAnimationJump = animationReader.ReadAnimationJsonFile("Content/crab-Jump.json", crabSkeleton.Data);

			this.dragonSkeleton = this.LoadSkeleton("dragon.png", "dragon.json", "dragon-skeleton.json");
			this.dragonAnimationFly = animationReader.ReadAnimationJsonFile("Content/dragon-flying.json", dragonSkeleton.Data);

			this.animationWalk = skeleton.Data.FindAnimation ("WalkLeft");
			this.animationJump = skeleton.Data.FindAnimation ("Jump");
//			this.goblinSkeleton = this.LoadSkeleton("goblin.png", "goblin.json", "goblins-skeleton.json", 400, 700);
//			this.goblinAnimationWalk = animationReader.ReadAnimationJsonFile("Content/goblins-walk.json", goblinSkeleton.Data);

			this.spineBoySkeleton = this.LoadSkeleton("spineboy.png", "spineboy.json", "spineboy-skeleton.json");
			this.spineBoyAnimationWalk = animationReader.ReadAnimationJsonFile("Content/spineboy-walk.json", spineBoySkeleton.Data);
			this.spineBoyAnimationJump = animationReader.ReadAnimationJsonFile("Content/spineboy-jump.json", spineBoySkeleton.Data);

			this.lineTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			this.lineTexture.SetData(new[]{Color.White});
			this.textureMaps.Add(lineTexture);

			this.animation = 0;
			this.SetSkeletonStartPosition();

			base.LoadContent ();
		}

		private Skeleton LoadSkeleton(string textureGraphicsFile, string textureJsonFile, string skeletonJsonFile)
		{
			var textureMap = Content.Load<Texture2D>(textureGraphicsFile);
			this.textureMaps.Add (textureMap);
			
			var texturePackerReader = new TextureMapJsonReader ();			
			var textureAtlas = texturePackerReader.ReadTextureJsonFile("Content/" + textureJsonFile, textureMap);
			
			var skeletonReader = new SkeletonJsonReader(new TextureAtlasAttachmentLoader( textureAtlas));
			var skeleton = skeletonReader.ReadSkeletonJsonFile("Content/" + skeletonJsonFile);
			skeleton.FlipY = true;

			return skeleton;
		}

		private int animation = 0;

		private float timer = 1;

		protected override void Draw (GameTime gameTime)
		{
			this.spriteBatch.Begin ();

			this.GraphicsDevice.Clear (Color.CornflowerBlue);

			switch (animation)
			{
				case 0:
					this.Animate(this.spineBoySkeleton, this.spineBoyAnimationWalk);
					break;
					
				case 1:
					this.Animate(this.spineBoySkeleton, this.spineBoyAnimationJump);
					break;

				case 2:
					this.Animate(this.dragonSkeleton, this.dragonAnimationFly);
					break;

				case 3:
					this.Animate(this.crabSkeleton, this.crabAnimationJump);
					break;

				case 4:
					this.Animate(this.crabSkeleton, this.crabAnimationWalk);
					break;

					/*
				case 5:
					this.Animate(this.goblinSkeleton, this.goblinAnimationWalk);
					break;
					*/
			}

			this.spriteBatch.End ();

			base.Draw (gameTime);
		}

		protected override void Update (GameTime gameTime)
		{
			if (TouchPanel.IsGestureAvailable)
			{
				while (TouchPanel.IsGestureAvailable)
				{
					// swallow all touches 
					TouchPanel.ReadGesture();
				}

				if (++animation > 4)
				{
					animation = 0;
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

			switch (animation)
			{
				case 0:
				case 1:
					skeleton = this.spineBoySkeleton;
					x = 400;
					y = 600;
					break;

				case 2:
					skeleton = this.dragonSkeleton;
					x = 500;
					y = 650;
					break;

				case 3:
				case 4:
					skeleton = this.crabSkeleton;
					x = 500;
					y = 750;
					break;
			}

			skeleton.SetToBindPose();

			var root = skeleton.GetRootBone();
			root.X = x;
			root.Y = y;
			
			skeleton.UpdateWorldTransform();
		}

		private void Animate(Skeleton skeleton, Animation animation)
		{
			animation.Apply(skeleton, timer++ / 100, true);

			skeleton.UpdateWorldTransform();
			skeleton.Draw(this.spriteBatch);
			//skeleton.DrawDebug(this.spriteBatch, this.lineTexture);
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

