using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI.Routing
{
	public class UIRouter:UIElementNode
	{
		public string InitRoute = "";
		public bool ShowDefault = false;
		private Dictionary<string, List<UIElementNode>> _cache = new Dictionary<string, List<UIElementNode>> ();
		private List<UIElementNode> _nodes = new List<UIElementNode> ();
		private string _route = "";
		private bool _active = false;

		void Awake ()
		{
			_route = InitRoute;
			_processNode (this);
		}

		void Start ()
		{
			this.Hide ();
			if (Parent == null && ShowDefault) {
				this.Show();
			}
		}

		public override void Hide ()
		{
			base.Hide ();

			_active = false;


			foreach (UIElementNode node in _nodes) {
				node.Hide ();
			}
		}

		public override void Show ()
		{
			base.Show ();
			_active = true;
			_render ();
		}

		public string Route {
			get {
				return _route;
			}

			set {
				_route = value;
				_render ();
			}
		}

		/**
		 * Updates all elements in route. 
		 */
		internal UIRouter Digest ()
		{
			if (_cache.ContainsKey (_route)) {
				foreach (UIElementNode listener in _cache[_route]) {
					listener.Refresh ();
				}
			}
			return this;
		}

		/**
		 * Adds and caches an element
		 */
		internal UIRouter Add (UIElementNode listener)
		{
			if (listener != null) {
				_processNode (listener);
			} else {
				Debug.LogError ("Null listener passed.");
			}
			return this;
		}

		/**
		 * Removes an element
		 */
		internal UIRouter Remove (UIElementNode listener)
		{


			if (_nodes.Remove (listener)) {
			
				foreach (string route in listener.Routes) {
					if (_cache.ContainsKey (route)) {
						_cache [route].Remove (listener);
					}
				}
				
			}

			return this;
		}

		//Recursivly traverses transform heirarchy passing elements to be cached as relevent.
		private void _processNode (UIElementNode parent)
		{

			if (parent != null && ((parent == this) || (parent.GetType () != typeof(UIRouter)))) {

				foreach (Transform child in parent.transform) {

					UIElementNode element = child.GetComponent<UIElementNode> ();

					if (element == null) {
						element = child.gameObject.AddComponent<UIElementNode> ();
						element.Routes = new string[parent.Routes.Length];
						Array.Copy (parent.Routes, element.Routes, parent.Routes.Length);
					}

					_cacheListener (parent, element);

					_processNode (element);
				}
			}
		}

		private void _cacheListener (UIElementNode parent, UIElementNode listener)
		{
			if (!_nodes.Contains (listener)) {
				listener.Router = this;
				listener.Parent = parent;

				if (!_nodes.Contains (listener)) {
					_nodes.Add (listener);
				}


				for (int i = 0; i < listener.Routes.Length; i++) {
					_cacheRoute (listener.Routes [i], listener);
				}
			}
		}

		private void _cacheRoute (string route, UIElementNode listener)
		{
			if (!_cache.ContainsKey (route)) {
				_cache.Add (route, new List<UIElementNode> ());
			}

			_cache [route].Add (listener);


		}

		private void parsePath (List<string> segments, string path)
		{
			if (!String.IsNullOrEmpty (path)) {

				int index = path.IndexOf ('/');

				if (index != -1) {
					segments.Add (path.Substring (0, index));
					path = path.Substring (index + 1, path.Length - (index + 1)); 
					parsePath (segments, path);
				} else {
					segments.Add (path);
				}
			} else {
				segments = null;
			}
		}

		private void _render ()
		{

			if (_active) {
				List<string> tags = new List<string> ();
				parsePath (tags, _route);
			
				foreach (UIElementNode node in _nodes) {
					node.Hide ();
				}
			
				for (int i = 0; i< tags.Count; i++) {
					if (_cache.ContainsKey (tags[i])) {
					
						foreach (UIElementNode listener in _cache[tags[i]]) {
							listener.Show ();

							if (listener.GetType () == typeof(UIRouter)) {
								UIRouter router = (UIRouter)listener;

								router.Route = _buildContextualRoute (tags, i);
							}

						}
					}
				}
			
				this.Digest ();
			}
		}

		//Build route string from reamining tags.
		private string _buildContextualRoute(List<string> tags, int start){

			string result = tags [start];

			for (int i = start; i < tags.Count; i++) {
				result += '/' + tags [i];
			}

			return result;
		}
	}
}
