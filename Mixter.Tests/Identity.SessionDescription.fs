module Mixter.Tests.Domain.Identity.SessionDescription

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity;
open Mixter.Domain.Identity.Session;
open Mixter.Domain.Identity.SessionDescription;

[<TestFixture>]
type ``Given a handler of session events`` ()=
    let sessionId = SessionId.generate()
    let userId = UserId "clem@mix-it.fr"
    
    [<Test>]
    member x.``When project user connected, then it returns a Session projection`` () =
        let userConnected = UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = DateTime.Now}
        let expectedSession: SessionDescription =  { SessionId = sessionId; UserId = userId }

        apply userConnected
            |> should equal (Some expectedSession)
            
    [<Test>]
    member x.``When project user disconnected, then it returns a Remove change of Session projection`` () =
        let userConnected = UserDisconnected { SessionId = sessionId; UserId = userId }

        apply userConnected
            |> should equal None