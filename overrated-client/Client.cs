using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace overrated_client
{
    class Client
    {
        private bool _gameRunning;
        private readonly Screenshotter _screenshotter;
        private readonly ScreenshotParser _parser;

        public Client()
        {
            _screenshotter = new Screenshotter();
            _parser = new ScreenshotParser();
        }

        public void StartPolling()
        {
            while (true)
            {
                Console.WriteLine("Trying to find overwatch process");
                var process = GetOverwatchProcess();
                _gameRunning = true;
                process.EnableRaisingEvents = true;
                process.Exited += OnOverwatchExit;
                Console.WriteLine("Process found! Doing some kind of loop while game is running...");
                while (_gameRunning)
                {
                    using (var screenshot = new Bitmap(_screenshotter.CaptureWindow(process.MainWindowHandle)))
                    {
                        Console.WriteLine(_parser.GetFpsFromImage(screenshot));
                    }
                    Thread.Sleep(3000);
                }
            }
        }

        private Process GetOverwatchProcess()
        {
            try
            {
                return Process.GetProcessesByName("Overwatch")[0];
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Overwatch process not found. Retrying in 5 seconds.");
                Thread.Sleep(5000);
                return GetOverwatchProcess();
            }
        }

        private void OnOverwatchExit(object sender, EventArgs args)
        {
            Console.WriteLine("Overwatch exited. Restarting polling...");
            _gameRunning = false;
        }
    }
}
