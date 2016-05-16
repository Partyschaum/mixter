module Mixter.Infrastructure.Identity.Read

open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription

open System.Collections.Generic

type MemorySessionsStore() =
    let store = new Dictionary<SessionId, SessionDescription>()

    member this.GetSession sessionId = 
        if store.ContainsKey(sessionId) 
        then Some store.[sessionId]
        else option.None

    member this.ApplyChange sessionId session = 
        match session with 
        | Some session -> store.Add (sessionId, session)
        | None -> store.Remove sessionId |> ignore
