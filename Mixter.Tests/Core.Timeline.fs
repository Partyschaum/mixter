namespace Mixter.Tests.Domain.Core.Timeline

open Xunit
open Swensen.Unquote
open System
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Timeline
open Mixter.Infrastructure.Core

type ``Timeline`` ()=
    [<Fact>] 
    member x.``When handle MessageQuacked Then save TimelineMessage projection for author`` () =
        let repository = new MemoryTimelineMessageStore()
        let messageQuacked = { MessageId = MessageId.generate(); AuthorId = { Email = "A" }; Content = "Hello" }

        MessageQuacked messageQuacked |> handle repository.Save repository.Delete

        test <@ repository.GetMessagesOfUser messageQuacked.AuthorId |> Seq.toList
                 = [{ 
                        Owner = messageQuacked.AuthorId
                        Author = messageQuacked.AuthorId
                        Content = messageQuacked.Content
                        MessageId = messageQuacked.MessageId 
                    }] @>

    [<Fact>] 
    member x.``When handle MessageDeleted Then remove this message in timeline`` () =
        let repository = new MemoryTimelineMessageStore()
        let messageId = MessageId.generate()
        let author = { Email = "A" }

        MessageQuacked { MessageId = messageId; AuthorId = author; Content = "Hello" } 
        |> handle repository.Save repository.Delete

        MessageDeleted { MessageId = messageId; Deleter = author }
        |> handle repository.Save repository.Delete 

        test <@ repository.GetMessagesOfUser author |> Seq.isEmpty @>

