﻿using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ISessionRepository
    {
        IList<Session> GetAll();
        IList<GameType> GetAllGameTypes();
        IList<Session> GetFiltered(CashGameFilter filter);
        void Add(IList<Session> sessions);
        void ReadXml();
        void SaveAsXml();
        void RemoveAll();
    }
}