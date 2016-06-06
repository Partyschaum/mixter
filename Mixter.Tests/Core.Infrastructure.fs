namespace Mixter.Tests.Infrastructure.Core

open NUnit.Framework
open FsUnit
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Timeline
open Mixter.Infrastructure.Core
open System.Collections.Generic

[<TestFixture>]
type ``TimelineMessageRepository should`` ()=

    [<Test>]
    member x.``return messages of user when GetMessagesOfUser`` () =
        let repository = new MemoryTimelineMessageStore()
        let timelineMessage = { Owner = { Email = "A" }; Author = { Email = "A" }; Content = "Hello"; MessageId = MessageId.generate() }

        repository.Save timelineMessage

        repository.GetMessagesOfUser timelineMessage.Owner
            |> should equal [timelineMessage]

    [<Test>]
    member x.``save only one message when save two same message`` () =
        let repository = new MemoryTimelineMessageStore()
        let timelineMessage = { Owner = { Email = "A" }; Author = { Email = "A" }; Content = "Hello"; MessageId = MessageId.generate() }

        repository.Save timelineMessage
        repository.Save timelineMessage

        repository.GetMessagesOfUser timelineMessage.Owner
            |> should equal [timelineMessage]
            
    [<Test>]
    member x.``Remove message of all users when remove this message`` () =
        let repository = new MemoryTimelineMessageStore()
        let messageId = MessageId.generate()
        let user1 = { Email = "A" }
        let user2 = { Email = "B" }
        repository.Save { Owner = user1; Author = user1; Content = "Hello"; MessageId = messageId }
        repository.Save { Owner = user2; Author = user2; Content = "Hello"; MessageId = messageId }

        repository.Delete messageId

        repository.GetMessagesOfUser user1 |> should be Empty
        repository.GetMessagesOfUser user2 |> should be Empty

            