using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    public class Engine
    {
        private readonly Random rand = new Random();

        public Task<int> DoStuffAsync(int delay, IProgress<double> progress)
        {
            return Task.Run(() =>
                {
                    var step = delay/100;

                    for (var i = 0; i < 100; i++)
                    {
                        Thread.Sleep(step);
                        progress.Report(i);
                    }
                    progress.Report(100);

                    return rand.Next();
                });
        }
    }
}
