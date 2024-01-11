namespace EMA_dotnet_vscode;

using LSEG.Ema.Access;
using System;
using System.Threading;
class AppClient: IOmmConsumerClient
    {
        // onRefreshMsg
        // Capture the initial image after subscribing (registering interest) in a market data item.  This 
        // callback utlizes the convenient print method to display output.
        public void OnRefreshMsg(RefreshMsg refreshMsg, IOmmConsumerEvent consumerEvent) 
        {
            Console.WriteLine(refreshMsg);
        }
        // onUpdateMsg
        // After receiving the initial image, we capture any updates to the item based on market events.
        // This callback utilizes the convenient print method to display output.
        public void OnUpdateMsg(UpdateMsg updateMsg, IOmmConsumerEvent consumerEvent) 
        {
            Console.WriteLine(updateMsg);
        }
        // onStatusMsg
        // General status messages related to your consumer item registration.  For example, you will receive a 
        // status message if you register interest in an invalid item.  This callback utilizes the convenient 
        // print method to display output.	
        public void OnStatusMsg(StatusMsg statusMsg, IOmmConsumerEvent consumerEvent) 
        {
            Console.WriteLine(statusMsg);
        }
        // onAllMsg, onAckMsg, onGenericMsg
        // These callbacks are not necessary for our series of tutorials.  You can refer to the documentation
        // to better understand the purpose of these callbacks.
        public void OnAllMsg(Msg msg, IOmmConsumerEvent consumerEvent) { }
        public void OnAckMsg(AckMsg ackMsg, IOmmConsumerEvent consumerEvent) { }
        public void onGenericMsg(GenericMsg genericMSg, IOmmConsumerEvent consumerEvent) { }
    }
class Program
{
    static void Main(string[] args)
    {
        OmmConsumer? consumer = null;
            try
            {
                // instantiate callback client
                AppClient appClient = new();

                Console.WriteLine("Connecting to market data server");

                // The consumer configuration class allows the customization of the consumer (OmmConsumer) 
                // interface.
                OmmConsumerConfig config = new OmmConsumerConfig().Host("host.docker.internal:14002").UserName("user");
                // create OMM consumer
                consumer = new OmmConsumer(config);

                Console.WriteLine("Subscribing to market data");

                // subscribe to Level1 market data
                // default subscription domain is MMT_MARKET_PRICE
                consumer.RegisterClient(new RequestMsg().ServiceName("ELEKTRON_DD").Name("EUR="), appClient);
                // block this thread for a while, API calls OnRefreshMsg(), OnUpdateMsg() and OnStatusMsg()
                Thread.Sleep(60000); // 

            }
            catch (OmmException excp)
            {
                Console.WriteLine($"Exception subscribing to market data: {excp.Message}");
            }
            finally
            {
                consumer?.Uninitialize();
            }
        }
    }
