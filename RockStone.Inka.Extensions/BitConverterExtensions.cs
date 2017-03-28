using System;

namespace RockStone.Inka.Extensions
{
    public static class BitConverterExtensions
    {
        public static string BytesToString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }
    }
}
