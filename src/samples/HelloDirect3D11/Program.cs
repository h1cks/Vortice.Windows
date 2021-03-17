﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpGen.Runtime;
using SharpGen.Runtime.Diagnostics;
using Vortice;

namespace HelloDirect3D11
{
    public static class Program
    {
        private class TestApplication : Application
        {
            protected override void InitializeBeforeRun()
            {
                var validation = false;
#if DEBUG
                validation = true;
#endif

                _graphicsDevice = new D3D11GraphicsDevice(validation, MainWindow);
            }
        }

        public static void Main()
        {
#if DEBUG
            Configuration.EnableObjectTracking = true;
#endif

            using (var app = new TestApplication())
                app.Run();

#if DEBUG
            Console.WriteLine(ObjectTracker.ReportActiveObjects());
#endif
        }
    }
}
