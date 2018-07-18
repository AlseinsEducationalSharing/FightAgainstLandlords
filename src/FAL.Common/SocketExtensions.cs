using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Autofac;

namespace FAL
{
    public static class SocketExtensions
    {
        public static async Task Send<T>(this Socket socket, T obj)
        {
            var buffer = default(byte[]);
            using (var stream = new MemoryStream())
            {
                Application.Container.Resolve<BinaryFormatter>().Serialize(stream, obj);
                await stream.FlushAsync();
                buffer = stream.ToArray();
            }
            await socket.SendAsync(new ArraySegment<byte>(BitConverter.GetBytes(buffer.LongLength)), SocketFlags.None);
            await socket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
        }

        public static async Task<T> Receive<T>(this Socket socket)
        {
            var buffer = new byte[sizeof(long)];
            await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            buffer = new byte[BitConverter.ToInt64(buffer, 0)];
            await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            using (var stream = new MemoryStream(buffer))
            {
                return (T)Application.Container.Resolve<BinaryFormatter>().Deserialize(stream);
            }
        }
    }
}