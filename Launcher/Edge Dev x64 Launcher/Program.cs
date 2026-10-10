using System;
using PortableEdge;

namespace Edge_Dev_x64_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Dev x64", () => new Form1(), args);
        }
    }
}
