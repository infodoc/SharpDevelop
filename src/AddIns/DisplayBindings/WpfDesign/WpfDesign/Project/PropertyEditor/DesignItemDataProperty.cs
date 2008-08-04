// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Diagnostics;
using System.Windows;

namespace ICSharpCode.WpfDesign.PropertyEditor
{
	/// <summary>
	/// wraps a DesignItemDataProperty (with IsEvent=false) for the property editor/grid.
	/// </summary>
	sealed class DesignItemDataProperty : DesignItemDataMember, IPropertyEditorDataProperty
	{
		internal DesignItemDataProperty(DesignItemDataSource ownerDataSource, DesignItemProperty property)
			: base(ownerDataSource, property)
		{
			Debug.Assert(!property.IsEvent);
		}
		
		public string Category {
			get { return property.Category; }
		}
		
		public System.ComponentModel.TypeConverter TypeConverter {
			get { return property.TypeConverter; }
		}
		
		public bool IsSet {
			get {
				return property.IsSet;
			}
			set {
				if (value != property.IsSet) {
					if (value) {
						// copy default value to local value
						property.SetValue(property.ValueOnInstance);
					} else {
						property.Reset();
					}
				}
			}
		}
		
		public event EventHandler IsSetChanged {
			add    { property.IsSetChanged += value; }
			remove { property.IsSetChanged -= value; }
		}
		
		public bool IsAmbiguous {
			get { return false; }
		}
		
		public object Value {
			get {
				return property.ValueOnInstance;
			}
			set {
				property.SetValue(value);
			}
		}
		
		public event EventHandler ValueChanged {
			add { property.ValueChanged += value; }
			remove { property.ValueChanged -= value; }
		}
		
		public bool CanUseCustomExpression {
			get {
				return true;
			}
		}
		
		public void SetCustomExpression(string expression)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Creates a UIElement that can edit the property value.
		/// </summary>
		public UIElement CreateEditor()
		{
			EditorManager manager = ownerDataSource.Services.GetService<EditorManager>();
			if (manager != null) {
				return manager.CreateEditor(this);
			} else {
				return new FallbackEditor(this);
			}
		}
	}
}
