using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfBlendApp
{
    public interface IUC
    {
        bool Testing { get; set; }
        bool startTest(object o, CancellationToken token, ManualResetEvent resetEvent);
    }
}
