using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBlendApp
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl , IUC, IPD
    {
        public ISubTestItem subTestItem;

        public bool Testing { get; set; }

        public bool GridColorTrigger_L
        {
            get { return (bool)GetValue(GridColorTrigger_LProperty); }
            set { SetValue(GridColorTrigger_LProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridColorTrigger_L.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridColorTrigger_LProperty =
            DependencyProperty.Register("GridColorTrigger_L", typeof(bool), typeof(UserControl1), new PropertyMetadata(true));


        public bool GridColorTrigger_R
        {
            get { return (bool)GetValue(GridColorTrigger_RProperty); }
            set { SetValue(GridColorTrigger_RProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridColorTrigger_L.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridColorTrigger_RProperty =
            DependencyProperty.Register("GridColorTrigger_R", typeof(bool), typeof(UserControl1), new PropertyMetadata(true));




        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(UserControl1), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));




        public double ProgressBarVal
        {
            get { return (double)GetValue(ProgressBarValProperty); }
            set { SetValue(ProgressBarValProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressBarVal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressBarValProperty =
            DependencyProperty.Register("ProgressBarVal", typeof(double), typeof(UserControl1), new PropertyMetadata(0.0));


        public string TestName
        {
            get { return (string)GetValue(TestNameProperty); }
            set { SetValue(TestNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TestName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TestNameProperty =
            DependencyProperty.Register("TestName", typeof(string), typeof(UserControl1), new PropertyMetadata(""));

        public void SetProgressBar(double val)
        {
            ProgressBarVal = val;
        }


        public UserControl1()
        {
            Testing = false;

            InitializeComponent();

            DataContext = this;
        }

        public UserControl1(ISubTestItem subTestItem)
        {
            Testing = false;

            this.subTestItem = subTestItem;

            TestName = subTestItem.Name;

            InitializeComponent();

            DataContext = this;
        }

        public bool startTest(object o, CancellationToken token, ManualResetEvent resetEvent)
        {
            Console.WriteLine(o);
            subTestItem.Test(this, token, resetEvent);
            return true;
        }
    }
}
