using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRental.Helper
{
    public class clsDebouncer
    {
        private CancellationTokenSource _cts;

        public async Task DebounceAsync(Func<Task> action, int delay = 300)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                await Task.Delay(delay, _cts.Token);
                await action();
            }
            catch (TaskCanceledException) { }
        }
    }
}
