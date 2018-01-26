module Mixter.Tests.Domain.Identity.Session

open Xunit
open Swensen.Unquote
open System
open Mixter.Domain.Identity.UserIdentity;
open Mixter.Domain.Identity.Session;

type ``Given a started session`` ()=

    [<Fact>]
    member x.``When he logs in, then user connected event is returned`` () =
        let sessionId = SessionId.generate()
        let generateSessionId = fun () -> sessionId
        let getCurrentTime = fun () -> new DateTime()
        let user = { Email = "clem@mix-it.fr" }

        test <@ logIn user generateSessionId getCurrentTime
                    = [ UserConnected { SessionId = sessionId; UserId = user; ConnectedAt = getCurrentTime () } ] @>

    [<Fact>]
    member x.``When disconnect, then user disconnected event is returned`` () =
        let sessionId = SessionId.generate()
        let userId = { Email = "clem@mix-it.fr" }
        let getCurrentTime = fun () -> new DateTime()
        let history = [ UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () }]
        
        test <@ history |> apply |> logOut
                    = [UserDisconnected { SessionId = sessionId; UserId = userId } ] @>
    
    [<Fact>]
    member x.``Given session have been disconnected, when disconnect, then nothing happen`` () =
        let sessionId = SessionId.generate()
        let userId = { Email = "clem@mix-it.fr" }
        let getCurrentTime = fun () -> new DateTime()
        let history = [  
            UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () };
            UserDisconnected { SessionId = sessionId; UserId = userId } 
        ]   
        
        test <@ history |> apply |> logOut = [] @>
        