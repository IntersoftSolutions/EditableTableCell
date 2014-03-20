using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace DataSamples.iOS
{
	public partial class TextboxTableCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("TextboxTableCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("TextboxTableCell");
		private UITextField _textField;
		public UITextField TextField
		{
			get{
				if(_textField==null)
				{
					_textField = this.ViewWithTag (101) as UITextField;
				}
				return _textField;
			}
		}

		public TextboxTableCell (IntPtr handle) : base (handle)
		{
			ContentView.Add (TextField);
		}

		public static TextboxTableCell Create ()
		{
			return (TextboxTableCell)Nib.Instantiate (null, null) [0];
		}
	}
}

