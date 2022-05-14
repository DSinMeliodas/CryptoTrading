using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public partial record Candle
{
    public class CandleFactory
    {
        private DateTime? m_OpenTime;
        private decimal? m_Open;
        private decimal? m_Close;
        private decimal? m_High;
        private decimal? m_Low;
        private decimal? m_BaseVolume;
        private decimal? m_ForeignVolume;

        public void SetOpenTime(DateTime openTime) => m_OpenTime = openTime;

        public void SetHighLow(decimal high, decimal low)
        {
            if (high <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(high), high,
                    $"{nameof(high)} cannot be lower than or equal to 0");
            }

            if (low <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(low), low,
                    $"{nameof(low)} cannot be lower than or equal to 0");
            }

            if (high < low)
            {
                throw new ArgumentOutOfRangeException(nameof(high), high,
                    $"{nameof(high)} cannot be lower than {nameof(low)} which was {low}");
            }

            m_High = high;
            m_Low = low;
        }

        public void SetOpen(decimal open)
        {
            if (m_Low is null || m_High is null)
            {
                throw new InvalidOperationException($"cannot set {nameof(open)} until both low and high are set");
            }

            if (open <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(open), open,
                    $"{nameof(open)} cannot be lower than or equal to 0");
            }

            if (open < m_Low)
            {
                throw new ArgumentOutOfRangeException(nameof(open), open, $"{nameof(open)} cannot be lower than low");
            }

            if (open > m_High)
            {
                throw new ArgumentOutOfRangeException(nameof(open), open, $"{nameof(open)} cannot be higher than high");
            }

            m_Open = open;
        }

        public void SetClose(decimal close)
        {
            if (m_Low is null || m_High is null)
            {
                throw new InvalidOperationException($"cannot set {nameof(close)} until both low and high are set");
            }

            if (close <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(close), close,
                    $"{nameof(close)} cannot be lower than or equal to 0");
            }

            if (close < m_Low)
            {
                throw new ArgumentOutOfRangeException(nameof(close), close,
                    $"{nameof(close)} cannot be lower than low");
            }

            if (close > m_High)
            {
                throw new ArgumentOutOfRangeException(nameof(close), close,
                    $"{nameof(close)} cannot be higher than high");
            }

            m_Close = close;
        }

        public void SetVolume(decimal baseVolume, decimal foreignVolume)
        {
            if (m_Low is null || m_High is null)
            {
                throw new InvalidOperationException($"cannot set set volume until both low and high are set");
            }

            if (baseVolume != 0 && foreignVolume == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(foreignVolume), foreignVolume,
                    $"{nameof(foreignVolume)} cannot be 0 if {nameof(baseVolume)} is not 0");
            }

            if (baseVolume == 0 && foreignVolume != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(baseVolume), baseVolume,
                    $"{nameof(baseVolume)} cannot be 0 if {nameof(baseVolume)} is not 0");
            }

            m_BaseVolume = baseVolume;
            m_ForeignVolume = foreignVolume;
        }

        public Candle Create()
        {
            ArgumentNullException.ThrowIfNull(m_OpenTime);
            ArgumentNullException.ThrowIfNull(m_Open);
            ArgumentNullException.ThrowIfNull(m_Close);
            ArgumentNullException.ThrowIfNull(m_High);
            ArgumentNullException.ThrowIfNull(m_Low);
            ArgumentNullException.ThrowIfNull(m_BaseVolume);
            ArgumentNullException.ThrowIfNull(m_ForeignVolume);
            return new(m_OpenTime.Value,
                m_Open.Value,
                m_Close.Value,
                m_High.Value,
                m_Low.Value,
                m_BaseVolume.Value,
                m_ForeignVolume.Value);
        }
    }
}