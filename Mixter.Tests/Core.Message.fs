namespace Mixter.Tests.Domain.Core.Message

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

[<TestFixture>]
type ``MessageId should`` ()=
    [<Test>] 
    member x.``Return unique id when generate`` () =
        let messageId1 = MessageId.generate
        let messageId2 = MessageId.generate

        messageId1 
            |> should not' (equal messageId2)

[<TestFixture>]
type ``Given a Message`` ()=
    [<Test>] 
    member x.``When quack, then user quacked event is returned`` () =
        let messageId = MessageId.generate
        quack messageId (UserId "clem@mix-it.fr") "hello world" 
            |> should equal [ MessageQuacked { MessageId = messageId; UserId = UserId "clem@mix-it.fr"; Content = "hello world" } ]

    [<Test>] 
    member x.``When requack, then user requacked event is returned`` () =
        let messageId = MessageId.generate
        [ MessageQuacked { MessageId = messageId; UserId = UserId "clem@mix-it.fr"; Content = "hello world" } ]
            |> apply
            |> requack (UserId "someone@mix-it.fr")
            |> should equal [ MessageRequacked { MessageId = messageId } ]
            
    [<Test>] 
    member x.``When author requack, then nothing is returned`` () =
        let messageId = MessageId.generate
        let authorId = UserId "clem@mix-it.fr"
        [ MessageQuacked { MessageId = messageId; UserId = authorId; Content = "hello world" } ]
            |> apply 
            |> requack authorId
            |> should equal []
