using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityEngine.UI.Routing
{
	[RequireComponent(typeof(Button))]
	public class ButtonControl : UIElementNode
	{

		public UIRouter TargetRouter;
		public string ChangeRoute;
		public bool HideOnClick;

		private Button btn;
		private Image img;
		private Text txt;

		void Awake(){
			btn = GetComponent<Button> ();
			img = GetComponent<Image> ();
			txt = GetComponentInChildren<Text> ();
		}

		void Start(){
			btn.onClick.AddListener (() => {

				UIRouter router = Router;

				if(TargetRouter != null){
					router = TargetRouter;
				}

				router.Route = _buildPath(router);

				if(HideOnClick){
					Hide();
				}
			});
		}

		public override void Hide ()
		{
			btn.enabled = false;
			img.enabled = false;
			txt.enabled = false;
		}

		public override void Show ()
		{
			btn.enabled = true;
			img.enabled = true;
			txt.enabled = true;
		}

		public override void Refresh ()
		{

		}

		private string _buildPath(UIRouter router){
			string code = "{current}/";

			int crnt = ChangeRoute.IndexOf (code);

			if (crnt == 0) {
				string nwPath = ChangeRoute.Substring(code.Length, ChangeRoute.Length - code.Length);
				return router.Route+'/'+nwPath;
			} else {
				return ChangeRoute;
			}
		}

	}
}