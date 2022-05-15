using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Management;

[Flags]
public enum ChangeAction
{
    Undefined = default,
    Seleced = 1,
    Added = 2,
    Removed = 4,
}