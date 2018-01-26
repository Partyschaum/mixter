module Mixter.Domain.Identity.Session

open System
open Mixter.Domain.Identity.UserIdentity

type SessionId = SessionId of string
    with static member Generate () = SessionId (Guid.NewGuid().ToString())
        
type Event = 
    | UserConnected of UserConnectedEvent
    | UserDisconnected of UserDisconnectedEvent
and UserConnectedEvent = { SessionId: SessionId; UserId: UserId; ConnectedAt: DateTime}
and UserDisconnectedEvent = { SessionId: SessionId; UserId: UserId }

type DecisionProjection = 
    | NotConnectedUser
    | ConnectedUser of ConnectedUser
    | DisconnectedUser of DisconnectedUser
and DisconnectedUser = { UserId: UserId }
and ConnectedUser = { UserId: UserId; SessionId: SessionId }
    
let logIn userId generateSessionId getCurrentTime =
    [ UserConnected { SessionId = generateSessionId (); UserId = userId; ConnectedAt = getCurrentTime () } ]

let logOut decisionProjection =
    match decisionProjection with
    | ConnectedUser p -> [ UserDisconnected { SessionId = p.SessionId; UserId = p.UserId } ]
    | _ -> []

let private applyOne decisionProjection event =
    match (decisionProjection, event) with
    | (NotConnectedUser _, UserConnected e) -> ConnectedUser { UserId = e.UserId; SessionId = e.SessionId }
    | (ConnectedUser _, UserDisconnected e) -> DisconnectedUser { UserId = e.UserId }
    | _ -> failwith "Invalid transition"

let apply events =
    Seq.fold applyOne NotConnectedUser events
