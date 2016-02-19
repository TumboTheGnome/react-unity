using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityEngine.UI.Routing
{
	[RequireComponent(typeof(Image))]
	public class PanelControl : UIElementNode
	{
		private Image _background;

		void Awake(){
			_background = GetComponent<Image> ();
		}

		public override void Hide ()
		{
			_background.enabled = false;
		}

		public override void Show ()
		{
			_background.enabled = true;
		}

		public override void Refresh ()
		{
			base.Refresh ();
		}
	}
}
