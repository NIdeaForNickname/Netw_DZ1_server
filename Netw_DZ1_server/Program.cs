namespace Netw_DZ1_server;

using System.Net; 
using System.Net.Sockets; 
using System.Text; 
 
class Server 
{
    private const int DEFAULT_BUFLEN = 512;
    private const string DEFAULT_PORT = "27015"; 
 
    static void Main()
    {
        try
        {
            
            
            var ipAddress = IPAddress.Any;
            var localEndPoint = new IPEndPoint(ipAddress, int.Parse(DEFAULT_PORT));
 
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            Console.WriteLine("IP: Ok");
            
            listener.Listen(10); 
            Console.WriteLine("Listening...");
            
            var clientSocket = listener.Accept(); 
            Console.WriteLine("Connected");
 
            listener.Close(); 
            while (true)
            {
                var buffer = new byte[DEFAULT_BUFLEN]; 
                int bytesReceived = clientSocket.Receive(buffer); 
 
                if (bytesReceived > 0) 
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    Console.WriteLine($"Received: {message}"); 
 
                    string response = "";
                    int iMessage = 0;
                    if (int.TryParse(message, out iMessage))
                    {
                        response = (iMessage + 1).ToString();
                    }
                    else
                    {
                        response = "Must be a number";
                    }
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response); 
                    clientSocket.Send(responseBytes); 
                    Console.WriteLine($"Response: {response}");
                }
                else if (bytesReceived == 0)
                {
                    Console.WriteLine("Connection closed"); 
                    break;
                }
            }
 
            clientSocket.Shutdown(SocketShutdown.Send);
            clientSocket.Close();
            Console.WriteLine("Closing server");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}