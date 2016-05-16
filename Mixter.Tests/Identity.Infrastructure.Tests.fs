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
        sessions.ApplyChange sessionId (Some { SessionId = sessionId; UserId = UserId "clem@mix-it.fr" })
        sessions.ApplyChange anotherSessionId (Some { SessionId = anotherSessionId; UserId = UserId "clem@mix-it.fr" })

        sessions.GetSession sessionId
            |> should equal (Some { UserId = UserId "clem@mix-it.fr"; SessionId = sessionId })
