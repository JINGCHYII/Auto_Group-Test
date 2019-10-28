using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfBlendApp
{
    public class MyListBoxItem : ListBoxItem
    {


        public List<ListContext> LCProperty
        {
            get { return (List<ListContext>)GetValue(LCPropertyProperty); }
            set { SetValue(LCPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LCProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LCPropertyProperty =
            DependencyProperty.Register("LCProperty", typeof(List<ListContext>), typeof(MyListBoxItem), new PropertyMetadata(new List<ListContext>()));


        int LBIndex = 0;

        public MyListBoxItem():base() { DataContext = this; }

        public MyListBoxItem(int index) : base() { LBIndex = index; DataContext = this; }

        public MyListBoxItem(List<ListContext> lcs, int index):base()
        {
            LCProperty = lcs;
            LBIndex = index;
            DataContext = this;
        }

        ListBox listbox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            listbox = Template.FindName("listbox", this) as ListBox;

            listbox.MouseDown += new MouseButtonEventHandler((s, e) =>
            {
                Console.WriteLine("dd");
                e.Handled = true;
            });
        }

        public void ChangeListBoxConText(string msg)
        {
            if (LCProperty == null || LCProperty.Any(x => x.LCs == null)) return;

            var parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is MyListBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            ListContext lc = LCProperty.Where(x => x.msg == msg).First();
            ((MyListBoxItem)((MyListBox)parent).Items[LBIndex + 1]).LCProperty = lc.LCs;
            // ((MyListBox)parent).ScrollIntoView(((MyListBox)parent).Items[LBIndex + 1]);
            listbox.SelectedItem = lc;

            ((MyListBox)parent).ScrollView(LBIndex + 1);

            this.IsHitTestVisible = false;
            //foreach (var item in VisualTreeExtern.FindVisualChild<MyTextBlock>(this))
            //{
            //    item.IsHitTestVisible = false;
            //}
            MainWindow.Wait(300);
            this.IsHitTestVisible = true;
            //foreach (var item in VisualTreeExtern.FindVisualChild<MyTextBlock>(this))
            //{
            //    item.IsHitTestVisible = true;
            //}
        }
    }
}
