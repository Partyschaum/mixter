namespace Mixter.Infrastructure.Identity.Read.Tests

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription
open Mixter.Infrastructure.Identity.Read

module ``Given a repository of session projection`` =

    [<Fact>]
    let ``Given repository contains two session projection, when get a session by its id, then it returns the corresponding session projection`` () =
        let sessionId = SessionId.Generate ()
        let anotherSessionId = SessionId.Generate ()
        let sessions = MemorySessionsStore()
        sessions.ApplyChange (SessionChange.Created (sessionId, { UserId = { Email = "user1@mix-it.fr" } }))
        sessions.ApplyChange (SessionChange.Created (anotherSessionId, { UserId = { Email = "user2@mix-it.fr" } }))

        test <@ sessions.GetSession sessionId
                    = (Some { UserId = { Email = "user1@mix-it.fr" } }) @>

    [<Fact>]
    let ``When GetUserSession with userId, then return sessionId`` () =
        let sessionId = SessionId.Generate ()
        let sessions = MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" }
        sessions.ApplyChange (SessionChange.Created (sessionId, { UserId = userId }))

        test <@ sessions.GetUserSession userId = Some sessionId @>

    [<Fact>]
    let ``When GetUserSession with unknown userId, then return None`` () =
        let sessions = MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" } 

        test <@ sessions.GetUserSession userId = None @>
            