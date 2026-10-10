using System;
using PortableEdge;

namespace Edge_Canary_x86_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Canary x86", () => new Form1(), args);
        }
    }
}
