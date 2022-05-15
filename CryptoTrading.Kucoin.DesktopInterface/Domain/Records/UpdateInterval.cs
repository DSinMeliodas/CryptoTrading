using System;
using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

internal record UpdateInterval
{
    private static readonly TimeSpan Seconds30 = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan Minute1 = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan Minute2 = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan Minute3 = TimeSpan.FromMinutes(3);
    private static readonly TimeSpan Minute5 = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan Minute10 = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan Minute15 = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan Minute30 = TimeSpan.FromMinutes(30);

    public static UpdateInterval Default => Interval1Minute;
    public static UpdateInterval Interval30Seconds { get; } = new(Seconds30, "30s");
    public static UpdateInterval Interval1Minute { get; } = new(Minute1, "1m");
    public static UpdateInterval Interval2Minute { get; } = new(Minute2, "2m");
    public static UpdateInterval Interval3Minute { get; } = new(Minute3, "3m");
    public static UpdateInterval Interval5Minute { get; } = new(Minute5, "5m");
    public static UpdateInterval Interval10Minute { get; } = new(Minute10, "10m");
    public static UpdateInterval Interval15Minute { get; } = new(Minute15, "15");
    public static UpdateInterval Interval30Minute { get; } = new(Minute30, "30m");

    public static IReadOnlyList<UpdateInterval> AllIntervals { get; } = new []
    {
        Interval30Seconds, Interval1Minute, Interval2Minute,
        Interval3Minute, Interval5Minute, Interval10Minute,
        Interval15Minute, Interval30Minute
    };

    public string Name { get; }

    public TimeSpan Interval { get; }

    private UpdateInterval(TimeSpan interval, string name)
    {
        Interval = interval;
        Name = name;
    }
    
}