/// <summary>
/// CurveTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	/** Base class for frames that use an interpolation bezier curve. */
	public abstract class CurveTimeline
	{
		static private readonly float LINEAR = 0;
		static private readonly float STEPPED = -1;
		static private readonly int BEZIER_SEGMENTS = 10;
		private readonly float[] curves; // dfx, dfy, ddfx, ddfy, dddfx, dddfy, ...
			
		public CurveTimeline (int keyframeCount)
		{
			curves = new float[(keyframeCount - 1) * 6];
		}
			
		public void setLinear (int keyframeIndex)
		{
			curves [keyframeIndex * 6] = LINEAR;
		}
			
		public void setStepped (int keyframeIndex)
		{
			curves [keyframeIndex * 6] = STEPPED;
		}
			
		/** Sets the control handle positions for an interpolation bezier curve used to transition from this keyframe to the next.
		 * cx1 and cx2 are from 0 to 1, representing the percent of time between the two keyframes. cy1 and cy2 are the percent of
		 * the difference between the keyframe's values. */
		public void setCurve (int keyframeIndex, float cx1, float cy1, float cx2, float cy2)
		{
			float subdiv_step = 1f / BEZIER_SEGMENTS;
			float subdiv_step2 = subdiv_step * subdiv_step;
			float subdiv_step3 = subdiv_step2 * subdiv_step;
			float pre1 = 3 * subdiv_step;
			float pre2 = 3 * subdiv_step2;
			float pre4 = 6 * subdiv_step2;
			float pre5 = 6 * subdiv_step3;
			float tmp1x = -cx1 * 2 + cx2;
			float tmp1y = -cy1 * 2 + cy2;
			float tmp2x = (cx1 - cx2) * 3 + 1;
			float tmp2y = (cy1 - cy2) * 3 + 1;
			int i = keyframeIndex * 6;
			float[] curves = this.curves;
			curves [i] = cx1 * pre1 + tmp1x * pre2 + tmp2x * subdiv_step3;
			curves [i + 1] = cy1 * pre1 + tmp1y * pre2 + tmp2y * subdiv_step3;
			curves [i + 2] = tmp1x * pre4 + tmp2x * pre5;
			curves [i + 3] = tmp1y * pre4 + tmp2y * pre5;
			curves [i + 4] = tmp2x * pre5;
			curves [i + 5] = tmp2y * pre5;
		}
			
		public float getCurvePercent (int keyframeIndex, float percent)
		{
			int curveIndex = keyframeIndex * 6;
			float[] curves = this.curves;
			float dfx = curves [curveIndex];
			if (dfx == LINEAR)
			{
				return percent;
			}
			if (dfx == STEPPED)
			{
				return 0;
			}
			float dfy = curves [curveIndex + 1];
			float ddfx = curves [curveIndex + 2];
			float ddfy = curves [curveIndex + 3];
			float dddfx = curves [curveIndex + 4];
			float dddfy = curves [curveIndex + 5];
			float x = dfx, y = dfy;
			int i = BEZIER_SEGMENTS - 2;
			while (true)
			{
				if (x >= percent)
				{
					float lastX = x - dfx;
					float lastY = y - dfy;
					return lastY + (y - lastY) * (percent - lastX) / (x - lastX);
				}
				if (i == 0)
				{
					break;
				}
				i--;
				dfx += ddfx;
				dfy += ddfy;
				ddfx += dddfx;
				ddfy += dddfy;
				x += dfx;
				y += dfy;
			}
			return y + (1 - y) * (percent - x) / (1 - x); // Last point is 1,1.
		}
	}
}

