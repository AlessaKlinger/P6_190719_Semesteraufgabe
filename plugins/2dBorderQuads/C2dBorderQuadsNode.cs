#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "BorderQuads", Category = "2d", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2dBorderQuadsNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Min X", DefaultValue = -1.0)]
		ISpread<double> FMinX;
		
		[Input("Max X", DefaultValue = 1.0)]
		ISpread<double> FMaxX;
		
		[Input("Min Y", DefaultValue = -1.0)]
		ISpread<double> FMinY;
		
		[Input("Max Y", DefaultValue = 1.0)]
		ISpread<double> FMaxY;
		
		[Input("Width", DefaultValue = 1.0)]
		ISpread<double> FWidth;

		[Output("Centers ")]
		ISpread<Vector2D> FCenters;
		
		[Output("Sizes ")]
		ISpread<Vector2D> FScales;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FCenters.SliceCount = SpreadMax * 4;
			FScales.SliceCount = SpreadMax * 4;
			
			var j = 0;
			Vector2D center;
			Vector2D size;
			for (int i = 0; i < SpreadMax; i++)
			{
				//quad right
				center.x = FMaxX[i] + FWidth[i] * 0.5;
				center.y = (FMaxY[i] + FMinY[i]) * 0.5;
				size.x = FWidth[i];
				size.y = FMaxY[i] - FMinY[i];
				
				FCenters[j] = center;
				FScales[j] = size;
				j++;
				
				//quad left
				center.x = FMinX[i] - FWidth[i] * 0.5;
				center.y = FCenters[j-1].y;
				
				FCenters[j] = center;
				FScales[j] = FScales[j-1];
				j++;
				
				//quad top
				center.x = (FMaxX[i] + FMinX[i]) * 0.5;				
				center.y = FMaxY[i] + FWidth[i] * 0.5;
				size.x = FMaxX[i] - FMinX[i] + 2 * FWidth[i];				
				size.y = FWidth[i];
				
				FCenters[j] = center;
				FScales[j] = size;
				j++;
				
				//quad bottom
				center.x = FCenters[j-1].x;				
				center.y = FMinY[i] - FWidth[i] * 0.5;
				
				FCenters[j] = center;
				FScales[j] = FScales[j-1];
				j++;
			}
			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
}
