using System;

namespace shenyangZC
{
    class Pack
    {
        public int byteFlag_;
        int bitFlag_;
        public byte[] buf_ { get; set; }

        public Pack()
        {
            this.buf_ = new byte[200];
            byteFlag_ = 0;
            bitFlag_ = 0;
        }

        public void SetBit(bool flag)
        {
            if (flag)
            {
                buf_[byteFlag_] |= (byte)(1 << bitFlag_);
            }
            bitFlag_++;
            if (bitFlag_ == 8)
            {
                Skip();
            }
        }

        public void Skip()
        {
            if (bitFlag_ != 0)
            {
                byteFlag_++;
                bitFlag_ = 0;
            }
        }

        public void PackByte(byte value)
        {
            buf_[byteFlag_] = value;
            byteFlag_++;
        }


        public void PackUint16(UInt16 value)
        {
            buf_[byteFlag_] = (byte)(value & 0xff);
            buf_[byteFlag_ + 1] = (byte)(value >> 8);
            byteFlag_ += 2;
        }

        public void PackUint32(UInt32 value)
        {
            PackUint16((UInt16)(value & 0xffff));
            PackUint16((UInt16)(value >> 16));
        }

        public void PackUint64(UInt64 value)
        {
            PackUint32((UInt32)(value & 0xffff));
            PackUint32((UInt32)(value >> 32));
        }
    }
}