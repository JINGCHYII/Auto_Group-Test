using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBlendApp
{
    public interface ICD
    {
        int Cur_Num { get; set; }
        int UC_Num { get; set; }
        bool Next_CD();
        bool AlreadyDone { get; set; }
    }
}
