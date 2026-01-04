using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silver.BLL.Faults;

namespace Silver.BLL
{
    public class Teste : BaseException
    {
        public void Somar()
        {
            int a = 0;
            int b = 1;
            int result = b / a;
        }
    }
}
