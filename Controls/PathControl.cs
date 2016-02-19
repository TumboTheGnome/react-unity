using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityEngine.UI.Routing
{
	[RequireComponent(typeof(Text))]
	public class PathControl : UIElementNode
	{
		private Text _txt; 

		void Awake(){
			_txt = GetComponent<Text> ();
		}

		public override void Hide ()
		{
			_txt.enabled = false;
		}

		public override void Show ()
		{
			_txt.enabled = true;
		}

		public override void Refresh ()
		{
			base.Refresh ();

			_txt.text = Router.Route;
		}
	}
}
