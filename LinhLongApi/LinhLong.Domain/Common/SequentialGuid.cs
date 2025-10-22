using System.Security.Cryptography;

namespace LinhLong.Domain.Common
{
    /// <summary>
    /// Sequential GUID generator optimized for SQL Server clustered index.
    /// </summary>
    public static class SequentialGuid
    {
        public static Guid NewGuid()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(10);
            var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L); // ms

            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestamp);

            var guidBytes = new byte[16];
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
            Buffer.BlockCopy(timestamp, 2, guidBytes, 10, 6);

            return new Guid(guidBytes);
        }
    }
}
