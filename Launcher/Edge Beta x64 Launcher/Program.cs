using System;
using PortableEdge;

namespace Edge_Beta_x64_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Beta x64", () => new Form1(), args);
        }
    }
}
