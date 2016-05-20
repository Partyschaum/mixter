namespace Mixter.Tests.Domain.Core.Timeline

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Timeline
open Mixter.Infrastructure.Core

[<TestFixture>]
type ``Timeline`` ()=
    [<Test>] 
    member x.``When handle MessageQuacked Then save TimelineMessage projection for author`` () =
        let repository = new MemoryTimelineMessageStore()
        let messageQuacked = { MessageId = MessageId.generate(); UserId = UserId "A"; Content = "Hello" }

        MessageQuacked messageQuacked |> handle repository.Save repository.Delete

        repository.GetMessagesOfUser messageQuacked.UserId
            |> should equal [{ Owner = messageQuacked.UserId; Author = messageQuacked.UserId; Content = messageQuacked.Content; MessageId = messageQuacked.MessageId }]


    [<Test>] 
    member x.``When handle MessageDeleted Then remove this message in timeline`` () =
        let repository = new MemoryTimelineMessageStore()
        let messageId = MessageId.generate()
        let author = UserId "A"

        MessageQuacked { MessageId = messageId; UserId = author; Content = "Hello" } 
        |> handle repository.Save repository.Delete

        MessageDeleted { MessageId = messageId; Deleter = author }
        |> handle repository.Save repository.Delete 

        repository.GetMessagesOfUser author
            |> should be Empty

