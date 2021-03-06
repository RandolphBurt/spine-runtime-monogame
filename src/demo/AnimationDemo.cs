/******************************************************************************
 * Spine Runtimes Software License
 * Version 2.1
 * 
 * Copyright (c) 2013, Esoteric Software
 * All rights reserved.
 * 
 * You are granted a perpetual, non-exclusive, non-sublicensable and
 * non-transferable license to install, execute and perform the Spine Runtimes
 * Software (the "Software") solely for internal use. Without the written
 * permission of Esoteric Software (typically granted by licensing Spine), you
 * may not (a) modify, translate, adapt or otherwise create derivative works,
 * improvements of the Software or develop new applications using the Software
 * or (b) remove, delete, alter or obscure any trademarks or any copyright,
 * trademark, patent or other intellectual property or proprietary rights
 * notices on or in the Software, including any copy thereof. Redistributions
 * in binary or source form must include this license and terms.
 * 
 * THIS SOFTWARE IS PROVIDED BY ESOTERIC SOFTWARE "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL ESOTERIC SOFTARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 * OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Spine;

namespace Demo {
	public class AnimationDemo : Microsoft.Xna.Framework.Game {
		GraphicsDeviceManager graphics;
		SkeletonMeshRenderer skeletonRenderer;
		Skeleton skeleton;
		Slot headSlot;
		AnimationState state;
		SkeletonBounds bounds = new SkeletonBounds();

		public AnimationDemo () {
			IsMouseVisible = true;

			graphics = new GraphicsDeviceManager(this);
			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
		}

		protected override void Initialize () {
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent () {
			skeletonRenderer = new SkeletonMeshRenderer(GraphicsDevice);
			skeletonRenderer.PremultipliedAlpha = true;

			// String name = "spineboy";
			// String name = "goblins-ffd";
			String name = "crab";

			Atlas atlas = new Atlas("Content/" + name + ".atlas", new XnaTextureLoader(GraphicsDevice));
			SkeletonJson json = new SkeletonJson(atlas);
			if (name == "spineboy") json.Scale = 0.6f;
			if (name == "raptor") json.Scale = 0.5f;
			skeleton = new Skeleton(json.ReadSkeletonData("Content/" + name + ".json"));
			if (name == "goblins-ffd") skeleton.SetSkin("goblin");

			// Define mixing between animations.
			AnimationStateData stateData = new AnimationStateData(skeleton.Data);
			state = new AnimationState(stateData);

			if (name == "spineboy") {
				stateData.SetMix("run", "jump", 0.2f);
				stateData.SetMix("jump", "run", 0.4f);

				// Event handling for all animations.
				state.Start += Start;
				state.End += End;
				state.Complete += Complete;
				state.Event += Event;

				state.SetAnimation(0, "test", false);
				TrackEntry entry = state.AddAnimation(0, "jump", false, 0);
				entry.End += End; // Event handling for queued animations.
				state.AddAnimation(0, "run", true, 0);
			} else if (name == "raptor") {
				state.SetAnimation(0, "walk", true);
				state.SetAnimation(1, "empty", false);
				state.AddAnimation(1, "gungrab", false, 2);
			} else if (name == "crab") {
				state.SetAnimation(0, "WalkLeft", true);
			} else {
				state.SetAnimation(0, "walk", true);
			}

			if (name == "crab")
			{
				skeleton.X = 500;
				skeleton.Y = 590;
			}
			else
			{
				skeleton.X = 400;
				skeleton.Y = 590;
			}

			skeleton.UpdateWorldTransform();

			headSlot = skeleton.FindSlot("head");
		}

		protected override void UnloadContent () {
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update (GameTime gameTime) {
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw (GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			state.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
			state.Apply(skeleton);
			skeleton.UpdateWorldTransform();
			skeletonRenderer.Begin();
			skeletonRenderer.Draw(skeleton);
			skeletonRenderer.End();

			bounds.Update(skeleton, true);
			MouseState mouse = Mouse.GetState();
			if (headSlot != null)
			{
				headSlot.G = 1;
				headSlot.B = 1;
				if (bounds.AabbContainsPoint (mouse.X, mouse.Y))
				{
					BoundingBoxAttachment hit = bounds.ContainsPoint (mouse.X, mouse.Y);
					if (hit != null)
					{
						headSlot.G = 0;
						headSlot.B = 0;
					}
				}
			}

			base.Draw(gameTime);
		}

		public void Start (AnimationState state, int trackIndex) {
			Console.WriteLine(trackIndex + " " + state.GetCurrent(trackIndex) + ": start");
		}

		public void End (AnimationState state, int trackIndex) {
			Console.WriteLine(trackIndex + " " + state.GetCurrent(trackIndex) + ": end");
		}

		public void Complete (AnimationState state, int trackIndex, int loopCount) {
			Console.WriteLine(trackIndex + " " + state.GetCurrent(trackIndex) + ": complete " + loopCount);
		}

		public void Event (AnimationState state, int trackIndex, Event e) {
			Console.WriteLine(trackIndex + " " + state.GetCurrent(trackIndex) + ": event " + e);
		}
	}
}