﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

using SharedObjects;

namespace Messages
{
    [Serializable]
    public class RequestGameList : Message
    {
        //public Lobby LobbyGameList;
    }

    [Serializable]
    public class RequestGameListReply : Message
    {
        public ConcurrentDictionary<int, GameInfo> LobbyGameList = new ConcurrentDictionary<int, GameInfo>();
    }
}
