using System.Net.NetworkInformation;
using System.Text;

namespace WitchSystem
{
    class Program
    {
        // Usage : ./WitchSystem 127.0.0.1

        public static int Main(string[] args)
        {
            if (args.Count() != 1)
            {
                Console.WriteLine("Usage : ./WitchSystem <ip-address>");
                return 1;
            }

            try
            {
                var ttl = GetTTL(args[0]);
                var OsName = GetOS(ttl);

                Console.WriteLine("{0} (ttl -> {1}): {2}",
                args[0],
                ttl,
                OsName);
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0} : {1}",
                args[0],
                ex.ToString());
            }

            return 0;
        }

        public static int GetTTL(String ip_address)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "abcdefghijklmnopqrstuvwabcdefghij";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(ip_address, timeout, buffer, options);

            if (reply.Status == IPStatus.Success) return reply.Options.Ttl;
            else throw new Exception(reply.Status.ToString());
        }

        public static string GetOS(int ttl)
        {
            if (ttl >= 0 && ttl <= 64) return "Linux";
            else if (ttl >= 65 && ttl <= 128) return "Windows";
            else return "Unknow";
        }
    }
}
