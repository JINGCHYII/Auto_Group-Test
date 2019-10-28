using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfBlendApp
{
    public interface IPD
    {
        bool GridColorTrigger_L { get; set; }
        bool GridColorTrigger_R { get; set; }
        Brush BorderColor { get; set; }
        double ProgressBarVal { get; set; }
        void SetProgressBar(double val);
    }
}
