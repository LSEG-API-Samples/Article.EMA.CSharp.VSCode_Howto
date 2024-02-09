/*|-----------------------------------------------------------------------------
 *|            This source code is provided under the Apache 2.0 license      --
 *|  and is provided AS IS with no warranty or guarantee of fit for purpose.  --
 *|                See the project's LICENSE.Md for details.                  --
 *|           Copyright (C) 2024 LSEG. All rights reserved.                   --
 *|-----------------------------------------------------------------------------
 */

namespace JSONUtil;

using Newtonsoft.Json;

public class RIC{
   
    public string Name {get; set;} = string.Empty;
    public string ServiceName {get; set;} = string.Empty;
    public double BID {get; set;}
    public double ASK {get; set;}
    public double NETCHNG_1 {get; set;}

    public RIC() {}

    public RIC(string ric, string serviceName, double bid = 0D, double ask = 0D, double netChange = 0D)
    {
        this.Name = ric;
        this.ServiceName = serviceName;
        this.BID = bid;
        this.ASK = ask;
        this.NETCHNG_1 = netChange;
    }
    public string ToJSON()
    {
        return JsonConvert.SerializeObject(this);
    }
}
