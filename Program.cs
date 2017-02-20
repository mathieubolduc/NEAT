using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Program
    {
        static void Main(string[] args)
        {
            NEAT neat = new NEAT(2, 1);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write("Press a key to terminate:");
                Console.ReadLine();
            }
                    
        }
    }
}
