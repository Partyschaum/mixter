module Mixter.Tests.Domain.Identity.SessionDescription

open Xunit
open Swensen.Unquote
open System
open Mixter.Domain.Identity.UserIdentity;
open Mixter.Domain.Identity.Session;
open Mixter.Domain.Identity.SessionDescription;

type ``Given a handler of session events`` ()=
    let sessionId = SessionId.Generate()
    let userId = { Email = "clem@mix-it.fr"}
    
    [<Fact>]
    member x.``When project user connected, then it returns a Session projection`` () =
        let userConnected = UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = DateTime.Now}

        test <@ apply userConnected = (Created (sessionId, { UserId = userId })) @>
            
    [<Fact>]
    member x.``When project user disconnected, then it returns a Remove change of Session projection`` () =
        let userConnected = UserDisconnected { SessionId = sessionId; UserId = userId }

        test <@ apply userConnected = Removed sessionId @>