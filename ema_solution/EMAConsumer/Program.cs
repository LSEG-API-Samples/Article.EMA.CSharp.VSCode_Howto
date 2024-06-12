/*|-----------------------------------------------------------------------------
 *|            This source code is provided under the Apache 2.0 license      --
 *|  and is provided AS IS with no warranty or guarantee of fit for purpose.  --
 *|                See the project's LICENSE.Md for details.                  --
 *|           Copyright (C) 2024 LSEG. All rights reserved.                   --
 *|-----------------------------------------------------------------------------
 */

namespace EMAConsumer;
using JSONUtil;
using System;
using System.IO;
using System.Threading;
using LSEG.Ema.Access;
using LSEG.Ema.Domain.Login;
using static LSEG.Ema.Access.DataType;
using LSEG.Ema.Rdm;
using DotNetEnv;

internal class AppClient: IOmmConsumerClient
{
    RIC _ric;
    private readonly LoginRefresh _loginRefresh = new();
    private readonly LoginStatus _loginStatus = new();

    public void SetRicObj(RIC ric)
    {
        this._ric = ric;
    }
    public void OnRefreshMsg(RefreshMsg refreshMsg, IOmmConsumerEvent consumerEvent)
    {

        Console.WriteLine("Refresh Message");
        if(refreshMsg.DomainType() == EmaRdm.MMT_LOGIN)
        {
            _loginRefresh.Clear();
            Console.WriteLine(_loginRefresh.Message(refreshMsg).ToString());
        } else
        {
            if (DataType.DataTypes.FIELD_LIST == refreshMsg.Payload().DataType)
			    Decode(refreshMsg.Payload().FieldList());

            Console.WriteLine(_ric.ToJSON());
        }

		Console.WriteLine();
    }
    public void OnUpdateMsg(UpdateMsg updateMsg, IOmmConsumerEvent consumerEvent)
    {
        Console.WriteLine("Update Message");
		if (DataTypes.FIELD_LIST == updateMsg.Payload().DataType)
			Decode(updateMsg.Payload().FieldList());
		
        Console.WriteLine(_ric.ToJSON());
		Console.WriteLine();
    }
    public void OnStatusMsg(StatusMsg statusMsg, IOmmConsumerEvent consumerEvent)
    {
        Console.WriteLine($"Received Status. Item Handle: {@consumerEvent.Handle}  Closure: {@consumerEvent.Closure}");
        Console.WriteLine($"Item Name: {(statusMsg.HasName ? statusMsg.Name() : "<not set>")}");
		Console.WriteLine("Service Name: " + (statusMsg.HasServiceName ? statusMsg.ServiceName() : "<not set>"));

		if (statusMsg.HasState)
			Console.WriteLine("Item State: " +statusMsg.State());

        if (statusMsg.DomainType() == EmaRdm.MMT_LOGIN)
        {
            _loginStatus.Clear();
            Console.WriteLine(_loginStatus.Message(statusMsg).ToString());
        }
		
		Console.WriteLine();;
    }
    public void OnAllMsg(Msg msg, IOmmConsumerEvent consumerEvent) { }
    public void OnAckMsg(AckMsg ackMsg, IOmmConsumerEvent consumerEvent) { }
    public void onGenericMsg(GenericMsg genericMSg, IOmmConsumerEvent consumerEvent) { }

    void Decode(FieldList fieldList)
    {
        foreach (var fieldEntry in fieldList)
        {
            var FieldName = fieldEntry.Name;
            _ric.GetType().GetProperty(FieldName).SetValue(_ric, Decode(fieldEntry));
        }
    }

    double Decode(FieldEntry fieldEntry)
    {
        double ReturnValue = 0D;
        if (Data.DataCode.BLANK == fieldEntry.Code)
        {
            return ReturnValue;
        }
        else
        {
            switch (fieldEntry.LoadType)
            {
                case DataTypes.REAL:
                    //Console.WriteLine(fieldEntry.OmmRealValue().AsDouble());
                    ReturnValue = fieldEntry.OmmRealValue().AsDouble();
                    break;
                default:
                    ReturnValue = 0D;
                    break;
            }
        }
        return ReturnValue;
    }
}
class Program
{
    public static string RicName { get; set; } = "JPY=";
    public static string? clientID { get; set; } = "<Client_ID>";
    public static string? clientSecret { get; set; } = "<Client_Secret>";

    static void printHelp()
	{
	    Console.WriteLine("\nOptions:\n" + "  -H\tShows this usage\n"
	    		+ "  -clientId machine account to perform authorization with the\r\n" 
	    		+ "\ttoken service (can be set via CLIENT_ID environment variable).\n"
	    		+ "  -clientSecret associated client secret with the machine id \r\n"
	    		+ "\tservice (can be set via CLIENT_SECRET environment variable).\n"
	    		+ "  -itemName Request item name (optional).\n"
	    		+ "\n");
	}

    static void readCommandlineArgs(string[] args)
	{
        try
	    {
            int argsCount = 0;
            while (argsCount < args.Length)
	        {
                if (0 == args[argsCount].CompareTo("-H"))
	            {
	                printHelp();
                    Environment.Exit(0);
	            }
                 else if ("-clientId".Equals(args[argsCount]))
    			{
                    clientID = argsCount < (args.Length - 1) ? args[++argsCount] : null;
    				++argsCount;				
    			}
	            else if ("-clientSecret".Equals(args[argsCount]))
    			{
	            	clientSecret = argsCount < (args.Length-1) ? args[++argsCount] : null;
    				++argsCount;				
    			}
    			else if ("-itemName".Equals(args[argsCount]))
    			{
    				if(argsCount < (args.Length-1))	RicName = args[++argsCount];
    				++argsCount;
    			}
                else // unrecognized command line argument
    			{
    				printHelp();
                    Environment.Exit(0);
    			}	
            }
        }    
        catch
        {
        	printHelp();
            Environment.Exit(0);
        }
    }
    static void Main(string[] args)
    {
        const string  ServiceName = "ELEKTRON_DD";
        DotNetEnv.Env.Load();
        OmmConsumer? consumer = null;
        try{
            // Get Client ID and Client Secret from Environment variable first
            clientID = Environment.GetEnvironmentVariable("CLIENT_ID") ?? "<Client_ID>";
            clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? "<Client_Secret>";
            // Get Client ID,Client Secret, and item name from command line argument
            readCommandlineArgs(args);

            RIC ric = new RIC(RicName,ServiceName);

            // instantiate callback client
            AppClient appClient = new();
            appClient.SetRicObj(ric);
            Console.WriteLine("Connecting to market data server");

            OmmConsumerConfig config = new OmmConsumerConfig().ClientId(clientID).ClientSecret(clientSecret);
            // create OMM consumer
            consumer = new OmmConsumer(config);

            Console.WriteLine("Subscribing to market data");

            LoginReq loginReq = new();
			consumer.RegisterClient(loginReq.Message(), appClient);

            OmmArray array = new()
            {
                FixedWidth = 2
            };

            array.AddInt(11) //NETCHNG_1
                .AddInt(22) //BID
				.AddInt(25) //ASK
				.Complete();
            
            var view = new ElementList()
				.AddUInt(EmaRdm.ENAME_VIEW_TYPE, 1)
				.AddArray(EmaRdm.ENAME_VIEW_DATA, array)
				.Complete();
            
            RequestMsg reqMsg = new();

            consumer.RegisterClient(reqMsg.ServiceName(ServiceName).Name(RicName).Payload(view), appClient);
            Thread.Sleep(90000); // 

        }catch (OmmException excp){
            Console.WriteLine($"Exception subscribing to market data: {excp.Message}");
        }
        finally
        {
             consumer?.Uninitialize();
        }
    }
}
