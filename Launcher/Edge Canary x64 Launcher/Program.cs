using System;
using PortableEdge;

namespace Edge_Canary_x64_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Canary x64", () => new Form1(), args);
        }
    }
}
