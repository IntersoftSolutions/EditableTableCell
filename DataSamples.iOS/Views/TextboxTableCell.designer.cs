// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace DataSamples.iOS
{
	[Register ("TextboxTableCell")]
	partial class TextboxTableCell
	{
		[Outlet]
		MonoTouch.UIKit.UITextField textBox { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TextLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (textBox != null) {
				textBox.Dispose ();
				textBox = null;
			}

			if (TextLabel != null) {
				TextLabel.Dispose ();
				TextLabel = null;
			}
		}
	}
}
