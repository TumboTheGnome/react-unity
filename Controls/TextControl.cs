using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityEngine.UI.Routing
{
	[RequireComponent(typeof(Text))]
	public class TextControl : UIElementNode
	{
		Text component;

		void Awake(){
			component = GetComponent<Text> ();
		}

		public override void Hide ()
		{
			component.enabled = false;
		}

		public override void Show ()
		{
			component.enabled = true;
		}

		public override void Refresh ()
		{

		}

	}
}
