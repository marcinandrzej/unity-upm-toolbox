using System;

namespace MA.Systems.Fading
{
    public static class ChannelMaskExtensions
    {
        public static bool HasSingleFlag(this EChannelMask mask)
        {
            int value = (int)mask;

            return value != 0 && (value & (value - 1)) == 0;
        }

        public static int CountFlags(EChannelMask mask)
        {
            uint value = (uint)mask;
            int count = 0;

            while (value != 0)
            {
                value &= value - 1;
                count++;
            }

            return count;
        }

        public static void PerformActionOnSeparatedFlags(EFadeOperationResult mask, Action<EFadeOperationResult> action)
        {
            int remainingBits = (int)mask;

            while (remainingBits != 0)
            {
                int singleBit = remainingBits & -remainingBits;

                EFadeOperationResult singleFlag = (EFadeOperationResult)singleBit;
                action.Invoke(singleFlag);

                remainingBits ^= singleBit;
            }
        }
    }
}
