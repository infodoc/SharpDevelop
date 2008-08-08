using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICSharpCode.XamlDesigner
{
    public partial class Outline
    {
        public Outline()
        {
            InitializeComponent();
        }

		public static readonly DependencyProperty RootProperty =
			DependencyProperty.Register("Root", typeof(OutlineNode), typeof(Outline));

		public OutlineNode Root {
			get { return (OutlineNode)GetValue(RootProperty); }
			set { SetValue(RootProperty, value); }
		}
    }
}