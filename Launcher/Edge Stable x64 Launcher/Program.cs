using System;
using PortableEdge;

namespace Edge_Stable_x64_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Stable x64", () => new Form1(), args);
        }
    }
}
