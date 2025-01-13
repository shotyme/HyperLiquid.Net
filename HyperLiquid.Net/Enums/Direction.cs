using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// Direction
    /// </summary>
    public enum Direction
    {
        [Map("Open Long")]
        OpenLong,
        [Map("Close Long")]
        CloseLong,
        [Map("Open Short")]
        OpenShort,
        [Map("Close Short")]
        CloseShort

#warning spot?
    }
}
