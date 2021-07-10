using AptumEngine.Core;
using AptumEngine.GUI;
using System;

namespace Sandbox
{
    class Program
    {
        static WindowConfig config = new()
        {
            initPos = new Vector2(200, 200),
            initSize = new Vector2(960, 960),
            title = "Window",
            resizable = true,
        };
        
        static readonly AWindow win = new AWindow(config);
        static void Main(string[] args) =>
            new Application()
            .AddLayer(new AptumGUILayer(win))
            .SetupEvents(win)
            .Run();
    }
}
