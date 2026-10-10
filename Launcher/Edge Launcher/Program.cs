using System;
using PortableEdge;

namespace Edge_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EdgeLaunch.Run("Edge", () => new Form1(), args);
        }
    }
}
