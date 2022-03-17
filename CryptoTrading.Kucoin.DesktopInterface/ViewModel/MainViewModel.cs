using LiveChartsCore;
using LiveChartsCore.Collections;
using LiveChartsCore.SkiaSharpView;

using System.ComponentModel;

using System.Runtime.CompilerServices;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal class MainViewModel : BaseViewModel
{
    public RangeObservableCollection<ISeries> Series { get; } = new ();

    public RangeObservableCollection<Axis> XAxes { get; } = new ();

    protected override void Dispose(bool disposing)
    {
    }
}