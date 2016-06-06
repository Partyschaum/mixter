module Mixter.Tests.Domain.Identity.Session

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity;
open Mixter.Domain.Identity.Session;

[<TestFixture>]
type ``Given a started session`` ()=

    [<Test>]
    member x.``When he logs in, then user connected event is returned`` () =
        let sessionId = SessionId.generate()
        let generateSessionId = fun () -> sessionId
        let getCurrentTime = fun () -> new DateTime()
        let user = { Email = "clem@mix-it.fr" }

        logIn user generateSessionId getCurrentTime
            |> should equal [ UserConnected { SessionId = sessionId; UserId = user; ConnectedAt = getCurrentTime () } ]

    [<Test>]
    member x.``When disconnect, then user disconnected event is returned`` () =
        let sessionId = SessionId.generate()
        let userId = { Email = "clem@mix-it.fr" }
        let getCurrentTime = fun () -> new DateTime()
        [ UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () }]
            |> apply
            |> logOut
            |> should equal [UserDisconnected { SessionId = sessionId; UserId = userId } ]
    
    [<Test>]
    member x.``Given session have been disconnected, when disconnect, then nothing happen`` () =
        let sessionId = SessionId.generate()
        let userId = { Email = "clem@mix-it.fr" }
        let getCurrentTime = fun () -> new DateTime()
        [  
            UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () };
            UserDisconnected { SessionId = sessionId; UserId = userId } 
        ]   |> apply
            |> logOut
            |> should equal []