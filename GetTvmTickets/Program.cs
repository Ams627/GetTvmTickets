using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GetTvmTickets
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new Exception($"You must supply one filename (RCS_R_T...)");
                }
                var doc = XDocument.Load(args[0]);
                var ns = doc.Root.GetDefaultNamespace();
                var nonTvmTickets1 = doc.Descendants(ns + "FTOT").Select(x=>x.Descendants(ns + "Channel"));
                var nonTvmTickets = doc.Descendants(ns + "FTOT").Select(x => new { TicketCode = x.Attribute("t").Value, ChannelList = x.Descendants(ns + "Channel").Select(y => y.Attribute("ch").Value)}).Where(z=>!z.ChannelList.Contains("00004")).Select(a=>a.TicketCode).ToList();
                nonTvmTickets.ForEach(x => Console.WriteLine(x));
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
