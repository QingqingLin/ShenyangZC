using System;

namespace shenyangZC
{
    class Unpack
    {
        int byteFlag_;
        int bitFlag_;
        byte[] buf_;

        public void Reset(byte[] buf)
        {
            buf_ = buf;
            byteFlag_ = 0;
            bitFlag_ = 0;
        }

        public UInt16 GetUint16()
        {
            UInt16 value = (UInt16)(buf_[byteFlag_ + 1] << 8);
            value |= buf_[byteFlag_];
            byteFlag_ += 2;
            return value;
        }

        public bool GetBit()
        {
            bool result = ((buf_[byteFlag_] >> (7 - bitFlag_++)) & 1) == 1;
            if (bitFlag_ == 8)
            {
                Skip();
            }
            return result;
        }

        public void Skip()
        {
            if (bitFlag_ != 0)
            {
                byteFlag_++;
                bitFlag_ = 0;
            }
        }

        public UInt32 GetUint32()
        {
            UInt32 value = (UInt32)GetUint16();
            value |= (UInt32)(GetUint16() << 16);
            return value;
        }

        public Byte GetByte()
        {
            byteFlag_++;
            return buf_[byteFlag_ - 1];
        }

        public void SkipBytes(int num)
        {
            byteFlag_ += num;
        }
    }
}
