using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace WpfBlendApp
{

    public enum UCType
    {
        UC1,
        UC2,
        UC3,
        And,
        Or
    }

    public class ListContext
    {
        public string msg { get; set; }
        public List<ListContext> LCs { get; set; }

        public ListContext() { }
        public ListContext(string msg, List<ListContext> lcs)
        {
            this.msg = msg;
            this.LCs = lcs;
        }
    }

    public class PointInfo
    {
        public Point SegPoint_L;
        public Point SegPoint_R;
        public int visualIndex;
        public UserControl uc;

        public PointInfo(Point s, Point e, int index)
        {
            SegPoint_L = s;
            SegPoint_R = e;
            visualIndex = index;
        }
    }

    public class UCInfo
    {
        public List<UserControl> UC_P;
        public List<UserControl> UC_N;
        public List<int> PointIndex;
        public int CurIndex;
        public bool UC_P_One;

        public UCInfo(UserControl uc_p, UserControl uc_n, List<int> pointIndex, int curindex, bool ucp_one = true)
        {
            if (uc_p == null)
                UC_P = new List<UserControl> { };
            else
                UC_P = new List<UserControl> { uc_p };

            if (uc_n == null)
                UC_N = new List<UserControl> { };
            else
                UC_N = new List<UserControl> { uc_n };

            PointIndex = pointIndex;
            CurIndex = curindex;
            UC_P_One = ucp_one;
        }
    }


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool GridColorTrigger_L
        {
            get { return (bool)GetValue(GridColorTrigger_LProperty); }
            set { SetValue(GridColorTrigger_LProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridColorTrigger_L.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridColorTrigger_LProperty =
            DependencyProperty.Register("GridColorTrigger_L", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));


        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(MainWindow), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));



        public double ScaleSize
        {
            get { return (double)GetValue(ScaleSizeProperty); }
            set { SetValue(ScaleSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleSizeProperty =
            DependencyProperty.Register("ScaleSize", typeof(double), typeof(MainWindow), new PropertyMetadata(1.0));


        public RoutedUICommand menuItemClickCommand { get; set; }


        List<ListContext> listContexts { get; set; }

        static MainWindow _mainWindow;

        public MainWindow()
        {
            InitializeComponent();

            listContexts = new List<ListContext>()
            {
                new ListContext("a",new List<ListContext>(){new ListContext("aa",new List<ListContext>() { new ListContext("aaaaa", null)}),new ListContext("aaa", new List<ListContext>() { new ListContext("aaaaaa", null) }) }),
                new ListContext("b",new List<ListContext>(){new ListContext("bb", new List<ListContext>() { new ListContext("bbbbb", null) }),new ListContext("bbb", new List<ListContext>() { new ListContext("bbbbbb", null) }) }),
                new ListContext("c",new List<ListContext>(){new ListContext("cc", new List<ListContext>() { new ListContext("ccccc", null) }),new ListContext("ccc", new List<ListContext>() { new ListContext("cccccc", null) }) }),
            };           

            UCListBox.AddLCs(listContexts);

            contextmenu.DataContext = listContexts;

            //UCListBox.Items.Add(new MyListBoxItem(listContexts,0));
            //UCListBox.Items.Add(new MyListBoxItem(listContexts[0].LCs,1));

            menuItemClickCommand = new RoutedUICommand();
            CommandBinding menuCommandBinding = new CommandBinding(menuItemClickCommand, MenuItem_Click);
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), menuCommandBinding);

            this.MouseUp += new MouseButtonEventHandler((s, e) =>
              {
                  canvas2.ReleaseMouseCapture();
              });

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;

            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseMove += OnMouseMove;

            this.Loaded += new RoutedEventHandler((s, e) => 
            {
                GridAdjustWithScrollViewer();
                StartContent.Margin = new Thickness(scrollGrid.Width / 2 - 200, scrollGrid.Height / 2 - 60, 0, 0);
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.ActualWidth / 2);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ActualHeight / 2);
            });
            this.SizeChanged += new SizeChangedEventHandler((s, e) => { GridAdjustWithScrollViewer(); });
            scrollViewer.SizeChanged += new SizeChangedEventHandler((s, e) => { GridAdjustWithScrollViewer(); });

            DataContext = this;

            if (_mainWindow == null)
                _mainWindow = this;
        }

        void GridAdjustWithScrollViewer()
        {
            scrollGrid.Width = (scrollViewer.ActualWidth * 2) < 600 ? 600 : (scrollViewer.ActualWidth * 2);
            scrollGrid.Height = (scrollViewer.ActualHeight * 2) < 450 ? 450 : (scrollViewer.ActualHeight * 2);
        }

        static int totalLine = 0;
        int SP_cur = 0;
        int EP_cur = 0;

        Dictionary<int, Point> IPPairs = new Dictionary<int, Point>(); //PointIndex_Point
        Dictionary<UserControl, UCInfo> UPPairs = new Dictionary<UserControl, UCInfo>();
        Dictionary<int, List<UserControl>> DUPairs = new Dictionary<int, List<UserControl>>(); //Path_UC

        List<Point> SP = new List<Point>();
        List<Point> EP = new List<Point>();

        List<PointInfo> PI = new List<PointInfo>();

        List<UserControl> UCs = new List<UserControl>();
        List<UserControl> SelUCs = new List<UserControl>();

        UserControl UC_L;
        UserControl UC_R;
        List<UserControl> S_UC = new List<UserControl>();
        List<int> SPindex = new List<int>();

        Point? startPoint;
        Point? endPoint;

        Brush pathBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        bool SelStart = false;
        bool curAdd = false;
        int index = 0;

        double thickness = 2;
        double lineOffset = 100;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //startPoint = e.GetPosition(canvas);
            //if (!isHitContent)
            //    canvas2.CaptureMouse();
        }

        private void Canvas2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = startPoint ?? e.GetPosition(canvas);

            Point pt = e.GetPosition((UIElement)sender);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!canRelease)
                endPoint = e.GetPosition(canvas);

            if (startPoint != null && canDraw && e.LeftButton == MouseButtonState.Pressed)
            {
                //canvas2.CaptureMouse();
                if (canvas.Children.Count > 0)
                {
                    if (curAdd)
                        canvas.Children.RemoveAt(canvas.Children.Count - 1);
                    else
                        curAdd = true;
                }

                if (canDraw)
                {
                    var pf = new PathFigure { StartPoint = (Point)startPoint };
                    BezierSegment bezierSegment = new BezierSegment(
                        new Point(R_firstDown ? Math.Min(endPoint.Value.X, startPoint.Value.X - lineOffset) : Math.Max(endPoint.Value.X, startPoint.Value.X + lineOffset), startPoint.Value.Y),
                        new Point(R_firstDown ? Math.Max(startPoint.Value.X, endPoint.Value.X + lineOffset) : Math.Min(startPoint.Value.X, endPoint.Value.X - lineOffset), endPoint.Value.Y),
                        (Point)endPoint, true);
                    pf.Segments.Add(bezierSegment);


                    //var pf2 = new PathFigure { StartPoint = new Point(Math.Max(endPoint.Value.X, startPoint.Value.X + (Is_UC_L ? 100 : -100)), startPoint.Value.Y) };
                    //pf2.Segments.Add(new LineSegment(new Point(Math.Min(startPoint.Value.X, endPoint.Value.X + (Is_UC_L ? -100 : 100)), endPoint.Value.Y), true));
                    //var pfc2 = new PathFigureCollection { pf2 };
                    //var pg2 = new PathGeometry(pfc2);
                    //var path2 = new Path { StrokeThickness = 2, Stroke = new SolidColorBrush(Color.FromRgb(255,0,0)), Data = pg2 };
                    //canvas2.Children.Add(path2);
                    

                    var pfc = new PathFigureCollection { pf };
                    var pg = new PathGeometry(pfc);
                    var path = new Path { StrokeThickness = thickness, Stroke = pathBrush, Data = pg };
                    path.MouseLeftButtonDown += PathMouseDownEvent;
                    canvas.Children.Add(path);
                    index = canvas.Children.Count - 1;
                    curAdd = true;
                }
            }
            else if (!Keyboard.IsKeyDown(Key.LeftCtrl) && startPoint != null && !canDraw && !isHitContent && e.LeftButton == MouseButtonState.Pressed)
            {
                canvas2.CaptureMouse();
                SelRect.Stroke = new SolidColorBrush(Color.FromRgb(170, 170, 170));
                Canvas.SetLeft(SelRect, Math.Min(startPoint.Value.X, endPoint.Value.X));
                Canvas.SetTop(SelRect, Math.Min(startPoint.Value.Y, endPoint.Value.Y));
                SelRect.Width = Math.Abs(startPoint.Value.X - endPoint.Value.X);
                SelRect.Height = Math.Abs(startPoint.Value.Y - endPoint.Value.Y);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SelRect.Stroke = Brushes.Transparent;

            GridColorTrigger_L = true;

            foreach (var item in UPPairs)
                ((IPD)item.Key).GridColorTrigger_L = ((IPD)item.Key).GridColorTrigger_R = true;

            foreach (var item in UPPairs.Where(x => !CanDraw(x.Key)))
                ((IPD)item.Key).GridColorTrigger_R = false;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (canDraw)
            {
                if (canRelease)
                {
                    endPoint = endPoint ?? e.GetPosition(canvas);

                    totalLine++;
                    SP_cur = 2 * index;
                    EP_cur = SP_cur + 1;

                    PI.Add(new PointInfo((Point)startPoint, (Point)endPoint, index));

                    if (!IPPairs.ContainsKey(SP_cur))
                        IPPairs.Add(SP_cur, R_firstDown ? (Point)endPoint : (Point)startPoint);
                    if (!IPPairs.ContainsKey(EP_cur))
                        IPPairs.Add(EP_cur, R_firstDown ? (Point)startPoint : (Point)endPoint);

                    int LP = SP_cur;
                    int RP = EP_cur;

                    if (StartGDown)
                    {
                        SPindex.Add(LP);
                        if (UPPairs.Keys.Contains(UC_R))
                            UPPairs[UC_R].PointIndex.Add(RP);
                        //else
                        //    UPPairs.Add(UC_R, new UCInfo(null, null, new List<int> { RP }));

                        S_UC.Add(UC_R);

                        DUPairs.Add(index, new List<UserControl> { UC_R });
                    }
                    else
                    {
                        if (UPPairs.Keys.Contains(UC_L))
                        {
                            UPPairs[UC_L].PointIndex.Add(LP);
                            UPPairs[UC_L].UC_N.Add(UC_R);
                        }
                        //else
                        //    UPPairs.Add(UC_L, new UCInfo(null, UC_R, new List<int> { LP }));

                        if (UPPairs.Keys.Contains(UC_R))
                        {
                            UPPairs[UC_R].PointIndex.Add(RP);
                            UPPairs[UC_R].UC_P.Add(UC_L);
                        }
                        //else
                        //    UPPairs.Add(UC_R, new UCInfo(UC_L, null, new List<int> { RP }));

                        DUPairs.Add(index, new List<UserControl> { UC_L, UC_R });
                    }
                }
                else
                {
                    if (curAdd)
                        canvas.Children.RemoveAt(canvas.Children.Count - 1);

                    index = canvas.Children.Count - 1;
                }
            }
            else if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !isHitContent)
            {
                foreach (var item in UCs.Where(x => x is IPD))
                    ((IPD)item).BorderColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                BorderColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                SelUCs.Clear();
                SelUCs = RectContainUCs();
                if (!((StartContent.Margin.Left + StartContent.ActualWidth < Canvas.GetLeft(SelRect) || StartContent.Margin.Left > Canvas.GetLeft(SelRect) + SelRect.ActualWidth) ||
                    (StartContent.Margin.Top + StartContent.ActualHeight < Canvas.GetTop(SelRect) || StartContent.Margin.Top > Canvas.GetTop(SelRect) + SelRect.ActualHeight)) && endPoint != null && endPoint != startPoint)
                    SelStart = true;
                else
                    SelStart = false;

                foreach (var item in SelUCs.Where(x => x is IPD))
                    ((IPD)item).BorderColor = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if(SelStart)
                    BorderColor = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            }

            startPoint = endPoint = null;
            curAdd = false;
            canDraw = canRelease = false;
            StartGDown = false;

            //foreach (var item in UCs)
            //    ((UserControl1)item).GridColorTrigger_L = ((UserControl1)item).GridColorTrigger_R = true;
        }

        private void Canvas2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (startPoint != null && canDraw && e.LeftButton == MouseButtonState.Pressed)
            {
                if (curAdd)
                    canvas.Children.RemoveAt(canvas.Children.Count - 1);

                index = canvas.Children.Count - 1;

                startPoint = null;
                curAdd = false;
                canDraw = canRelease = false;
                StartGDown = false;
            }
        }


        void RefreshLine(Point startPos, Point endPos, int index)
        {
            if (canvas.Children.Count > index)
            {
                canvas.Children.RemoveAt(index);
            }
            var pf = new PathFigure { StartPoint = (Point)startPos };
            BezierSegment bezierSegment = new BezierSegment(
                new Point(Math.Max(endPos.X, startPos.X + lineOffset), startPos.Y),
                new Point(Math.Min(startPos.X, endPos.X - lineOffset), endPos.Y),
                endPos, true);
            pf.Segments.Add(bezierSegment);

            var pfc = new PathFigureCollection { pf };
            var pg = new PathGeometry(pfc);
            var path = new Path { StrokeThickness = thickness, Stroke = pathBrush, Data = pg };
            path.MouseLeftButtonDown += PathMouseDownEvent;
            canvas.Children.Insert(index, path);
        }

        private void PathMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            int Pindex = 0;
            Path path = sender as Path;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(canvas); i++)
            {
                Visual visual = (Visual)VisualTreeHelper.GetChild(canvas, i);
                if (visual is Path && (Path)visual == path)
                {
                    Pindex = i;
                }
            }

            canvas.Children.RemoveAt(Pindex);
            canvas.Children.Insert(Pindex, new UIElement());
            foreach (var item in UPPairs)
            {
                if (item.Value.PointIndex.Contains(2 * Pindex))
                    item.Value.PointIndex.Remove(2 * Pindex);
                if (item.Value.PointIndex.Contains(2 * Pindex + 1))
                    item.Value.PointIndex.Remove(2 * Pindex + 1);
            }

            if (SPindex.Contains(2 * Pindex))
                SPindex.Remove(2 * Pindex);
            if (SPindex.Contains(2 * Pindex + 1))
                SPindex.Remove(2 * Pindex + 1);

            if (DUPairs.Keys.Contains(Pindex))
            {
                if (DUPairs[Pindex].Count == 2)
                {
                    if (UPPairs.Keys.Contains(DUPairs[Pindex][0]))
                    {
                        if (UPPairs[DUPairs[Pindex][0]].UC_N.Contains(DUPairs[Pindex][1]))
                            UPPairs[DUPairs[Pindex][0]].UC_N.Remove(DUPairs[Pindex][1]);
                        if (UPPairs[DUPairs[Pindex][0]].UC_P.Contains(DUPairs[Pindex][1]))
                            UPPairs[DUPairs[Pindex][0]].UC_P.Remove(DUPairs[Pindex][1]);
                    }
                    if (UPPairs.Keys.Contains(DUPairs[Pindex][1]))
                    {
                        if (UPPairs[DUPairs[Pindex][1]].UC_N.Contains(DUPairs[Pindex][0]))
                            UPPairs[DUPairs[Pindex][1]].UC_N.Remove(DUPairs[Pindex][0]);
                        if (UPPairs[DUPairs[Pindex][1]].UC_P.Contains(DUPairs[Pindex][0]))
                            UPPairs[DUPairs[Pindex][1]].UC_P.Remove(DUPairs[Pindex][0]);
                    }
                }
                else
                {
                    if (S_UC.Contains(DUPairs[Pindex][0]))
                        S_UC.Remove(DUPairs[Pindex][0]);
                }

                if (DUPairs.Keys.Contains(Pindex))
                    DUPairs.Remove(Pindex);
            }
        }

        bool StartGDown = false;

        private void SRG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = SRG.TranslatePoint(new Point(SRG.ActualWidth / 2, SRG.ActualHeight / 2), canvas);

            StartGDown = true;
            canRelease = false;
            canDraw = true;

            R_firstDown = false;
            foreach (var item in UCs)
                ((IPD)item).GridColorTrigger_L = false;

            foreach (var item in S_UC)
                ((IPD)item).GridColorTrigger_R = false;

            foreach (var item in UPPairs)
                if (item.Value.UC_P_One && item.Value.UC_P != null && item.Value.UC_P.Count > 0)
                    ((IPD)item.Key).GridColorTrigger_R = false;
        }

        private void SRG_MouseEnter(object sender, MouseEventArgs e)
        {
            StartGDown = true;
            canRelease = false;
            if (R_firstDown)
            {
                canRelease = true;
                if (S_UC.Contains(UC_R))
                    canRelease = false;
            }
        }

        private void SRG_MouseLeave(object sender, MouseEventArgs e)
        {
            canRelease = false;
        }

        private void SRG_MouseMove(object sender, MouseEventArgs e)
        {
            endPoint = SRG.TranslatePoint(new Point(SRG.ActualWidth / 2, SRG.ActualHeight / 2), canvas);
        }

        private bool CanDraw(UserControl uc)
        {
            return !((UPPairs.Keys.Contains(uc) && UPPairs[uc].UC_P_One && (UPPairs[uc].UC_P?.Count ?? 0) > 0) || S_UC.Contains(uc));
        }

        private List<UserControl> RectContainUCs()
        {
            List<UserControl> retUCs = new List<UserControl>();
            foreach (var item in UCs)
            {
                double left = item.Margin.Left; double left2 = Canvas.GetLeft(SelRect);
                double top = item.Margin.Top; double top2 = Canvas.GetTop(SelRect);
                if ((!((item.Margin.Left + item.ActualWidth < Canvas.GetLeft(SelRect) || item.Margin.Left > Canvas.GetLeft(SelRect) + SelRect.ActualWidth) ||
                    (item.Margin.Top + item.ActualHeight < Canvas.GetTop(SelRect) || item.Margin.Top > Canvas.GetTop(SelRect) + SelRect.ActualHeight))) && endPoint != null && endPoint != startPoint)
                {
                    retUCs.Add(item);
                }
            }
            return retUCs;
        }

        bool canDraw = false;
        bool canRelease = false;

        bool R_firstDown = false;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            UCType uCType = (UCType)menuItem.DataContext;
            UserControl uc = new UserControl();

            int canvas2childCnt = canvas2.Children.Count;

            if (uCType == UCType.UC1)
                uc = new UserControl1(new TestClass1("Test1"));
            else if (uCType == UCType.UC2)
                uc = new UserControl1(new TestClass2("Test2"));
            else if (uCType == UCType.UC3)
                uc = new UserControl1(new TestClass3("Test3"));
            else if (uCType == UCType.And)
                uc = new AndUC();
            else if (uCType == UCType.Or)
                uc = new OrUC();

            Grid child = (Grid)uc.Content;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(child); i++)
            {
                Visual visual = (Visual)VisualTreeHelper.GetChild(child, i);
                if (visual is Path && ((Path)visual).Name == "dragPath")
                {
                    ((Path)visual).Cursor = Cursors.Hand;
                    ((Path)visual).MouseMove += new MouseEventHandler(Element_MouseMove);
                    ((Path)visual).MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                    ((Path)visual).MouseLeftButtonDown += new MouseButtonEventHandler((o, s) =>
                    {
                        var maxZ_ = canvas2.Children.OfType<UIElement>().Select(x => Canvas.GetZIndex(x)).Max();
                        Canvas.SetZIndex(uc, maxZ_ + 1);
                    });
                    ((Path)visual).MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
                }
                else if (visual is Ellipse && (((Ellipse)visual).Name == "LG" || ((Ellipse)visual).Name == "RG"))
                {
                    ((Ellipse)visual).MouseLeftButtonDown += new MouseButtonEventHandler((o, s) =>
                    {
                        canRelease = false;
                        canDraw = true;
                        if (((Ellipse)visual).Name == "LG")
                        {
                            UC_R = uc as UserControl;
                            if (!CanDraw(UC_R))
                            {
                                canDraw = false;
                                return;
                            }

                            ((IPD)uc).GridColorTrigger_L = false;
                            R_firstDown = true;
                            foreach (var item in UCs)
                                ((IPD)item).GridColorTrigger_R = false;
                            if (UPPairs.Count != 0 && UPPairs.Keys.Contains(UC_R))
                                foreach (var item in UPPairs[UC_R].UC_P.Where(x => x != null))
                                    ((IPD)item).GridColorTrigger_L = false;
                        }
                        else
                        {
                            GridColorTrigger_L = false;
                            UC_L = uc as UserControl;
                            ((IPD)uc).GridColorTrigger_R = false;
                            R_firstDown = false;

                            foreach (var item in UCs)
                                ((IPD)item).GridColorTrigger_L = false;
                            if (UPPairs.Count != 0 && UPPairs.Keys.Contains(UC_L))
                            {
                                foreach (var item in UPPairs[UC_L].UC_N.Where(x => x != null))
                                    ((IPD)item).GridColorTrigger_R = false;
                                foreach (var item in UPPairs.Where(x => !CanDraw(x.Key)))
                                    ((IPD)item.Key).GridColorTrigger_R = false;
                            }
                        }
                        startPoint = (((Ellipse)visual)).TranslatePoint(new Point((((Ellipse)visual)).ActualWidth / 2, (((Ellipse)visual)).ActualHeight / 2), canvas);
                    });
                    ((Ellipse)visual).MouseEnter += new MouseEventHandler((o, s) =>
                    {
                        canRelease = false;
                        if (((Ellipse)visual).Name == "LG" && !R_firstDown)
                        {
                            UC_R = uc as UserControl;
                            canRelease = true;
                            //if (UPPairs.Count != 0 && UPPairs.Keys.Contains(UC_L) && UPPairs[UC_L].UC_N.Contains(UC_R))
                            //    canRelease = false;
                            if (!((IPD)UC_R).GridColorTrigger_R)
                                canRelease = false;
                        }
                        else if (((Ellipse)visual).Name == "RG" && R_firstDown)
                        {
                            UC_L = uc as UserControl;
                            canRelease = true;
                            //if (UPPairs.Count != 0 && UPPairs.Keys.Contains(UC_R) && UPPairs[UC_R].UC_P.Contains(UC_L))
                            //    canRelease = false;
                            if (!((IPD)UC_L).GridColorTrigger_L)
                                canRelease = false;
                        }
                    });
                    ((Ellipse)visual).MouseLeave += new MouseEventHandler((o, s) =>
                    {
                        canRelease = false;
                    });
                    ((Ellipse)visual).MouseMove += new MouseEventHandler((o, s) =>
                    {
                        endPoint = (((Ellipse)visual)).TranslatePoint(new Point((((Ellipse)visual)).ActualWidth / 2, (((Ellipse)visual)).ActualHeight / 2), canvas);
                    });
                }
                else if (visual is Button && ((Button)visual).Name == "CloseBtn")
                {
                    ((Button)visual).Click += new RoutedEventHandler((o, s) =>
                    {
                        canvas2.Children.RemoveAt(canvas2childCnt);
                        UserControl rmUC = null;
                        foreach (var item in UPPairs.Where(x => x.Value.CurIndex == canvas2childCnt))
                            rmUC = item.Key;

                        if (rmUC != null)
                        {
                            UCs.Remove(rmUC);
                            UPPairs.Remove(rmUC);
                            foreach (var item in UPPairs)
                            {
                                if (item.Value.UC_P.Contains(rmUC))
                                    item.Value.UC_P.Remove(rmUC);
                                if (item.Value.UC_N.Contains(rmUC))
                                    item.Value.UC_N.Remove(rmUC);
                            }
                        }

                        List<Path> rmPaths = new List<Path>();
                        foreach (var item in DUPairs.Where(x=>x.Value.Contains(uc)))
                        {
                            var path = canvas.Children[item.Key];
                            if (path is Path)
                            {
                                rmPaths.Add((Path)path);
                            }
                        }
                        foreach (var item in rmPaths)
                        {
                            ((Path)item).RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = Mouse.MouseDownEvent, Source = this });
                        }
                        canvas2.Children.Insert(canvas2childCnt, new UIElement());
                    });
                }
            }

            canvas2.Children.Add(uc);
            Point pp = Mouse.GetPosition(canvas2);
            uc.Margin = new Thickness(pp.X, pp.Y, 0, 0);
            var maxZ = canvas2.Children.OfType<UIElement>().Select(x => Canvas.GetZIndex(x)).Max();
            Canvas.SetZIndex(uc, maxZ + 1);


            if (uCType == UCType.UC1 || uCType == UCType.UC2 || uCType == UCType.UC3)
                UPPairs.Add(uc, new UCInfo(null, null, new List<int> { }, canvas2.Children.Count - 1));
            else if (uCType == UCType.And || uCType == UCType.Or)
                UPPairs.Add(uc, new UCInfo(null, null, new List<int> { }, canvas2.Children.Count - 1, false));

            UCs.Add(uc);
        }

        bool isDragDropInEffect = false;
        Point pos = new Point();

        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragDropInEffect)
            {
                void PointMoving(int pointIndex)
                {
                    double xPos_ = e.GetPosition(null).X / ScaleSize - pos.X + IPPairs[pointIndex].X;
                    double yPos_ = e.GetPosition(null).Y / ScaleSize - pos.Y + IPPairs[pointIndex].Y;
                    if (pointIndex % 2 == 0)
                        RefreshLine(new Point(xPos_, yPos_), IPPairs[pointIndex + 1], pointIndex / 2);
                    else
                        RefreshLine(IPPairs[pointIndex - 1], new Point(xPos_, yPos_), pointIndex / 2);
                    IPPairs[pointIndex] = new Point(xPos_, yPos_);
                }

                var parent = VisualTreeHelper.GetParent((Path)sender);
                FrameworkElement currEle = parent as FrameworkElement;

                var par = VisualTreeHelper.GetParent((Grid)currEle);
                var uc = ((Grid)((ContentPresenter)par).Content).Parent;

                if ((SelUCs.Count > 0 && uc is UserControl && SelUCs.Contains(uc as UserControl)) ||
                    SelStart && !(uc is UserControl))
                {
                    foreach (var item in SelUCs)
                    {
                        foreach (int pointIndex in UPPairs[item].PointIndex)
                            PointMoving(pointIndex);

                        double xPos_ = e.GetPosition(null).X / ScaleSize - pos.X + item.Margin.Left;
                        double yPos_ = e.GetPosition(null).Y / ScaleSize - pos.Y + item.Margin.Top;
                        item.Margin = new Thickness(xPos_, yPos_, 0, 0);
                    }
                    if (SelStart)
                    {
                        foreach (int pointIndex in SPindex)
                            PointMoving(pointIndex);

                        double xPos_ = e.GetPosition(null).X / ScaleSize - pos.X + StartContent.Margin.Left;
                        double yPos_ = e.GetPosition(null).Y / ScaleSize - pos.Y + StartContent.Margin.Top;
                        StartContent.Margin = new Thickness(xPos_, yPos_, 0, 0);
                    }
                    Point pos_tmp = e.GetPosition(null);
                    pos = new Point(pos_tmp.X / ScaleSize, pos_tmp.Y / ScaleSize);
                    return;
                }

                SelUCs.Clear();

                foreach (var item in UCs)
                    if (item is IPD)
                        ((IPD)item).BorderColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                BorderColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                if (uc is UserControl)
                {
                    if (UPPairs.ContainsKey(uc as UserControl))
                        foreach (int pointIndex in UPPairs[uc as UserControl].PointIndex)
                            PointMoving(pointIndex);
                }
                else
                    foreach (int pointIndex in SPindex)
                        PointMoving(pointIndex);

                //double xPos = e.GetPosition(null).X - pos.X + currEle.Margin.Left;
                //double yPos = e.GetPosition(null).Y - pos.Y + currEle.Margin.Top;
                //currEle.Margin = new Thickness(xPos, yPos, 0, 0);
                //pos = e.GetPosition(null);

                double xPos = e.GetPosition(null).X / ScaleSize - pos.X + ((FrameworkElement)uc).Margin.Left;
                double yPos = e.GetPosition(null).Y / ScaleSize - pos.Y + ((FrameworkElement)uc).Margin.Top;
                ((FrameworkElement)uc).Margin = new Thickness(xPos, yPos, 0, 0);
                Point pos_ = e.GetPosition(null);
                pos = new Point(pos_.X / ScaleSize, pos_.Y / ScaleSize);
            }
        }

        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fEle = sender as FrameworkElement;

            isDragDropInEffect = true;
            Point pos_ = e.GetPosition(null);
            pos = new Point(pos_.X / ScaleSize, pos_.Y / ScaleSize);

            fEle.CaptureMouse();
            //fEle.Cursor = Cursors.Hand;
        }

        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
            }
        }


        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y < scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(scrollGrid);

            if (e.Delta > 0)
                ScaleSize += 0.5;
            if (e.Delta < 0)
                ScaleSize -= 0.5;

            ScaleSize = Math.Min(Math.Max(0.5, ScaleSize), 3);

            scaleTransform.ScaleX = scaleTransform.ScaleY = ScaleSize;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, scrollGrid);

            e.Handled = true;
        }

        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, scrollGrid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(scrollGrid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / scrollGrid.ActualWidth;
                    double multiplicatorY = e.ExtentHeight / scrollGrid.ActualHeight;

                    double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }


        public HitTestFilterBehavior HitTestFilter(DependencyObject o)
        {
            var thing = canvas.Children;
            Type type = o.GetType();
            switch (type.Name)
            {
                case "Button":
                    Console.WriteLine("button down");
                    ((Button)o).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return HitTestFilterBehavior.Continue;
                case "Path":
                    Console.WriteLine("path down");
                    //((Path)o).RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = Mouse.MouseDownEvent, Source = this });
                    return HitTestFilterBehavior.Continue;
            }
            return HitTestFilterBehavior.Continue;
        }

        bool isHitContent = false;
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            if ((result.VisualHit is Canvas || result.VisualHit is ScrollViewer) && !isHitContent)
                isHitContent = false;
            else
                isHitContent = true;

            //Console.WriteLine(isHitContent);

            if (result.VisualHit is Ellipse && (((Ellipse)result.VisualHit).Name == "LG" || ((Ellipse)result.VisualHit).Name == "RG" || ((Ellipse)result.VisualHit).Name == "SRG"))
            {
                return HitTestResultBehavior.Stop;
            }

            if (result.VisualHit is Path && ((Path)result.VisualHit).Name != "divPath" && ((Path)result.VisualHit).Name != "dragPath")
            {
                ((Path)result.VisualHit).RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = Mouse.MouseDownEvent, Source = this });
                return HitTestResultBehavior.Stop;
            }

            return HitTestResultBehavior.Continue;
        }

        private void ScrollGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((UIElement)sender);

            isHitContent = false;

            VisualTreeHelper.HitTest(scrollGrid, null,
                new HitTestResultCallback(MyHitTestResult),
                new PointHitTestParameters(pt));
        }

        enum TestState
        {
            Start,
            Stop,
            Wait
        }

        TestState testState = TestState.Start;
        CancellationTokenSource cts = new CancellationTokenSource();
        ManualResetEvent resetEvent = new ManualResetEvent(true);
        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            startBtn.IsEnabled = false;
            waitBtn.IsEnabled = true;
         
            foreach (var item in UPPairs)
            {
                if (item.Key is ICD)
                {
                    ((ICD)item.Key).Cur_Num = 0;
                    ((ICD)item.Key).AlreadyDone = false;
                    ((ICD)item.Key).UC_Num = item.Value.UC_P != null ? item.Value.UC_P.Count : 0;
                }
                else if (item.Key is IUC)
                {
                    ((IUC)item.Key).Testing = false;
                }

                if(item.Key is IPD)
                {
                    ((IPD)item.Key).ProgressBarVal = 0.0;
                }
            }

            List<Task> tlist = new List<Task>();
            
            foreach (var item in S_UC)
            {
                tlist.Add(Task.Run(() => { ConTest(item, cts.Token, resetEvent); }));
            }
            Task.WhenAll(tlist).ContinueWith((s) =>
            {
                App.Current.Dispatcher.Invoke(() => 
                {
                    startBtn.IsEnabled = true;
                    waitBtn.IsEnabled = false;
                    testState = TestState.Start;
                });               
            });
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            waitBtn.Content = "Wait";
            waitBtn.IsEnabled = false;
            resetEvent.Set();
            waitState = false;
            cts.Cancel();
        }

        bool waitState = false;
        private void WaitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (waitState)
            {
                waitBtn.Content = "Wait";
                resetEvent.Set();
            }
            else
            {
                waitBtn.Content = "Continue";
                resetEvent.Reset();
            }

            waitState = !waitState;
        }

        object testLock = new object();

        private void ConTest(UserControl uc, CancellationToken token, ManualResetEvent resetEvent)
        {
            if (UPPairs.Keys.Contains(uc))
            {
                if (uc is IUC)
                {
                    lock (testLock)
                    {
                        if (!((IUC)uc).Testing)
                            ((IUC)uc).Testing = true;
                        else
                            return;
                    }

                    ((IUC)uc).startTest("aa", token, resetEvent);

                }
                else if (uc is ICD)
                {
                    Task task = Task.Run(() =>
                    {
                        if (((ICD)uc).AlreadyDone)
                            return;

                        ((ICD)uc).Cur_Num++;
                        while (!((ICD)uc).Next_CD() && !token.IsCancellationRequested)
                        {

                        }
                        ((ICD)uc).AlreadyDone = true;
                    });
                    task.Wait();
                }

                if (!token.IsCancellationRequested && UPPairs[uc].UC_N != null && UPPairs[uc].UC_N.Count > 0)
                {
                    List<Task> tlist = new List<Task>();
                    foreach (var item in UPPairs[uc].UC_N)
                    {
                        tlist.Add(Task.Run(() =>
                        {
                            ConTest(item, token, resetEvent);
                        }));
                    }
                    Task.WaitAll(tlist.ToArray());
                }
                else
                    return;
            }
        }


        public static void Wait(int interval)
        {
            ExecuteWait(() => Thread.Sleep(interval));
        }

        public static void ExecuteWait(Action action)
        {
            var waitFrame = new DispatcherFrame();

            // Use callback to "pop" dispatcher frame
            IAsyncResult op = action.BeginInvoke(dummy => waitFrame.Continue = false, null);

            // this method will block here but window messages are pumped
            Dispatcher.PushFrame(waitFrame);

            // this method may throw if the action threw. caller's responsibility to handle.
            action.EndInvoke(op);
        }
    }
}
