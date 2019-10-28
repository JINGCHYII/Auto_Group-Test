using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfBlendApp
{
    public class MyTextBlock : TextBlock
    {

        public MyTextBlock() : base()
        {
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler((s, e) =>
              {
                  if (e.ClickCount == 1)
                  {
                      Console.WriteLine("click once");
                      TextBlock item = s as TextBlock;
                      string msg = item.Text;

                      var parent = VisualTreeHelper.GetParent(this);
                      while (parent != null && !(parent is MyListBoxItem))
                      {
                          parent = VisualTreeHelper.GetParent(parent);
                      }
                      ((MyListBoxItem)parent).ChangeListBoxConText(msg);
                     
                      e.Handled = true;
                  }
                  else if (e.ClickCount == 2)
                  {
                      Console.WriteLine("click twice");
                      e.Handled = true;
                  }
              });
        }
    }
}
