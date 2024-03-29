﻿using System;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Util;

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
            ThrowHelper.ThrowIfLowerThanOrEqual0(high);
            ThrowHelper.ThrowIfLowerThanOrEqual0(low);
            ThrowHelper.ThrowIfLowerThan(low, high);
            m_High = high;
            m_Low = low;
        }

        public void SetOpen(decimal open)
        {
            ThrowHelper.ThrowIfHighOrLowNotSet(m_High, m_Low, open);
            ThrowHelper.ThrowIfLowerThanOrEqual0(open);
            ThrowHelper.ThrowIfLowerThan(open, m_Low.Value, limitExpression: "low");
            ThrowHelper.ThrowIfHigherThan(open, m_High.Value, limitExpression: "high");
            m_Open = open;
        }

        public void SetClose(decimal close)
        {
            ThrowHelper.ThrowIfHighOrLowNotSet(m_High, m_Low, close);
            ThrowHelper.ThrowIfLowerThanOrEqual0(close);
            ThrowHelper.ThrowIfLowerThan(close, m_Low.Value, limitExpression: "low");
            ThrowHelper.ThrowIfHigherThan(close, m_High.Value, limitExpression: "high");
            m_Close = close;
        }

        public void SetVolume(decimal baseVolume, decimal foreignVolume)
        {
            ThrowHelper.ThrowIfHighOrLowNotSet(m_High, m_Low, "volume");
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