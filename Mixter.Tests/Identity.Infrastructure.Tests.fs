namespace Mixter.Infrastructure.Identity.Read.Tests

open NUnit.Framework
open FsUnit
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription
open Mixter.Infrastructure.Identity.Read
open System.Collections.Generic

[<TestFixture>]
type ``Given a repository of session projection`` ()=

    [<Test>]
    member x.``Given repository contains two session projection, when get a session by its id, then it returns the corresponding session projection`` () =
        let sessionId = SessionId.generate ()
        let anotherSessionId = SessionId.generate ()
        let sessions = new MemorySessionsStore()
        sessions.ApplyChange (SessionChange.Created (sessionId, { UserId = UserId "user1@mix-it.fr" }))
        sessions.ApplyChange (SessionChange.Created (anotherSessionId, { UserId = UserId "user2@mix-it.fr" }))

        sessions.GetSession sessionId
            |> should equal (Some { UserId = UserId "user1@mix-it.fr" })

    [<Test>]
    member x.``When GetUserSession with userId, then return sessionId`` () =
        let sessionId = SessionId.generate ()
        let sessions = new MemorySessionsStore()
        let userId = UserId "user1@mix-it.fr" 
        sessions.ApplyChange (SessionChange.Created (sessionId, { UserId = userId }))

        sessions.GetUserSession userId
            |> should equal (Some sessionId)

    [<Test>]
    member x.``When GetUserSession with unknown userId, then return None`` () =
        let sessions = new MemorySessionsStore()
        let userId = UserId "user1@mix-it.fr" 

        sessions.GetUserSession userId
            |> should equal None
            