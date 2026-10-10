using System;
using PortableEdge;

namespace Edge_Beta_x86_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge Beta x86", () => new Form1(), args);
        }
    }
}
