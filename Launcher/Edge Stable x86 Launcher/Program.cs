using System;
using PortableEdge;

namespace Edge_Stable_x86_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Stable x86", () => new Form1(), args);
        }
    }
}
