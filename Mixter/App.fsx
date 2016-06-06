#load "Identity.UserIdentity.fs"
#load "Identity.Session.fs"
#load "Identity.SessionDescription.fs"
#load "Identity.Infrastructure.fs"

open System
open System.Collections.Generic

open Mixter
open Mixter.Domain.Identity
open UserIdentity
open Session
open Infrastructure.Identity.Read

let sessionsStore = new MemorySessionsStore()

let sessionHandler (event: Session.Event) =
    SessionDescription.apply event
        |> sessionsStore.ApplyChange

let userIdentityHandler (event: UserIdentity.Event) =
    ()

let eventsHandler handler events =
    events |> Seq.iter handler

let simulateUserRegistration = 
    let userId = { Email = "clem@mix-it.fr" }
    userId
        |> register
        |> eventsHandler userIdentityHandler

    userId

let simulateUserLogin userId =
    let now = fun () -> DateTime.Now
    let sessionId = SessionId.generate
    
    logIn userId sessionId now 
        |> eventsHandler sessionHandler

let userId = simulateUserRegistration
simulateUserLogin userId
let sessionId = sessionsStore.GetUserSession userId

