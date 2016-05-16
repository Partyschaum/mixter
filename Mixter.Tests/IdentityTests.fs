namespace Mixter.Domain.Identity.Tests

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity

module UserIdentityTests =
    open UserIdentity;

    [<TestFixture>]
    type ``Given a User`` ()=

        [<Test>] 
        member x.``When he registers, then user registered event is returned`` () =
            register (UserId "clem@mix-it.fr" ) 
                |> should equal [ UserIdentity.UserRegistered { UserId = UserIdentity.UserId "clem@mix-it.fr" } ]

module SessionTests = 
    open Session

    [<TestFixture>]
    type ``Given a started session`` ()=

        [<Test>]
        member x.``When he logs in, then user connected event is returned`` () =
            let sessionId = SessionId.generate()
            let generateSessionId = fun () -> sessionId
            let getCurrentTime = fun () -> new DateTime()
            let user = UserIdentity.UserId "clem@mix-it.fr"

            logIn user generateSessionId getCurrentTime
                |> should equal [ Session.UserConnected { SessionId = sessionId; UserId = user; ConnectedAt = getCurrentTime () } ]

        [<Test>]
        member x.``When disconnect, then user disconnected event is returned`` () =
            let sessionId = SessionId.generate()
            let userId = UserIdentity.UserId "clem@mix-it.fr"
            let getCurrentTime = fun () -> new DateTime()
            [ Session.UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () }]
                |> apply
                |> logOut
                |> should equal [Session.UserDisconnected { SessionId = sessionId; UserId = userId } ]
    
        [<Test>]
        member x.``Given session have been disconnected, when disconnect, then nothing happen`` () =
            let sessionId = SessionId.generate()
            let userId = UserIdentity.UserId "clem@mix-it.fr"
            let getCurrentTime = fun () -> new DateTime()
            [  
                UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () };
                UserDisconnected { SessionId = sessionId; UserId = userId } 
            ]   |> apply
                |> logOut
                |> should equal []

open SessionDescription
open UserIdentity
open Session

[<TestFixture>]
type ``Given a handler of session events`` ()=
    let sessionId = SessionId.generate()
    let userId = UserId "clem@mix-it.fr"
    
    [<Test>]
    member x.``When project user connected, then it returns a Session projection`` () =
        let userConnected = UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = DateTime.Now}
        let expectedSession: SessionDescription =  { SessionId = sessionId; UserId = userId }

        SessionDescription.apply userConnected
            |> should equal (Some expectedSession)
            
    [<Test>]
    member x.``When project user disconnected, then it returns a Remove change of Session projection`` () =
        let userConnected = UserDisconnected { SessionId = sessionId; UserId = userId }

        SessionDescription.apply userConnected
            |> should equal None