module Mixter.Domain.Identity

open System

module UserIdentity =
    type UserId = UserId of string
        
    type Event = 
        | UserRegistered of UserRegisteredEvent
    and UserRegisteredEvent = { UserId: UserId }

    type DecisionProjection = 
        | UnregisteredUser
        | RegisteredUser of RegisteredUser
    and RegisteredUser = { UserId: UserId }

    let register userId =
        [ UserRegistered { UserId = userId } ]

    let private applyOne decisionProjection event =
        match (decisionProjection, event) with
        | (UnregisteredUser, UserRegistered e) -> RegisteredUser { UserId = e.UserId }
        | _ -> failwith "Invalid transition"

    let apply events =
        Seq.fold applyOne UnregisteredUser events

module Session = 
    open UserIdentity

    type SessionId = SessionId of string
        with static member generate () = SessionId (Guid.NewGuid().ToString())
        
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

module SessionDescription = 
    open UserIdentity
    open Session

    type SessionDescription = { SessionId: SessionId; UserId: UserId }

    let apply event = 
        match event with
        | UserConnected e -> Some { SessionId = e.SessionId; UserId = e.UserId }
        | UserDisconnected e -> None
