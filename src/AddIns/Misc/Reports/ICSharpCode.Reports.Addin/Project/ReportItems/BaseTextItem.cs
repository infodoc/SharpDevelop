﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using ICSharpCode.Reports.Addin.Designer;
using ICSharpCode.Reports.Core;
using ICSharpCode.Reports.Core.BaseClasses.Printing;
using ICSharpCode.Reports.Addin.Dialogs;

namespace ICSharpCode.Reports.Addin
{
	/// <summary>
	/// Description of ReportTextItem.
	/// </summary>

	[Designer(typeof(ICSharpCode.Reports.Addin.Designer.TextItemDesigner))]
	public class BaseTextItem:AbstractItem
	{
		
		private string formatString;
		private StringFormat stringFormat;
		private StringTrimming stringTrimming;
		private ContentAlignment contentAlignment;
		
		
		public BaseTextItem():base()
		{
			base.DefaultSize = GlobalValues.PreferedSize;
			base.Size = GlobalValues.PreferedSize;
			base.BackColor = Color.White;
			this.contentAlignment = ContentAlignment.TopLeft;
			TypeDescriptor.AddProvider(new TextItemTypeProvider(), typeof(BaseTextItem));
		}
		
		
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			this.Draw(e.Graphics);
		}
		
		
		public override void Draw(Graphics graphics)
		{
			StringTrimming designTrimmimg = StringTrimming.EllipsisCharacter;
			
			if (graphics == null) {
				throw new ArgumentNullException("graphics");
			}
			using (Brush b = new SolidBrush(this.BackColor)){
				graphics.FillRectangle(b, base.DrawingRectangle);
			}
			
			if (this.stringTrimming != StringTrimming.None) {
				designTrimmimg = stringTrimming;
			}
			TextDrawer.DrawString(graphics,this.Text,this.Font,
			                      new SolidBrush(this.ForeColor),
			                      this.ClientRectangle,
			                      designTrimmimg,
			                      this.contentAlignment);
			
			base.DrawControl(graphics,base.DrawingRectangle);
		}
		
		
		
		[EditorAttribute(typeof(DefaultTextEditor), 
		                  typeof(System.Drawing.Design.UITypeEditor) )]
		public override string Text {
			get { return base.Text; }
			set { base.Text = value;
				this.Invalidate();
			}
		}
		
		
		#region Format and alignment
		
		[Browsable(true),
		 Category("Appearance"),
		 Description("String to format Number's Date's etc")]
		[DefaultValue("entry1")]
		[TypeConverter(typeof(FormatStringConverter))]

		public string FormatString {
			get { return formatString; }
			set {
				formatString = value;
				Invalidate();
			}
		}

		
		[Browsable(false)]
		public StringFormat StringFormat {
			get { return stringFormat; }
			set {
				stringFormat = value;
				this.Invalidate();
			}
		}
		
		
		[Category("Appearance")]
		public StringTrimming StringTrimming {
			get { return stringTrimming; }
			set {
				stringTrimming = value;
				this.Invalidate();
			}
		}
		
		[Category("Appearance")]
		[EditorAttribute(typeof(System.Drawing.Design.ContentAlignmentEditor), 
		                  typeof(System.Drawing.Design.UITypeEditor) )]
		public ContentAlignment ContentAlignment {
			get { return contentAlignment; }
			set {
				contentAlignment = value;
				this.Invalidate();
			}
		}
		
		#endregion
		
				
		[Browsable(true),
		 Category("Databinding"),
		 Description("Datatype of the underlying Column")]
		
		
		[DefaultValue("System.String")]
		
		[TypeConverter(typeof(DataTypeStringConverter))]

		public string DataType {get;set;}
		
		
		#region Expression
		
		[Browsable(true),
		 Category("Expression"),
		 Description("Enter a valid Expression")]
		
		public string Expression {get;set;}
		
		#endregion
		
		#region CanGrow/CanShrink
		
		public bool CanGrow {get;set;}
			
		public bool CanShrink {get;set;}
			
		#endregion
	}
	
	#region TypeProvider
	
	internal class TextItemTypeProvider : TypeDescriptionProvider
	{
		public TextItemTypeProvider() :  base(TypeDescriptor.GetProvider(typeof(AbstractItem)))
		{
		}
		
	
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			ICustomTypeDescriptor td = base.GetTypeDescriptor(objectType,instance);
			return new TextItemTypeDescriptor(td, instance);
		}
	}
	
	
	internal class TextItemTypeDescriptor : CustomTypeDescriptor
	{
//		private BaseTextItem instance;
		
		public TextItemTypeDescriptor(ICustomTypeDescriptor parent, object instance)
			: base(parent)
		{
//			instance = instance as BaseTextItem;
		}

		
		public override PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection props = base.GetProperties(attributes);
			List<PropertyDescriptor> allProperties = new List<PropertyDescriptor>();

			DesignerHelper.AddDefaultProperties(allProperties,props);
			
			DesignerHelper.AddTextbasedProperties(allProperties,props);
			
			PropertyDescriptor prop = prop = props.Find("Text",true);
			allProperties.Add(prop);
		
			prop = props.Find("DrawBorder",true);
			allProperties.Add(prop);
			
			prop = props.Find("FrameColor",true);
			allProperties.Add(prop);
			
			prop = props.Find("ForeColor",true);
			allProperties.Add(prop);
			
			prop = props.Find("Visible",true);
			allProperties.Add(prop);
			
			prop = props.Find("Expression",true);
			allProperties.Add(prop);
			
			return new PropertyDescriptorCollection(allProperties.ToArray());
		}
	}
	#endregion
	
}
