using System.Net.NetworkInformation;
using System.Text;

namespace WichSystem;

internal static class Program
{
    // Usage: ./WichSystem <ip-address>
    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: ./WichSystem <ip-address>");
            return 1;
        }

        var ipAddress = args[0];

        try
        {
            var ttl = GetTtl(ipAddress);
            var osName = DetectOperatingSystem(ttl);

            Console.WriteLine($"{ipAddress} (ttl -> {ttl}): {osName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ipAddress} : {ex.Message}");
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Sends a ping to the specified IP address and returns the Time To Live (TTL) value.
    /// </summary>
    /// <param name="ipAddress">The target IP address.</param>
    /// <returns>The TTL value from the ping response.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ping fails or no TTL is returned.</exception>
    private static int GetTtl(string ipAddress)
    {
        using var pingSender = new Ping();
        var options = new PingOptions
        {
            // Default TTL is often 128; this might be adjusted by network devices.
            DontFragment = true
        };

        // Create a buffer of data to send (32 bytes).
        const string data = "abcdefghijklmnopqrstuvwabcdefghij";
        const int timeout = 200;
        var buffer = Encoding.ASCII.GetBytes(data);

        var reply = pingSender.Send(ipAddress, timeout, buffer, options);

        if (reply.Status == IPStatus.Success && reply.Options != null)
        {
            return reply.Options.Ttl;
        }

        // If the ping wasn't successful, throw an exception with a descriptive message.
        throw new InvalidOperationException(
            $"Ping to {ipAddress} failed with status: {reply.Status}");
    }

    /// <summary>
    /// Determines the likely operating system based on the given TTL value.
    /// </summary>
    /// <param name="ttl">The Time To Live value from the ping response.</param>
    /// <returns>A string representing the likely operating system.</returns>
    private static string DetectOperatingSystem(int ttl)
    {
        // Basic heuristic:
        // - TTL values around 64 often come from Linux/Unix-like systems.
        // - TTL values around 128 often come from Windows systems.
        return ttl switch
        {
            <= 64 => "Linux",
            <= 128 => "Windows",
            _ => "Unknown"
        };
    }
}
