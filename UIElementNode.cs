using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace UnityEngine.UI.Routing
{
	public class UIElementNode : MonoBehaviour, IEquatable<UIElementNode>
	{
		public string[] Routes = new string[]{};
		internal UIRouter Router;
		internal UIElementNode Parent;

		public virtual void Refresh ()
		{
		}

		public virtual void Show ()
		{

			foreach (MonoBehaviour c in GetComponents<MonoBehaviour>()) {
				c.enabled = true;
			}
		}

		public virtual void Hide ()
		{
			foreach (MonoBehaviour c in GetComponents<MonoBehaviour>()) {
				c.enabled = false;
			}
		}

		#region IEquatable implementation
		public bool Equals (UIElementNode other)
		{
			return this.GetInstanceID () == other.GetInstanceID ();
		}
		#endregion
	}
}