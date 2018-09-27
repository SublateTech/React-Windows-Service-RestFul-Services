using ShiolWinSvc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMPPEngineer;
using XMPPEngineer.Client;
using XMPPEngineer.Im;

namespace ShiolWinSvc
{
    class XmppServiceProvider : AbstractServiceProvider
    {
        XmppClient client = null;
        Jid juliet = "leninv@jabber.no";

        public override void StartService()
        {
            string hostname = "jabber.no",
             username = "alvaro",
             password = "michelle";
            

            client = new XmppClient(hostname, username, password, "jabber.no");
            client.Message += Client_Message;
            
            
            client.Connect();

        //    client.SendInvite(new Jid("leninv@jabber.no"), null, "Bienvenido");

            /*
            Console.WriteLine("Juliet's XMPP client supports: ");
            foreach (var feat in client.GetFeatures(juliet))
                Console.WriteLine(" - " + feat);

            Message message = new Message(juliet, "Hello, World.");
            client.SendMessage(message);
            */
            
            while (true)
            {
                Console.Write("Type a message or type 'quit' to exit: ");
                string s = Console.ReadLine();
                if (s == "quit")
                    return;
                // Send the message to Juliet.
                if (!client.Connected)
                    client.Connect();
                client.SendMessage(juliet, s);
            }
            
            //   client.SendMessage(juliet, "Hola..");


            /*
            // basic
            using (XmppClient client = new XmppClient("jabber.no", "alvaro", "michelle"))
                {
                    
                    Message message = new Message(new Jid("cecilia@domain"), "Hello, World.");
                    client.SendMessage(message);
                }
            */

            // with stream management
            /*
            using (XmppClient clientsm = new XmppClient("domain", "user", "password"))
            {
                clientsm.Connect();

                clientsm.StreamManagementEnabled += (sdr, evt) =>
                {
                    Message messagesm = new Message(new Jid("user@domain"), "Hello, World.");
                    clientsm.SendMessage(messagesm);

                    // xep-0033 - multicast can send to a jid that can then route to multiple users
                    System.Collections.Generic.List<Jid> jids = new System.Collections.Generic.List<Jid>();
                    jids.Add(new Jid("admin@domain"));
                    jids.Add(new Jid("test@domain"));
                    jids.Add(new Jid("other@domain"));

                    Message multimessagesm = new Message(new Jid("multicast.domain"), "Test - " + DateTime.Now.ToLongTimeString(), null, jids);
                    clientsm.SendMessage(multimessagesm);
                };

                // enable stream management and recovery mode
                clientsm.EnableStreamManagement();
            }
            */
        }

        public override void StopService()
        {
            if (client != null && client.Connected)
                client.Close();
            client = null;
        }
        private  void Client_Message(object sender, MessageEventArgs e)
        {
            //throw new NotImplementedException();

            
            if (e.Message.Body.IndexOf("sql://") > -1)
            {
                string sqlMessage = e.Message.Body.Replace("sql://","");
                Console.WriteLine(sqlMessage);
                Consultas sql = new Consultas();
                sqlMessage = sql.getJsonSqlMessage(sqlMessage);
                client.SendMessage(e.Jid, sqlMessage);
                //Console.WriteLine(sqlMessage);
            }
            else
                Console.WriteLine(e.Message.Body);

        }
        
        public static void Main(string[] args)
        {
            XmppServiceProvider server = new XmppServiceProvider();
            server.StartService();
            Console.ReadKey();
            server.StopService();
        }


    }
}
