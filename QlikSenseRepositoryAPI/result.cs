using System;

namespace QlikSenseRepository
{
    class Result
    {
        private static void Main()
        {
            ConnectQlik get = new ConnectQlik();
            string response = get.GetRequest("qs2.qliklocal.net", "/qrs/about");
            Console.WriteLine(response);
            Console.ReadKey(true);
        }
    }
}