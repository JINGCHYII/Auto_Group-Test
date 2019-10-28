using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfBlendApp
{
    public class MyListBox : ListBox
    {



        public List<ListContext> LCsProperty
        {
            get { return (List<ListContext>)GetValue(LCsPropertyProperty); }
            set { SetValue(LCsPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LCsProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LCsPropertyProperty =
            DependencyProperty.Register("LCsProperty", typeof(List<ListContext>), typeof(MyListBox), new PropertyMetadata(new List<ListContext>(), new PropertyChangedCallback(OnLCsChanged)));

        private static void OnLCsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyListBox myListBox = d as MyListBox;
            myListBox.Items.Clear();
            myListBox.LCdex((List<ListContext>)(e.NewValue), ref myListBox.ItemNums);
            myListBox.Items.Add(new MyListBoxItem((List<ListContext>)(e.NewValue), 0));
            for (int i = 1; i < myListBox.ItemNums; i++)
            {
                myListBox.Items.Add(new MyListBoxItem(i));
            }
        }


        int ItemNums;


        public MyListBox() : base() { this.SelectionChanged += new SelectionChangedEventHandler((s, e) => { btnEnabledProperty = SelectedIndex > 0; }); }
       // public MyListBox(List<ListContext> lcs) : base() => LCsProperty = lcs;

        public void AddLCs(List<ListContext> lcs) => LCsProperty = lcs;


        int LCdex(List<ListContext> listContexts, ref int index)
        {
            if (listContexts != null && listContexts.Count > 0)
            {
                index++;
                return LCdex(listContexts[0].LCs, ref index);
            }
            else
                return index;
        }

        public void ScrollView(int index)
        {
            //double offset = scrollViewer.HorizontalOffset;
            //scrollViewer.ScrollToHorizontalOffset(offset+60);

            SelectedIndex = index;
            ScrollIntoView(Items[index]);
        }

        Button backBtn;
        ScrollViewer scrollViewer;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            scrollViewer = Template.FindName("SV", this) as ScrollViewer;
            backBtn = Template.FindName("BackBtn", this) as Button;
            backBtn.Click += BackBtn_Click;
            backBtn.SetBinding(Button.IsEnabledProperty,
                new Binding("btnEnabledProperty")
                {
                    Source = this
                });
        }

        public bool btnEnabledProperty
        {
            get { return (bool)GetValue(btnEnabledPropertyProperty); }
            set { SetValue(btnEnabledPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for btnEnabledProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty btnEnabledPropertyProperty =
            DependencyProperty.Register("btnEnabledProperty", typeof(bool), typeof(MyListBox), new PropertyMetadata(false));



        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count <= 0) return;

            foreach (var item in VisualTreeExtern.FindVisualChild<MyListBoxItem>(this))
            {
                item.IsHitTestVisible = false;
            }

            SelectedIndex = Math.Max(0, SelectedIndex);
            ScrollIntoView(Items[(Math.Max(0, --SelectedIndex))]);
            MainWindow.Wait(300);
            foreach (var item in VisualTreeExtern.FindVisualChild<MyListBoxItem>(this))
            {
                item.IsHitTestVisible = true;
            }
        }
    }
}
