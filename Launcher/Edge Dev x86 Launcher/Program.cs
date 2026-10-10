using System;
using PortableEdge;

namespace Edge_Dev_x86_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Dev x86", () => new Form1(), args);
        }
    }
}
