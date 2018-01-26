#r "../packages/Suave/lib/net40/Suave.dll"
#r "../packages/Newtonsoft.Json/lib/portable-net45+win8+wp8+wpa81/Newtonsoft.Json.dll"

#load "Identity.UserIdentity.fs"
#load "Identity.Session.fs"
#load "Identity.SessionDescription.fs"
#load "Identity.Infrastructure.fs"
#load "EventsStore.fs"
#load "Api.fs"

open System

open Mixter
open Mixter.Domain.Identity
open UserIdentity
open Session
open Infrastructure.Identity.Read

let sessionsStore = MemorySessionsStore()

let sessionHandler (event: Session.Event) =
    SessionDescription.apply event
        |> sessionsStore.ApplyChange

let userIdentityHandler (event: UserIdentity.Event) =
    ()

let eventsHandler = Seq.iter

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

Api.start()