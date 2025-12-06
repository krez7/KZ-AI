using System;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bitboard
{
    public class RandXOR
    {
        static public uint state = 2732384823; //1804289383
        static public uint GetRandUInt32()
        {
            uint number = RandXOR.state;
            number ^= number << 13;
            number ^= number >> 17;
            number ^= number << 5;

            state = number;

            return number;
        }

        static public UInt64 getRandUInt64()
        {
            UInt64 n1, n2, n3, n4;
            n1 = ((UInt64)GetRandUInt32() & 0xFFFF);
            n2 = ((UInt64)GetRandUInt32() & 0xFFFF);
            n3 = ((UInt64)GetRandUInt32() & 0xFFFF);
            n4 = ((UInt64)GetRandUInt32() & 0xFFFF);
            
            return (n1 | (n2 << 16) | (n3 << 32) | (n4 << 48));
        }
    }
}
