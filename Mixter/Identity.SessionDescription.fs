module Mixter.Domain.Identity.SessionDescription

open System
open UserIdentity
open Session

type SessionDescription = { SessionId: SessionId; UserId: UserId }

let apply event = 
    match event with
    | UserConnected e -> Some { SessionId = e.SessionId; UserId = e.UserId }
    | UserDisconnected e -> None
