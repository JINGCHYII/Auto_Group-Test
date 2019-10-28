using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfBlendApp
{
    public class ISubTestItem
    {
        public string Name;
        public virtual void Test(object TestObj, CancellationToken token, ManualResetEvent resetEvent) { }
    }
}
