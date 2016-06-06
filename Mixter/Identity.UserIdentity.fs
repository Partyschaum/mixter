module Mixter.Domain.Identity.UserIdentity

open System

[<StructuredFormatDisplay("{Email}")>]
type UserId = { Email: string }
        
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
