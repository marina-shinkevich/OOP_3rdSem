using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace lab8
{
    public delegate int Del(int a, int b);

    public class FileName
    {
        public event Del Event;

        public int GG(int a, int b)
        {
            if (Event != null)
            {
                return Event.Invoke(a, b);
            }
            return 0;
        }

    }
    public class KK
    {
        public int FFF(int a, int b)
        {
            return a * b;
        }
    }
    
    public class HHHHH
    {
        public static void Main(string[] args)
        {

            FileName f = new FileName();
            KK g = new KK();

            f.Event += g.FFF;

           int res= f.GG(2, 3);
            Console.WriteLine(res);


        }
    }

}
