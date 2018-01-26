﻿module Mixter.Infrastructure.Identity.Read

open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription

open System.Collections.Generic

type MemorySessionsStore() =
    let store = new Dictionary<SessionId, SessionDescription>()

    member __.GetSession sessionId = 
        if store.ContainsKey(sessionId) 
        then Some store.[sessionId]
        else option.None

    member __.GetUserSession userId =
        let keyValue = store |> Seq.tryFind (fun d -> d.Value.UserId = userId)
        match keyValue with
        | Some x -> Some x.Key
        | None -> None

    member __.ApplyChange change = 
        match change with 
        | Created (sessionId, session) -> store.Add (sessionId, session)
        | Removed sessionId -> store.Remove sessionId |> ignore
