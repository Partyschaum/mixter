namespace Mixter.Infrastructure.Identity.Read.Tests

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription
open Mixter.Infrastructure.Identity.Read
open System.Collections.Generic

type ``Given a repository of session projection`` ()=

    [<Fact>]
    member x.``Given repository contains two session projection, when get a session by its id, then it returns the corresponding session projection`` () =
        let sessionId = SessionId.generate ()
        let anotherSessionId = SessionId.generate ()
        let sessions = new MemorySessionsStore()
        sessions.ApplyChange (SessionChange.Created (sessionId, { UserId = { Email = "user1@mix-it.fr" } }))
        sessions.ApplyChange (SessionChange.Created (anotherSessionId, { UserId = { Email = "user2@mix-it.fr" } }))

        test <@ sessions.GetSession sessionId
                    = (Some { UserId = { Email = "user1@mix-it.fr" } }) @>

    [<Fact>]
    member x.``When GetUserSession with userId, then return sessionId`` () =
        let sessionId = SessionId.generate ()
        let sessions = new MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" }
        sessions.ApplyChange (SessionChange.Created (sessionId, { UserId = userId }))

        test <@ sessions.GetUserSession userId = Some sessionId @>

    [<Fact>]
    member x.``When GetUserSession with unknown userId, then return None`` () =
        let sessions = new MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" } 

        test <@ sessions.GetUserSession userId = None @>
            