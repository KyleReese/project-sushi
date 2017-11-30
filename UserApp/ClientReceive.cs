﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Messages;
using SharedObjects;
using CommSubSystem;
using CommSubSystem.ConversationClass;
//using log4net;
using System.Net;

namespace UserApp
{
    public class ClientReceive : Receiver
    {
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(ClientReceive));

        protected override void ExecuteBasedOnType(byte[] bytes, TypeOfMessage type, IPEndPoint refEp)
        {
            Conversation conv;
            switch (type)
            {
                case Envelope.TypeOfMessage.RequestGameListReply:
                    conv = null;
                    break;

                default:
                    conv = null;
                    break;
            }
            if (conv != null)
            {
                Thread thrd = new Thread(conv.Execute);
                thrd.Start();
            }
        }
    }

    
}
