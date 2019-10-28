using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// OrUC.xaml 的交互逻辑
    /// </summary>
    public partial class OrUC : UserControl, ICD, IPD
    {
        public int Cur_Num { get; set; }
        public int UC_Num { get; set; }
        public bool Next_CD() => Cur_Num >= 1 ? true : false;
        public bool AlreadyDone { get; set; }

        public void SetProgressBar(double val)
        {
            ProgressBarVal = val;
        }

        public bool GridColorTrigger_L
        {
            get { return (bool)GetValue(GridColorTrigger_LProperty); }
            set { SetValue(GridColorTrigger_LProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridColorTrigger_L.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridColorTrigger_LProperty =
            DependencyProperty.Register("GridColorTrigger_L", typeof(bool), typeof(OrUC), new PropertyMetadata(true));



        public bool GridColorTrigger_R
        {
            get { return (bool)GetValue(GridColorTrigger_RProperty); }
            set { SetValue(GridColorTrigger_RProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridColorTrigger_L.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridColorTrigger_RProperty =
            DependencyProperty.Register("GridColorTrigger_R", typeof(bool), typeof(OrUC), new PropertyMetadata(true));



        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(OrUC), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));



        public double ProgressBarVal
        {
            get { return (double)GetValue(ProgressBarValProperty); }
            set { SetValue(ProgressBarValProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressBarVal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressBarValProperty =
            DependencyProperty.Register("ProgressBarVal", typeof(double), typeof(OrUC), new PropertyMetadata(0.0));


        public OrUC()
        {
            Cur_Num = 0;
            UC_Num = 0;
            AlreadyDone = false;

            InitializeComponent();

            DataContext = this;
        }
    }
}
