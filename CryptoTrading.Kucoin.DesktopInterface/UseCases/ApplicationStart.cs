using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using System.Diagnostics;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public class ApplicationStart : IUseCase
{
    public void Execute()
    {
        if (!DataHub.Instance.ManageUpdater(new KucoinTickUpdater()))
        {
            _ = MessageBox.Show("Could not init the updater, exiting the application...", "Error");
            Process.GetCurrentProcess().Kill(true);
        }
        if (!DataHub.Instance.Start(true))
        {
            _ = MessageBox.Show("Could not start the updater, exiting the application...", "Error");
            Process.GetCurrentProcess().Kill(true);
        }
    }
}