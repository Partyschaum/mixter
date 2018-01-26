module Mixter.Domain.Identity.SessionDescription

open UserIdentity
open Session

type SessionDescription = { UserId: UserId }
type SessionChange =
    | Created of SessionId * SessionDescription
    | Removed of SessionId

let apply event = 
    match event with
    | UserConnected e -> Created (e.SessionId, { UserId = e.UserId })
    | UserDisconnected e -> Removed e.SessionId
