using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfBlendApp
{
    public class TestClass3 : ISubTestItem
    {

        public TestClass3(string name)
        {
            Name = name;
        }

        public override void Test(object TestObj, CancellationToken token, ManualResetEvent resetEvent)
        {
            Console.WriteLine("TestClass1 test start");
            for (int i = 0; i < 80; i++)
            {
                resetEvent.WaitOne();
                if (token.IsCancellationRequested) return;
               
                if (TestObj is IPD)
                {
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        ((IPD)TestObj).SetProgressBar((i + 1) / 80.0);
                        //System.Threading.Thread.Sleep(100);
                    }));
                    MainWindow.Wait(100);
                }
            }
            Console.WriteLine("TestClass1 test end");
            base.Test(TestObj, token, resetEvent);
        }
    }
}
